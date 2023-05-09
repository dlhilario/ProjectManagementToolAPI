using projectManagementToolWebAPI.Model;
using System.Collections.Generic;
using System.ServiceModel;

namespace projectManagementToolWebAPI
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IPGMTWebService" in both code and config file together.
    [ServiceContract]
    public interface IPGMTWebService
    {
        /// <summary>
        /// User Login 
        /// </summary>
        /// <param name="username">User Name</param>
        /// <param name="password">Password</param>
        /// <returns>
        /// User Profile in session
        /// </returns>
        [OperationContract]
        Users Login(string username, string password);
        /// <summary>
        /// User Role
        /// </summary>
        /// <param name="roleID"></param>
        /// <returns></returns>
        [OperationContract]
        UserRole GetUserRole(int roleID);
        /// <summary>
        /// Get a list of users
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<Users> GetUsers();
        /// <summary>
        /// Create user Profile
        /// </summary>
        /// <param name="userInfo">User Object</param>
        /// <returns>
        /// Return result
        /// </returns>
        [OperationContract]
        UserProfile CreateUserProfile(string UserName, string Password, string Email, string FirstName, string LastName);
        /// <summary>
        /// Error logger
        /// </summary>
        /// <param name="errorlog">ErrorLog Object</param>
        [OperationContract]
        void ErrorLogger(ErrorLog errorlog);
        /// <summary>
        /// Update user
        /// </summary>
        /// <param name="user">User Object</param>
        /// <returns>boolean</returns>
        [OperationContract]
        bool UpdateUser(Users user);
        /// <summary>
        /// User activity log
        /// </summary>
        /// <param name="activiyLog">Activity
        /// </param>
        [OperationContract]
        void ActivityLogger(ActivityLog activiyLog);
        /// <summary>
        /// Delete user profile
        /// </summary>
        /// <param name="userId">
        /// User Id
        /// </param>
        /// <returns>
        /// True if deleted
        /// </returns
        [OperationContract]
        bool DeleteUser(int userId);

        /// <summary>
        /// Get a list of company 
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <returns>
        /// Result message
        /// </returns>
        [OperationContract]
        [ApplyDataContractResolver]
        List<Companies> GetCompanies(int userId);
        /// <summary>
        /// Add a Company
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <param name="company">Company</param>
        /// <returns>
        /// Result message
        /// </returns>
        [OperationContract]
        bool CreateCompany(int userId, Companies company);
        /// <summary>
        /// Delete Company
        /// </summary>
        /// <param name="Company Id">Company ID</param>
        /// <returns>
        /// bool
        /// </returns>
        [OperationContract]
        bool DeleteCompany(int CompanyId, int UserID);
        /// <summary>
        /// Update Company
        /// </summary>
        /// <param name="company">company</param>
        /// <returns>
        /// Boolean
        /// </returns>
        [OperationContract]
        bool UpdateCompany(int userId, Companies company);
        /// <summary>
        /// Get PRoject list
        /// </summary>
        /// <param name="CompanyID">Company Id</param>
        /// <param name="userID">user id</param>
        /// <returns>List<Projects></returns>
        [OperationContract]
        List<Projects> GetProjects(int CompanyID, int userID);
        /// <summary>
        /// Get Projects
        /// </summary>
        /// <param name="ProjectId">Project ID</param>
        /// <returns>Projects</returns>
        [OperationContract]
        Projects GetProject(int ProjectId, int userID);
        /// <summary>
        /// Add Project 
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <param name="project">Project</param>
        /// <returns>ProjectResponse</returns>
        [OperationContract]
        ProjectResponse AddProject(int userId, Projects project);
        /// <summary>
        /// Update Project
        /// </summary>
        /// <param name="userID">User Id</param>
        /// <param name="project">Project</param>
        /// <returns>string</returns>
        [OperationContract]
        ProjectResponse UpdateProject(int userID, Projects project);
        /// <summary>
        /// Delete Project
        /// </summary>
        /// <param name="userID">User Id</param>
        /// <param name="projectID">Project Id</param>
        /// <returns></returns>
        [OperationContract]
        bool DeleteProject(int userID, int projectID);
        /// <summary>
        /// Get Project Statuses
        /// </summary>
        /// <returns>List<ProjectStatus></returns>
        [OperationContract]
        List<ProjectStatus> GetProjectStatuses();
        /// <summary>
        /// Get List of Material
        /// </summary>
        /// <param name="projectId">Project Id</param>
        /// <param name="userID">UserId</param>
        /// <returns> List<MaterialList></returns>
        [OperationContract]
        List<MaterialList> GetMaterials(int projectId, int userID);
        /// <summary>
        /// Get list of comments
        /// </summary>
        /// <param name="projectId">Project Id</param>
        /// <param name="userID">User Id</param>
        /// <returns>List<Comments></returns>
        [OperationContract]
        List<Comments> GetComments(int projectId, int userID);
        /// <summary>
        /// Get Project Overview
        /// </summary>
        /// <param name="CompanyId"></param>
        /// <returns>OverviewDetails</returns>
        [OperationContract]
        OverviewDetails GetOverView(int CompanyId, int UserId);
        /// <summary>
        /// Get Attachments by Id
        /// </summary>
        /// <param name="attachmentID">AttachmentID</param>
        /// <returns>Attachments</returns>
        [OperationContract]
        Attachments GetAttachmentById(int attachmentID);
        /// <summary>
        /// Get Attachments by projectid and userid
        /// </summary>
        /// <param name="ProjectId">ProjectID</param>
        /// <param name="userId">UserID</param>
        /// <returns>List<Attachments> </returns>
        [OperationContract]
        List<Attachments> GetAttachments(int ProjectId, int userId);
        /// <summary>
        /// To Add attachment one at a time
        /// </summary>
        /// <param name="attachments">Attachment Object</param>
        /// <returns>AttachmentResponse</returns>
        [OperationContract]
        AttachmentResponse AddAttachments(int userId, Attachments attachments);
        /// <summary>
        /// Get Username by user id
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <returns></returns>
        [OperationContract]
        string GetUserName(int userId);
    }
}
