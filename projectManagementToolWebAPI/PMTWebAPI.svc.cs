using projectManagementToolWebAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace projectManagementToolWebAPI
{
    public class PMTWebAPI : IPMTWebAPI
    {
        #region Login

        /// <inheritdoc />
        public UserProfile Login(string username, string password)
        {
            ActivityLogger(new ActivityLog() { Message = "Login", Method = "Login()", UserName = username });
            UserProfile userProfile = new UserProfile();

            // Lets hash and compare the password
            try
            {
                using (var mgtdb = new MGTToolContext())
                {
                    //fetch for the salt key
                    // authenticate user
                    Users usr = mgtdb.Users.SingleOrDefault(x => x.UserName == username && x.Status == "_ACTIVE_");

                    if (usr != null)
                    {
                        string salt = usr.SaltPassword;
                        string hash = GenerateSHA256Hash(password, salt);

                        if (hash.Equals(usr.Password))
                        {
                            if (usr.Active)
                            {
                                #region Set User to the User Profile

                                var userprofile = (from u in mgtdb.Users
                                                   join r in mgtdb.UserRole on u.UserRoleId equals r.ID
                                                   where u.ID == usr.ID
                                                   select new UserProfile
                                                   {
                                                       UserId = u.ID,
                                                       Email = u.Email,
                                                       UserName = u.UserName,
                                                       Active = u.Active,
                                                       UserRole = r,
                                                       Status = u.Status,
                                                       FirstName = u.FirstName,
                                                       LastName = u.LastName,
                                                       LoggedIn = u.loggedIn,
                                                       LastLoggedIn = u.LastLogin,
                                                   }).SingleOrDefault();

                                #endregion

                                //update the table to sow the last time the logged in and to change
                                //the status to loggedin
                                usr.loggedIn = true;
                                usr.LastLogin = DateTime.Now;
                                mgtdb.SaveChanges();

                                userProfile = userprofile;
                            }
                            else
                            {
                                userProfile.HasMessage = true;
                                userProfile.Message = "The profile is not active yet";
                            }

                            mgtdb.Users.Add(new Users() { loggedIn = true, LastLogin = DateTime.Now });
                            mgtdb.SaveChanges();
                        }
                        else
                        {
                            userProfile.HasMessage = true;
                            userProfile.Message = "The Password do not macth your profile";
                        }

                    }
                    else
                    {
                        userProfile.HasMessage = true;
                        userProfile.Message = "Usename not found";
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger(new ErrorLog()
                {
                    ErrorMessage = ex.Message,
                    Method = "Login",
                    StackTrace = ex.StackTrace,
                    UserName = username
                });
            }

            return userProfile;
        }
        /// <summary>
        /// Delete user profile
        /// </summary>
        /// <param name="userId">
        /// User Id
        /// </param>
        /// <returns>
        /// True if deleted
        /// </returns>
        public bool DeleteUser(int userId)
        {
            bool result = false;
            using (var context = new MGTToolContext())
            {
                Users user = context.Users.SingleOrDefault(x => x.ID == userId);
                user.Status = "_DELETED_";
                user.Active = false;

                int isDeleted = context.SaveChanges();
                if (isDeleted > 0)
                {
                    return true;
                }
            }
            return result;
        }
        /// <inheritdoc />
        public UserProfile CreateUserProfile(string UserName, string Password, string Email, string FirstName, string LastName)
        {
            UserProfile userProfile = new UserProfile();

            #region First has the password
            string salt = GenerateSaltValue(5);
            string pwd = GenerateSHA256Hash(Password, salt);
            #endregion
            using (var context = new MGTToolContext())
            {
                try
                {
                    #region check if the profile exist
                    Users u = context.Users.SingleOrDefault(x => x.UserName == UserName);
                    if (u != null && !string.IsNullOrEmpty(u.UserName))
                    {
                        userProfile.HasMessage = true;
                        userProfile.Message = "Profile already exist, please login.";
                        return userProfile;
                    }
                    #endregion
                    Users user = new Users
                    {
                        Active = true,

                        Email = Email,
                        FirstName = FirstName,
                        LastName = LastName,
                        Status = "_ACTIVE_",
                        UserRoleId = 1,
                        loggedIn = true,
                        CreatedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now,
                        LastLogin = DateTime.Now,
                        Password = pwd,
                        UserName = UserName,
                        SaltPassword = salt
                    };

                    context.Users.Add(user);
                    int result = context.SaveChanges();

                    if (result > 0)
                    {
                        userProfile.HasMessage = true;
                        userProfile.Message = "Profile created Successfully";

                        userProfile.UserId = user.ID;
                        userProfile.UserName = user.UserName;
                        userProfile.FirstName = FirstName;
                        userProfile.LastName = LastName;
                        userProfile.Status = user.Status;
                        userProfile.LastLoggedIn = user.LastLogin;
                        userProfile.Email = Email;
                        userProfile.Active = user.Active;
                        userProfile.UserRole.ID = user.UserRoleId;
                        return userProfile;
                    }
                    else
                    {
                        userProfile.HasMessage = true;
                        userProfile.Message = "Cannot Create Profile, Please try again.";
                        return userProfile;
                    }
                }
                catch (Exception ex)
                {

                    ErrorLogger(new ErrorLog()
                    {
                        ErrorMessage = ex.Message,
                        Method = "CreateUserProfile",
                        StackTrace = ex.StackTrace,
                        TimeStamp = DateTime.Now,
                        UserName = UserName
                    });

                    userProfile.HasMessage = true;
                    userProfile.Message = ex.Message;
                    return userProfile;
                }


            }

        }
        private string GenerateSaltValue(int size)
        {
            var rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
            var buss = new byte[size];
            rng.GetBytes(buss);

            return Convert.ToBase64String(buss);
        }
        private string GenerateSHA256Hash(string password, string salt)
        {
            var bytes = System.Text.Encoding.UTF8.GetBytes(password + salt);
            System.Security.Cryptography.SHA256Managed sha256String = new System.Security.Cryptography.SHA256Managed();
            byte[] hash = sha256String.ComputeHash(bytes);
            return ByteArrayToHexString(hash);
        }
        private string ByteArrayToHexString(byte[] hash)
        {
            StringBuilder hex = new StringBuilder(hash.Length * 2);
            foreach (var item in hash)
            {
                hex.AppendFormat("{0:x2}", item);
            }
            return hex.ToString();
        }
        #endregion

        #region Client
        /// <summary>
        /// Add a client
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <param name="clientName">Client Name</param>
        /// <returns>
        /// Result message
        /// </returns>
        public string AddClient(int userId, string clientName)
        {
            string resultMessage = "";
            using (var context = new MGTToolContext())
            {
                Clients client = new Clients()
                {
                    UserId = userId,
                    ClientName = clientName,

                };
                context.Clients.Add(client);
                int result = context.SaveChanges();
                if (result > 0)
                {
                    resultMessage = "Client has been added";
                }
                else
                {
                    resultMessage = "Cannot add client";
                }
            }

            return resultMessage;
        }

        public List<Clients> GetClients(int userId)
        {
            List<Clients> client = new List<Clients>();
            using (var context = new MGTToolContext())
            {
                client = (from c in context.Clients select c).Where(x => x.UserId == userId).ToList();
            }
            return client;
        }

        /// <summary>
        /// Delete Client
        /// </summary>
        /// <param name="clientId">Client ID</param>
        /// <returns>
        /// Result Message
        /// </returns>
        public string DeleteClient(int clientId)
        {
            string resultMessage = "";
            using (var context = new MGTToolContext())
            {
                Clients client = context.Clients.SingleOrDefault(x => x.ID == clientId);
                context.Clients.Remove(client);
                int result = context.SaveChanges();
                if (result > 0)
                {
                    resultMessage = "Client deleted successfully";
                }
                else
                {
                    resultMessage = "Cannot not deleted";
                }
            }
            return resultMessage;
        }
        /// <summary>
        /// Update Client
        /// </summary>
        /// <param name="clientId">Client ID</param>
        /// <param name="client">Client Information</param>
        /// <returns>
        /// Boolean
        /// </returns>
        public bool UpdateClient(int clientId, Clients client)
        {
            using (var context = new MGTToolContext())
            {
                Clients cl = context.Clients.SingleOrDefault(x => x.ID == clientId);
                if (cl != null)
                {
                    cl.ClientName = client.ClientName;

                    int updated = context.SaveChanges();
                    if (updated > 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        #endregion

        #region Projects

        public void GetProjects(string guid) { }

        public void AddProject(int clientId)
        {
            Projects project = new Projects();

            using (var context = new MGTToolContext())
            {

                context.Projects.Add(project);
                context.SaveChanges();
            }
        }

        public void UpdateProject() { }

        public void DeleteProject() { }

        #endregion

        #region Attachments
        #endregion

        #region Logging

        /// <inheritdoc />
        public void ErrorLogger(ErrorLog errorlog)
        {
            using (var context = new MGTToolContext())
            {
                ErrorLog error = new ErrorLog()
                {
                    ErrorMessage = errorlog.ErrorMessage,
                    Method = errorlog.Method,
                    StackTrace = errorlog.StackTrace,
                    TimeStamp = DateTime.Now,
                    UserName = errorlog.UserName
                };

                context.ErrorLog.Add(error);
                context.SaveChanges();

            }
        }

        /// <summary>
        /// User activity log
        /// </summary>
        /// <param name="activiyLog">Activity</param>
        public void ActivityLogger(ActivityLog activiyLog)
        {
            using (var context = new MGTToolContext())
            {
                ActivityLog activity = new ActivityLog()
                {
                    Message = activiyLog.Message,
                    Method = activiyLog.Method,
                    TimeStamp = DateTime.Now,
                    UserName = activiyLog.UserName
                };
                context.ActivityLog.Add(activity);
                context.SaveChanges();
            }
        }
        #endregion

        #region Comments
        #endregion

        #region Client
        #endregion

    }
}
