using projectManagementToolWebAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace projectManagementToolWebAPI
{
    [ServiceContract]
    public interface IPMTWebAPI
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
        UserProfile Login(string username, string password);
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
        /// Add a client
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <param name="clientName">Client Name</param>
        /// <returns>
        /// Result message
        /// </returns>
        [OperationContract]
        string AddClient(int userId, string clientName);
        /// <summary>
        /// Delete Client
        /// </summary>
        /// <param name="clientId">Client ID</param>
        /// <returns>
        /// Result Message
        /// </returns>
        [OperationContract]
        string DeleteClient(int clientId);
        /// <summary>
        /// Update Client
        /// </summary>
        /// <param name="clientId">Client ID</param>
        /// <param name="client">Client Information</param>
        /// <returns>
        /// Boolean
        /// </returns>
        [OperationContract]
        bool UpdateClient(int clientId, Clients client);
    }

}
