using projectManagementToolWebAPI.Model;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace projectManagementToolWebAPI
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "PGMTWebService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select PGMTWebService.svc or PGMTWebService.svc.cs at the Solution Explorer and start debugging.
    public class PGMTWebService : IPGMTWebService
    {
        #region Login

        /// <inheritdoc />
        public Users Login(string username, string password)
        {

            ActivityLogger(new ActivityLog() { Message = "User Attempting to login", Method = this.GetType().Name, TimeStamp = DateTime.Now, UserName = username });

            // Lets hash and compare the password
            try
            {
                using (var context = new MGTToolContext())
                {
                    //fetch for the salt key
                    // authenticate user
                    Users user = (from u in context.Users
                                  where u.UserName.Equals(username) && u.Status.Equals(Status.ACTIVE)
                                  select u).SingleOrDefault();


                    if (user != null)
                    {

                        if (AuthenticateUser(username, password))
                        {
                            ActivityLogger(new ActivityLog() { Message = "User login Successfully", Method = this.GetType().Name, TimeStamp = DateTime.Now, UserName = username });
                            if (user.Active)
                            {
                                #region Set User to the User Profile                

                                if (!user.Status.Equals(Status.DELETED))
                                {
                                    user = UpdateUserLoginStatus(user.ID, true);
                                    user.UserProfile = new UserProfile();
                                    var profile = GetUserProfile(user);
                                    user.UserProfile = new UserProfile()
                                    {
                                        ID = profile.ID,
                                        Active = profile.Active,
                                        UserName = profile.UserName,
                                        Email = profile.Email,
                                        FirstName = profile.FirstName,
                                        LastName = profile.LastName,
                                        HasMessage = profile.HasMessage,
                                        Message = profile.Message,
                                        LoggedIn = profile.LoggedIn,
                                        LastLoggedIn = profile.LastLoggedIn,
                                        Status = profile.Status,
                                        StreetNumber = profile.StreetNumber,
                                        UserRoleId = profile.UserRoleId,
                                        UserId = profile.UserId
                                    };
                                    user.UserProfileID = user.UserProfile.ID;

                                    var role = GetUserRole(user.UserRoleId);
                                    user.UserRole = new UserRole()
                                    {

                                        Description = role.Description,
                                        Role = role.Role,
                                        ID = role.ID
                                    };


                                    return user;

                                }
                                else
                                {
                                    UserProfile userProfile = new UserProfile();
                                    userProfile.HasMessage = true;
                                    userProfile.Message = "User has been deleted, please create a new account, or contact Support if you think this is an error.";
                                    return user;
                                }

                                #endregion

                                //update the table to sow the last time the logged in and to change
                                //the status to loggedin


                            }
                            else
                            {
                                UserProfile userProfile = new UserProfile();
                                userProfile.HasMessage = true;
                                userProfile.Message = "The User is not active yet";
                                return user;
                            }

                        }
                        else
                        {
                            UserProfile userProfile = new UserProfile();
                            userProfile.HasMessage = true;
                            userProfile.Message = "Unable to login, check your username or password.";
                            return user;
                        }

                    }
                    else
                    {
                        UserProfile userProfile = new UserProfile
                        {
                            HasMessage = true,
                            Message = "Username not found"
                        };
                        return user;
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

            return new Users();
        }

        private bool AuthenticateUser(string username, string password)
        {
            ActivityLogger(new ActivityLog() { Message = "Login", Method = "Login()", UserName = username });

            // Lets hash and compare the password
            try
            {
                using (var mgtdb = new MGTToolContext())
                {
                    //fetch for the salt key
                    // authenticate user
                    var user = (from u in mgtdb.Users
                                where u.UserName.Equals(username) && u.Status.Equals(Status.ACTIVE)
                                select u).SingleOrDefault();

                    if (user != null)
                    {
                        string salt = user.SaltPassword;
                        string hash = GenerateSHA256Hash(password, salt);

                        if (hash.Equals(user.Password))
                        {
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger(new ErrorLog() { ErrorMessage = ex.Message, Method = "AuthenticateUser", StackTrace = ex.StackTrace, TimeStamp = DateTime.Now, UserName = username });
            }
            return false;
        }

        public List<Users> GetUsers()
        {
            using (var context = new MGTToolContext())
            {
                List<Users> users = context.Users.ToList();
                return users;
            }
        }
        public Users UpdateUserLoginStatus(int userId, bool LoggedIn)
        {
            using (var context = new MGTToolContext())
            {
                Users usr = context.Users.SingleOrDefault(x => x.ID.Equals(userId));
                usr.LoggedIn = LoggedIn;
                usr.ModifiedDate = DateTime.Now;
                usr.LastLogin = DateTime.Now;
                context.SaveChanges();

                return usr;
            }
        }
        public UserProfile GetUserProfile(Users user)
        {
            using (var context = new MGTToolContext())
            {
                UserProfile userProfile = (from upf in context.UserProfile where upf.UserId.Equals(user.ID) select upf).FirstOrDefault();
                if (userProfile == null)
                {
                    userProfile = new UserProfile()
                    {
                        Active = true,
                        UserId = user.ID,
                        UserName = user.UserName,
                        Email = user.Email,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Status = Status.ACTIVE,
                        UserRoleId = user.UserRoleId,
                        HasMessage = false,
                        Message = string.Empty,
                        StreetNumber = string.Empty,
                        LastLoggedIn = DateTime.Now,
                        LoggedIn = true
                    };
                    context.UserProfile.Add(userProfile);
                    int result = context.SaveChanges();
                    if (result > 0)
                    {

                        return userProfile;
                    }
                    else
                    {
                        userProfile = new UserProfile();
                        userProfile.HasMessage = true;
                        userProfile.Message = "Unable to create user profile please try again or contact Support";
                        return userProfile;
                    }
                }

                return userProfile;
            }
        }
        /// <inheritdoc />
        public bool UpdateUserProfile(int userId, UserProfile profile)
        {
            using (var context = new MGTToolContext())
            {
                UserProfile profileResult = context.UserProfile.SingleOrDefault(x => x.UserId.Equals(userId) && x.ID.Equals(profile.ID));
                profileResult = profile;
                int result = context.SaveChanges();
                if (result > 0)
                {
                    ActivityLogger(new ActivityLog() { Message = "User Profile Updated Successfully", Method = this.GetType().Name, TimeStamp = DateTime.Now, UserName = GetUserName(userId) });

                    return true;
                }
                else
                {
                    ActivityLogger(new ActivityLog() { Message = "Unable to Update User Profile", Method = this.GetType().Name, TimeStamp = DateTime.Now, UserName = GetUserName(userId) });

                    return false;
                }
            }
        }

        public bool UpdateUser(Users user)
        {
            using (var context = new MGTToolContext())
            {
                Users usr = context.Users.SingleOrDefault(x => x.ID.Equals(user.ID));
                usr = user;
                usr.ModifiedDate = DateTime.Now;
                int result = context.SaveChanges();
                if (result > 0)
                {
                    return true;
                }

            }
            return false;
        }
        /// <inheritdoc />
        public UserRole GetUserRole(int roleID)
        {
            using (var context = new MGTToolContext())
            {
                UserRole role = context.UserRole.SingleOrDefault(x => x.ID.Equals(roleID));
                return role;
            }
        }
        /// <inheritdoc />
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
                        LoggedIn = true,
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
                        userProfile.FirstName = FirstName;
                        userProfile.LastName = LastName;
                        userProfile.Status = user.Status;
                        userProfile.LastLoggedIn = user.LastLogin;
                        userProfile.Email = Email;
                        userProfile.Active = user.Active;
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

        private List<Comments> GetListOfCommentsValue(DbDataReader read, string columnName)
        {
            return (!string.IsNullOrEmpty(read[columnName].ToString())) ? (List<Comments>)read[columnName] : new List<Comments>();
        }
        private List<Attachments> GetListOfAttachmentsValue(DbDataReader read, string columnName)
        {
            return (!string.IsNullOrEmpty(read[columnName].ToString())) ? (List<Attachments>)read[columnName] : new List<Attachments>();
        }
        private List<MaterialList> GetListOfMaterialsValue(DbDataReader read, string columnName)
        {
            return (!string.IsNullOrEmpty(read[columnName].ToString())) ? (List<MaterialList>)read[columnName] : new List<MaterialList>();
        }
        private List<Projects> GetListOfProjectsValue(DbDataReader read, string columnName)
        {
            return (!string.IsNullOrEmpty(read[columnName].ToString())) ? (List<Projects>)read[columnName] : new List<Projects>();
        }
        private bool GetBooleanValue(DbDataReader read, string columnName)
        {
            return (!string.IsNullOrEmpty(read[columnName].ToString())) ? (bool)read[columnName] : false;
        }
        private DateTime? GetDateTimeValue(DbDataReader read, string columnName)
        {
            return (!string.IsNullOrEmpty(read[columnName].ToString())) ? (DateTime)read[columnName] : (DateTime?)null;
        }
        private int GetIntValue(DbDataReader read, string columnName)
        {
            return (!string.IsNullOrEmpty(read[columnName].ToString())) ? (int)read[columnName] : 0;
        }
        private string GetStringValue(DbDataReader read, string columnName)
        {
            return (!string.IsNullOrEmpty(read[columnName].ToString())) ? read[columnName].ToString() : string.Empty;
        }

        public string GetUserName(int userId)
        {
            using (var context = new MGTToolContext())
            {
                var usr = context.Users.SingleOrDefault(x => x.ID == userId);
                return usr.UserName;
            }
        }

        #endregion

        #region Company
        public List<Companies> GetCompanies(int userId)
        {
            using (var context = new MGTToolContext())
            {
                List<Companies> companies = (from c in context.Companies where c.UserID.Equals(userId) && c.IsDeleted != true select c).ToList();
                return companies;
            }
        }

        public bool CreateCompany(int userId, Companies company)
        {
            ActivityLogger(new ActivityLog() { Message = "Adding company", Method = this.GetType().Name, TimeStamp = DateTime.Now, UserName = "" });

            using (var context = new MGTToolContext())
            {
                Companies comp = (from c in context.Companies
                                  where c.UserID.Equals(userId)
                                   && c.StreetNumber.Equals(company.StreetNumber)
                                   && c.City.Equals(company.City)
                                   && c.State.Equals(company.State)
                                   && c.ZipCode.Equals(company.ZipCode)
                                  select c).FirstOrDefault();
                if (comp != null)
                {
                    ActivityLogger(new ActivityLog() { Message = "Company Already Exist", Method = this.GetType().Name, TimeStamp = DateTime.Now, UserName = GetUserName(userId) });

                    return false;
                }
                else
                {

                    comp = new Companies();
                    comp = company;
                    comp.ModifiedDate = DateTime.Now;
                    comp.UpdatedByUserID = userId;

                    context.Companies.Add(comp);
                    int result = context.SaveChanges();
                    if (result > 0)
                    {
                        ActivityLogger(new ActivityLog() { Message = "Company Added Successfully", Method = this.GetType().Name, TimeStamp = DateTime.Now, UserName = GetUserName(userId) });
                        return true;
                    }
                    ActivityLogger(new ActivityLog() { Message = "Unable to Create Company", Method = this.GetType().Name, TimeStamp = DateTime.Now, UserName = GetUserName(userId) });

                    return false;
                }

            }
        }

        public bool UpdateCompany(int userId, Companies company)
        {
            using (var context = new MGTToolContext())
            {
                Companies comp = (from c in context.Companies
                                  where c.UserID.Equals(userId)
                                   && c.StreetNumber.Equals(company.StreetNumber)
                                   && c.City.Equals(company.City)
                                   && c.State.Equals(company.State)
                                   && c.ZipCode.Equals(company.ZipCode)
                                   && c.ID.Equals(company.ID)
                                  select c).FirstOrDefault();


                comp = new Companies();
                comp = company;
                comp.CreatedDate = DateTime.Now;
                comp.ModifiedDate = DateTime.Now;
                comp.UserID = userId;

                int result = context.SaveChanges();

                if (result > 0)
                {
                    return true;
                }

                return false;
            }

        }

        public bool DeleteCompany(int userId, int companyId)
        {
            using (var context = new MGTToolContext())
            {
                Companies comp = (from c in context.Companies where c.ID.Equals(companyId) select c).FirstOrDefault();

                comp.Status = Status.DELETED;
                comp.IsDeleted = true;
                comp.ModifiedDate = DateTime.Now;
                comp.UpdatedByUserID = userId;

                int result = context.SaveChanges();

                if (result > 0)
                {
                    return true;
                }

                return false;
            }

        }


        #endregion

        #region Projects
        /// <inheritdoc />
        public List<Projects> GetProjects(int CompanyID, int userID)
        {
            List<Projects> companies = new List<Projects>();
            try
            {
                using (var context = new MGTToolContext())
                {
                    DbCommand command = context.Database.Connection.CreateCommand();
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.CommandText = "SP_GetProjectList";
                    command.Parameters.Add(CompanyID);

                    DbDataReader read = command.ExecuteReader();
                    while (read.Read())
                    {
                        var com = new Projects();
                        com.ID = GetIntValue(read, nameof(com.ID));
                        com.ProjectName = GetStringValue(read, nameof(com.ProjectName));
                        com.ProjectScope = GetStringValue(read, nameof(com.ProjectScope));
                        com.CompanyID = GetIntValue(read, nameof(com.CompanyID));
                        com.StreetNumber = GetIntValue(read, nameof(com.StreetNumber));
                        com.StreetAddress = GetStringValue(read, nameof(com.StreetAddress));
                        com.City = GetStringValue(read, nameof(com.City));
                        com.State = GetStringValue(read, nameof(com.State));
                        com.ZipCode = GetIntValue(read, nameof(com.ZipCode));
                        com.EstimatedCompletionDate = GetDateTimeValue(read, nameof(com.EstimatedCompletionDate));
                        com.StartDate = GetDateTimeValue(read, nameof(com.StartDate));
                        com.UpdatedByUserID = GetIntValue(read, nameof(com.UpdatedByUserID));
                        com.Lot = GetStringValue(read, nameof(com.Lot));
                        com.Zone = GetStringValue(read, nameof(com.Zone));
                        com.CreatedDate = (DateTime)GetDateTimeValue(read, nameof(com.CreatedDate));
                        com.IsDeleted = GetBooleanValue(read, nameof(com.IsDeleted));
                        com.ModifiedDate = (DateTime)GetDateTimeValue(read, nameof(com.ModifiedDate));
                        com.MaterialList = GetListOfMaterialsValue(read, nameof(com.MaterialList));
                        com.AttachmentList = GetListOfAttachmentsValue(read, nameof(com.AttachmentList));
                        com.CommentList = GetListOfCommentsValue(read, nameof(com.CommentList));

                        companies.Add(com);
                    }
                    read.Close();
                    return companies;
                }
            }
            catch (Exception ex)
            {
                ErrorLogger(new ErrorLog() { ErrorMessage = ex.Message, Method = "", StackTrace = ex.StackTrace, UserName = GetUserName(userID), });
            }
            return new List<Projects>();
        }
        /// <inheritdoc />
        public string AddProject(int userId, Projects project)
        {
            string resultMessage = "";
            using (var context = new MGTToolContext())
            {
                Projects proj = new Projects();
                proj = project;
                proj.CreatedDate = DateTime.Now;
                proj.ModifiedDate = DateTime.Now;
                proj.IsDeleted = false;
                proj.Status = Status.ACTIVE;
                proj.UpdatedByUserID = userId;

                context.Projects.Add(proj);
                int result = context.SaveChanges();
                if (result > 0)
                {
                    resultMessage = "Project has been added";
                }
                else
                {
                    resultMessage = "Project can not be added";
                }
            }
            return resultMessage;
        }
        /// <inheritdoc />
        public string UpdateProject(int userID, Projects project)
        {

            string resultMessage = "";
            using (var context = new MGTToolContext())
            {
                Projects proj = context.Projects.SingleOrDefault(x => x.ID == project.ID);
                proj = project;
                proj.ModifiedDate = DateTime.Now;
                proj.IsDeleted = false;
                proj.Status = Status.ACTIVE;
                proj.UpdatedByUserID = userID;

                context.Projects.Add(proj);
                int result = context.SaveChanges();
                if (result > 0)
                {
                    resultMessage = "Project has been updated";
                }
                else
                {
                    resultMessage = "Project can not be updated";
                }
            }
            return resultMessage;
        }
        /// <inheritdoc />
        public bool DeleteProject(int userID, int projectID)
        {
            using (var context = new MGTToolContext())
            {
                var proj = context.Projects.SingleOrDefault(x => x.ID == projectID);
                proj.IsDeleted = true;
                proj.Status = Status.DELETED;
                proj.ModifiedDate = DateTime.Now;
                proj.UpdatedByUserID = userID;
                int result = context.SaveChanges();
                if (result > 0)
                {
                    return true;
                }
                return false;
            }
        }

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
            using (var entity = new MGTToolContext())
            {
                ActivityLog activity = new ActivityLog()
                {
                    Message = activiyLog.Message,
                    Method = activiyLog.Method,
                    TimeStamp = DateTime.Now,
                    UserName = activiyLog.UserName
                };
                entity.ActivityLog.Add(activity);
                entity.SaveChanges();
            }
        }
        #endregion

        #region Comments
        #endregion

    }
}
