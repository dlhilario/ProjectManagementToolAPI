using System;
using System.Runtime.Serialization;

namespace projectManagementToolWebAPI.Model
{
    /// <summary>
    /// Store user profile information, this profile will be use for the user once the login to the site.
    /// </summary>
    [DataContract]
    public class UserProfile
    {
        private bool hasMessage = false;
        /// <summary>
        /// Check if the user profile has any message
        /// </summary>
        [DataMember]
        public bool HasMessage { get { return hasMessage; } set { hasMessage = value; } }
        /// <summary>
        /// Profile Message
        /// </summary>
        [DataMember]
        public string Message { get; set; }
        /// <summary>
        /// User Profile ID
        /// </summary>
        [DataMember]
        public int UserId { get; set; }
        /// <summary>
        /// User Name
        /// </summary>
        [DataMember]
        public string UserName { get; set; }
        /// <summary>
        /// First Name
        /// </summary>
        [DataMember]
        public string FirstName { get; set; }
        /// <summary>
        /// Last Name
        /// </summary>
        [DataMember]
        public string LastName { get; set; }
        /// <summary>
        /// User Role, the role will determine the access level for each user on any page.
        /// </summary>
        private UserRole _UserRole = new UserRole();

        [DataMember]
        public UserRole UserRole { get { return _UserRole; } set { _UserRole = value; } }
        /// <summary>
        /// Email Address
        /// </summary>
        [DataMember]
        public string Email { get; set; }
        /// <summary>
        /// Status
        /// </summary>
        [DataMember]
        public string Status { get; set; }

        private bool active = false;
        /// <summary>
        /// Active
        /// </summary>
        [DataMember]
        public bool Active { get { return active; } set { active = value; } }

        private bool loggedIn = false;
        /// <summary>
        /// Check if the user is logged in
        /// </summary>
        [DataMember]
        public bool LoggedIn { get { return loggedIn; } set { loggedIn = value; } }
        /// <summary>
        /// Last time the user logged in
        /// </summary>
        [DataMember]
        public DateTime LastLoggedIn { get; set; }
    }
}