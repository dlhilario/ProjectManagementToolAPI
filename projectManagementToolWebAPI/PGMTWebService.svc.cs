using projectManagementToolWebAPI.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
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
                                  where u.UserName.Equals(username) && u.Status == StatusEnum._ACTIVE_.ToString()
                                  select u).SingleOrDefault();


                    if (user != null)
                    {

                        if (AuthenticateUser(username, password))
                        {
                            ActivityLogger(new ActivityLog() { Message = "User login Successfully", Method = this.GetType().Name, TimeStamp = DateTime.Now, UserName = username });
                            if (user.Active)
                            {
                                #region Set User to the User Profile                

                                if (!user.Status.Equals(StatusEnum._DELETED_))
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
                                where u.UserName.Equals(username) && u.Status == StatusEnum._ACTIVE_.ToString()
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
                        Status = StatusEnum._ACTIVE_.ToString(),
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
                context.Entry(profileResult).State = EntityState.Modified;
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
                user.Status = StatusEnum._DELETED_.ToString();
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
                        Status = StatusEnum._ACTIVE_.ToString(),
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

        private List<Comments> GetListOfCommentsValue(int ProjectID, int userID)
        {
            List<Comments> comments = new List<Comments>();
            comments = GetComments(ProjectID, userID);
            return comments;
        }
        private List<Attachments> GetListOfAttachmentsValue(int projectId, int userId)
        {
            return GetAttachments(projectId, userId);

        }
        private List<MaterialList> GetListOfMaterialsValue(int projectId, int userID)
        {
            List<MaterialList> materialList = new List<MaterialList>();
            materialList = GetMaterials(projectId, userID);
            return materialList;
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
            return (!string.IsNullOrEmpty(read[columnName].ToString())) ? (DateTime?)read[columnName] : (DateTime?)null;
        }
        private double GetDoubleValue(DbDataReader read, string columnName)
        {
            return (!string.IsNullOrEmpty(read[columnName].ToString())) ? (double)read[columnName] : 0;
        }
        private int GetIntValue(DbDataReader read, string columnName)
        {
            return (!string.IsNullOrEmpty(read[columnName].ToString())) ? (int)read[columnName] : 0;
        }
        private string GetStringValue(DbDataReader read, string columnName)
        {
            return (!string.IsNullOrEmpty(read[columnName].ToString())) ? read[columnName].ToString() : string.Empty;
        }
        private byte[] GetByteArrayValue(DbDataReader read, string columnName)
        {
            return (!string.IsNullOrEmpty(read[columnName].ToString())) ? (byte[])read[columnName] : null;
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
            List<Companies> companies = new List<Companies>();
            try
            {
                using (var context = new MGTToolContext())
                {
                    DbCommand command = context.Database.Connection.CreateCommand();
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.CommandText = "SP_GetCompanyList";
                    command.Parameters.Add(new SqlParameter("@UserID", userId));
                    command.Connection.Open();

                    DbDataReader read = command.ExecuteReader();
                    while (read.Read())
                    {
                        var company = new Companies();
                        company.UserID = GetIntValue(read, nameof(company.UserID));
                        company.ID = GetIntValue(read, nameof(company.ID));
                        company.Address1 = GetStringValue(read, nameof(company.Address1));
                        company.Address2 = GetStringValue(read, nameof(company.Address2));
                        company.BusinessUrl = GetStringValue(read, nameof(company.BusinessUrl));
                        company.City = GetStringValue(read, nameof(company.City));
                        company.State = GetStringValue(read, nameof(company.State));
                        company.ZipCode = GetIntValue(read, nameof(company.ZipCode));
                        company.ZipCode = GetIntValue(read, nameof(company.ZipCode));
                        company.Status = GetStringValue(read, nameof(company.Status));
                        company.StreetNumber = GetIntValue(read, nameof(company.StreetNumber));
                        company.UpdatedByUserID = GetIntValue(read, nameof(company.UpdatedByUserID));
                        company.PhoneNumber = GetStringValue(read, nameof(company.PhoneNumber));
                        company.Name = GetStringValue(read, nameof(company.Name));
                        company.CreatedDate = (DateTime)GetDateTimeValue(read, nameof(company.CreatedDate));
                        company.IsDeleted = GetBooleanValue(read, nameof(company.IsDeleted));
                        company.ModifiedDate = (DateTime)GetDateTimeValue(read, nameof(company.ModifiedDate));
                        company.ContactPhoneNumber = GetStringValue(read, nameof(company.ContactPhoneNumber));
                        company.EmailAdress = GetStringValue(read, nameof(company.EmailAdress));

                        companies.Add(company);
                    }
                    read.Close();
                    if (command.Connection.State.Equals(ConnectionState.Open))
                        command.Connection.Close();
                    return companies;
                }
            }
            catch (Exception ex)
            {
                ErrorLogger(new ErrorLog() { ErrorMessage = ex.Message, Method = "GetCompanies", StackTrace = ex.StackTrace, UserName = GetUserName(userId) });
            }
            return new List<Companies>();
        }

        public bool CreateCompany(int userId, Companies company)
        {
            ActivityLogger(new ActivityLog() { Message = "Adding company", Method = this.GetType().Name, TimeStamp = DateTime.Now, UserName = "" });

            using (var context = new MGTToolContext())
            {
                Companies comp = (from c in context.Companies
                                  where c.UserID == userId
                                   && c.StreetNumber == company.StreetNumber
                                   && c.City == company.City
                                   && c.State == company.State
                                   && c.ZipCode == company.ZipCode
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
                                  where c.UserID == userId
                                   && c.StreetNumber == company.StreetNumber
                                   && c.City == company.City
                                   && c.State == company.State
                                   && c.ZipCode == company.ZipCode
                                   && c.ID == company.ID
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
                Companies comp = (from c in context.Companies where c.ID == companyId select c).FirstOrDefault();

                comp.Status = StatusEnum._DELETED_.ToString();
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
            List<Projects> projects = new List<Projects>();
            try
            {
                using (var context = new MGTToolContext())
                {
                    DbCommand command = context.Database.Connection.CreateCommand();
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.CommandText = "SP_GetProjectList";
                    command.Parameters.Add(new SqlParameter("@CompanyID", CompanyID));
                    command.Connection.Open();

                    DbDataReader read = command.ExecuteReader();
                    while (read.Read())
                    {
                        var project = new Projects();
                        project.ID = GetIntValue(read, nameof(project.ID));
                        project.ProjectName = GetStringValue(read, nameof(project.ProjectName));
                        project.ProjectScope = GetStringValue(read, nameof(project.ProjectScope));
                        project.Status = GetStringValue(read, nameof(project.Status));
                        project.CompanyID = GetIntValue(read, nameof(project.CompanyID));
                        project.StreetNumber = GetIntValue(read, nameof(project.StreetNumber));
                        project.StreetAddress = GetStringValue(read, nameof(project.StreetAddress));
                        project.City = GetStringValue(read, nameof(project.City));
                        project.State = GetStringValue(read, nameof(project.State));
                        project.ZipCode = GetIntValue(read, nameof(project.ZipCode));
                        project.EstimatedCompletionDate = GetDateTimeValue(read, nameof(project.EstimatedCompletionDate));
                        project.StartDate = GetDateTimeValue(read, nameof(project.StartDate));
                        project.UpdatedByUserID = GetIntValue(read, nameof(project.UpdatedByUserID));
                        project.Lot = GetStringValue(read, nameof(project.Lot));
                        project.Zone = GetStringValue(read, nameof(project.Zone));
                        project.CreatedDate = GetDateTimeValue(read, nameof(project.CreatedDate));
                        project.IsDeleted = GetBooleanValue(read, nameof(project.IsDeleted));
                        project.ModifiedDate = GetDateTimeValue(read, nameof(project.ModifiedDate));
                        project.MaterialList = GetListOfMaterialsValue(project.ID, userID);
                        project.CommentList = GetListOfCommentsValue(project.ID, userID);
                        project.CostEstimate = GetDoubleValue(read, nameof(project.CostEstimate));
                        project.AttachmentList = GetListOfAttachmentsValue(project.ID, userID);

                        projects.Add(project);
                    }
                    read.Close();
                    if (command.Connection.State.Equals(ConnectionState.Open))
                        command.Connection.Close();
                    return projects;
                }
            }
            catch (Exception ex)
            {
                ErrorLogger(new ErrorLog() { ErrorMessage = ex.Message, Method = "", StackTrace = ex.StackTrace, UserName = GetUserName(userID), });
            }
            return new List<Projects>();
        }

        /// <inheritdoc />
        public Projects GetProject(int ProjectId, int userID)
        {
            Projects project = new Projects();
            try
            {
                using (var context = new MGTToolContext())
                {
                    DbCommand command = context.Database.Connection.CreateCommand();
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.CommandText = "SP_GetProjectSelect";
                    command.Parameters.Add(new SqlParameter("@ProjectID", ProjectId));
                    command.Connection.Open();

                    DbDataReader read = command.ExecuteReader();
                    while (read.Read())
                    {
                        project.ID = GetIntValue(read, nameof(project.ID));
                        project.ProjectName = GetStringValue(read, nameof(project.ProjectName));
                        project.ProjectScope = GetStringValue(read, nameof(project.ProjectScope));
                        project.CompanyID = GetIntValue(read, nameof(project.CompanyID));
                        project.StreetNumber = GetIntValue(read, nameof(project.StreetNumber));
                        project.StreetAddress = GetStringValue(read, nameof(project.StreetAddress));
                        project.CostEstimate = GetDoubleValue(read, nameof(project.CostEstimate));
                        project.City = GetStringValue(read, nameof(project.City));
                        project.State = GetStringValue(read, nameof(project.State));
                        project.ZipCode = GetIntValue(read, nameof(project.ZipCode));
                        project.EstimatedCompletionDate = GetDateTimeValue(read, nameof(project.EstimatedCompletionDate));
                        project.StartDate = GetDateTimeValue(read, nameof(project.StartDate));
                        project.UpdatedByUserID = GetIntValue(read, nameof(project.UpdatedByUserID));
                        project.Lot = GetStringValue(read, nameof(project.Lot));
                        project.Zone = GetStringValue(read, nameof(project.Zone));
                        project.CreatedDate = GetDateTimeValue(read, nameof(project.CreatedDate));
                        project.IsDeleted = GetBooleanValue(read, nameof(project.IsDeleted));
                        project.ModifiedDate = GetDateTimeValue(read, nameof(project.ModifiedDate));
                        project.MaterialList = GetListOfMaterialsValue(project.ID, userID);
                        project.CommentList = GetListOfCommentsValue(project.ID, userID);
                        project.AttachmentList = GetListOfAttachmentsValue(project.ID, userID);

                    }
                    read.Close();
                    if (command.Connection.State.Equals(ConnectionState.Open))
                        command.Connection.Close();
                    return project;
                }
            }
            catch (Exception ex)
            {
                ErrorLogger(new ErrorLog() { ErrorMessage = ex.Message, Method = "", StackTrace = ex.StackTrace, UserName = GetUserName(userID), });
            }
            return new Projects();
        }

        /// <inheritdoc />
        public ProjectResponse AddProject(int userId, Projects project)
        {
            ProjectResponse response = new ProjectResponse();
            try
            {
                using (var context = new MGTToolContext())
                {
                    Projects proj = new Projects();
                    proj = project;
                    proj.CreatedDate = DateTime.Now;
                    proj.ModifiedDate = DateTime.Now;
                    proj.IsDeleted = false;
                    proj.Status = project.Status;
                    proj.UpdatedByUserID = userId;

                    context.Entry(proj).State = EntityState.Added;
                    context.Projects.Add(proj);
                    int result = context.SaveChanges();
                    if (result > 0)
                    {
                        response.Success = true;
                        response.ProjectID = proj.ID;
                        response.StatusMessage = "Project has been added";
                    }
                    else
                    {
                        response.StatusMessage = "Project can not be added";
                    }
                }
            }
            catch (Exception ex)
            {
                response.HasHerror = true;
                response.ErrorMessage = ex.Message;
                ErrorLogger(new ErrorLog() { ErrorMessage = ex.Message, Method = this.GetType().Name, TimeStamp = DateTime.Now, UserName = GetUserName(userId) });
            }

            return response;
        }
        /// <inheritdoc />
        public ProjectResponse UpdateProject(int userID, Projects project)
        {

            ProjectResponse response = new ProjectResponse();
            using (var context = new MGTToolContext())
            {
                try
                {
                    Projects proj = context.Projects.AsNoTracking().SingleOrDefault(x => x.ID == project.ID && x.CompanyID == project.CompanyID);
                    if (proj != null)
                    {
                        proj = project;
                        proj.ModifiedDate = DateTime.Now;
                        proj.IsDeleted = false;
                        proj.Status = project.Status;
                        proj.CostEstimate = project.CostEstimate;
                        proj.UpdatedByUserID = userID;
                        foreach (Comments comment in project.CommentList)
                        {
                            InsertComments(comment.ProjectID, comment.UpdatedByUserID, comment.Comment);
                        }
                        foreach (Attachments attachment in project.AttachmentList)
                        {
                            // check if exist update else insert
                        }
                        foreach (MaterialList item in project.MaterialList)
                        {
                            // check if exist update else insert
                        }



                        int result = context.SaveChanges();
                        if (result > 0)
                        {
                            response.ProjectID = proj.ID;
                            response.Success = true;
                            response.StatusMessage = "Project has been updated.";
                        }
                        else
                        {
                            response.HasHerror = true;
                            response.Success = false;
                            response.ErrorMessage = "Project can not be updated.";
                        }
                    }
                    else
                    {
                        response.HasHerror = true;
                        response.ErrorMessage = "Project can not be found.";
                    }
                }
                catch (Exception ex)
                {

                    response.HasHerror = true;
                    response.ErrorMessage = $"Fatal Error: {ex.Message}";
                }


            }
            return response;
        }
        /// <inheritdoc />
        public bool DeleteProject(int userID, int projectID)
        {
            using (var context = new MGTToolContext())
            {
                var proj = context.Projects.SingleOrDefault(x => x.ID == projectID);
                proj.IsDeleted = true;
                proj.Status = StatusEnum._DELETED_.ToString();
                proj.ModifiedDate = DateTime.Now;
                proj.UpdatedByUserID = userID;
                context.Entry(proj).State = EntityState.Deleted;
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

        public AttachmentResponse AddAttachments(int userId, Attachments attachments)
        {
            AttachmentResponse response = new AttachmentResponse();
            try
            {
                using (var context = new MGTToolContext())
                {
                    Attachments att = new Attachments();
                    att = attachments;
                    att.CreatedDate = DateTime.Now;
                    context.Attachments.Add(att);
                    context.Entry(att).State = EntityState.Added;
                    int result = context.SaveChanges();
                    if (result > 0)
                    {
                        response.Success = true;
                        response.AttachmentID = att.ID;
                        response.StatusMessage = "Attachment Added Successfully";
                    }
                    else
                    {
                        response.StatusMessage = "Unable to add Attachement";
                    }

                }
            }
            catch (Exception ex)
            {
                response.HasHerror = true;
                response.ErrorMessage = ex.Message;
                ErrorLogger(new ErrorLog() { ErrorMessage = ex.Message, Method = this.GetType().Name, TimeStamp = DateTime.Now, UserName = GetUserName(userId) });
            }
            return response;

        }
        /// <inheritdoc />
        public Attachments GetAttachmentById(int attachmentID)
        {
            Attachments attachments = new Attachments();
            using (var context = new MGTToolContext())
            {
                attachments = context.Attachments.SingleOrDefault(x => x.ID == attachmentID);
            }
            return attachments;
        }
        /// <inheritdoc />
        public List<Attachments> GetAttachments(int ProjectId, int userId)
        {
            List<Attachments> attachments = new List<Attachments>();
            using (var context = new MGTToolContext())
            {
                DbCommand command = context.Database.Connection.CreateCommand();
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "SP_GetAttachements";
                command.Parameters.Add(new SqlParameter("@ProjectID", ProjectId));
                command.Connection.Open();

                DbDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Attachments attachment = new Attachments();
                    attachment.ID = GetIntValue(reader, nameof(attachment.ID));
                    attachment.ProjectID = GetIntValue(reader, nameof(attachment.ProjectID));
                    attachment.FileName = GetStringValue(reader, nameof(attachment.FileName));
                    attachment.FileSize = GetIntValue(reader, nameof(attachment.FileSize));
                    attachment.FileType = GetStringValue(reader, nameof(attachment.FileType));
                    attachment.Description = GetStringValue(reader, nameof(attachment.Description));
                    attachment.Document = GetByteArrayValue(reader, nameof(attachment.Document));
                    attachment.CreatedDate = (DateTime)GetDateTimeValue(reader, nameof(attachment.CreatedDate));
                    attachments.Add(attachment);
                }

                reader.Close();
                if (command.Connection.State.Equals(ConnectionState.Open))
                    command.Connection.Close();
            }
            return attachments;
        }


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
        public List<Comments> GetComments(int projectId, int userID)
        {
            List<Comments> comments = new List<Comments>();
            try
            {
                using (var context = new MGTToolContext())
                {
                    DbCommand command = context.Database.Connection.CreateCommand();
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.CommandText = "SP_GetCommentList";
                    command.Parameters.Add(new SqlParameter("@ProjectId", projectId));
                    command.Connection.Open();

                    DbDataReader read = command.ExecuteReader();
                    while (read.Read())
                    {
                        var comment = new Comments();
                        comment.ID = GetIntValue(read, nameof(comment.ID));
                        comment.ProjectID = GetIntValue(read, nameof(comment.ProjectID));
                        comment.Comment = GetStringValue(read, nameof(comment.Comment));
                        comment.UpdatedByUserID = GetIntValue(read, nameof(comment.UpdatedByUserID));
                        comment.Time_Stamp = (DateTime)GetDateTimeValue(read, nameof(comment.Time_Stamp));

                        comments.Add(comment);
                    }
                    read.Close();
                    if (command.Connection.State.Equals(ConnectionState.Open))
                        command.Connection.Close();
                    return comments;
                }
            }
            catch (Exception ex)
            {
                ErrorLogger(new ErrorLog() { ErrorMessage = ex.Message, Method = "", StackTrace = ex.StackTrace, UserName = GetUserName(userID), });
            }
            return new List<Comments>();
        }

        public int InsertComments(int? projectId, int? userID, string comment)
        {
            int commentId = default(int);
            using (var entity = new MGTToolContext())
            {
                DbCommand command = entity.Database.Connection.CreateCommand();
                command.Connection.Open();
                command.CommandText = "SP_Comments_Insert";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@ProjectID", projectId));
                command.Parameters.Add(new SqlParameter("@Comments", comment));
                command.Parameters.Add(new SqlParameter("@UserID", userID));
                command.Parameters.Add(new SqlParameter("@CommentID", ParameterDirection.Output));
                DbDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    int.TryParse(reader["CommentID"].ToString(), out commentId);
                }
                if (command.Connection.State == ConnectionState.Open)
                    command.Connection.Close();
                reader.Close();

            }
            return commentId;
        }
        #endregion

        #region Material 
        public List<MaterialList> GetMaterials(int projectId, int userID)
        {
            List<MaterialList> materialList = new List<MaterialList>();
            try
            {
                using (var context = new MGTToolContext())
                {
                    DbCommand command = context.Database.Connection.CreateCommand();
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.CommandText = "SP_GetMaterialList";
                    command.Parameters.Add(new SqlParameter("@ProjectId", projectId));
                    command.Connection.Open();

                    DbDataReader read = command.ExecuteReader();
                    while (read.Read())
                    {
                        var material = new MaterialList();
                        material.ID = GetIntValue(read, nameof(material.ID));
                        material.InvoiceNumber = GetStringValue(read, nameof(material.InvoiceNumber));
                        material.ItemDescription = GetStringValue(read, nameof(material.ItemDescription));
                        material.ItemName = GetStringValue(read, nameof(material.ItemName));
                        material.ItemQuantity = GetIntValue(read, nameof(material.ItemQuantity));
                        material.Price = GetDoubleValue(read, nameof(material.Price));
                        material.ProjectID = GetIntValue(read, nameof(material.ProjectID));

                        material.PurchaseDate = GetDateTimeValue(read, nameof(material.PurchaseDate));

                        materialList.Add(material);
                    }
                    read.Close();
                    if (command.Connection.State.Equals(ConnectionState.Open))
                        command.Connection.Close();
                    return materialList;
                }
            }
            catch (Exception ex)
            {
                ErrorLogger(new ErrorLog() { ErrorMessage = ex.Message, Method = "", StackTrace = ex.StackTrace, UserName = GetUserName(userID), });
            }
            return new List<MaterialList>();
        }
        #endregion

        #region Project Status
        public List<ProjectStatus> GetProjectStatuses()
        {
            List<ProjectStatus> ps = new List<ProjectStatus>();
            using (var entity = new MGTToolContext())
            {
                ps = entity.ProjectStatus.ToList();

                return ps;
            }
        }
        #endregion

        #region Overview
        public OverviewDetails GetOverView(int CompanyId, int UserId)
        {
            OverviewDetails od = new OverviewDetails();
            using (var entity = new MGTToolContext())
            {
                var command = entity.Database.Connection.CreateCommand();
                command.CommandText = "SP_GetProjectOverview";
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@CompanyId", CompanyId));
                command.Parameters.Add(new SqlParameter("@UserId", UserId));
                command.Connection.Open();

                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    od.TotalProjects = GetIntValue(reader, nameof(od.TotalProjects));
                    od.TotalOpen = GetIntValue(reader, nameof(od.TotalOpen));
                    od.TotalCompleted = GetIntValue(reader, nameof(od.TotalCompleted));
                    od.TotalDeleted = GetIntValue(reader, nameof(od.TotalDeleted));
                    od.TotalOnHold = GetIntValue(reader, nameof(od.TotalOnHold));
                    od.TotalPending = GetIntValue(reader, nameof(od.TotalPending));
                    od.TotalCanceled = GetIntValue(reader, nameof(od.TotalCanceled));
                }
                reader.Close();
                if (command.Connection.State.Equals(ConnectionState.Open))
                    command.Connection.Close();
            }

            return od;
        }
        #endregion

    }
}
