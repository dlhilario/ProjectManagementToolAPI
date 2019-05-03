using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace projectManagementToolWebAPI.Model
{
    /// <summary>
    /// Store user profile information, this profile will be use for the user once the login to the site.
    /// </summary>
    [DataContract]
    public class UserProfile
    {
        [Key, DataMember]
        public int ID { get; set; }
        [DataMember, StringLength(50)]
        public string UserName { get; set; }
        [DataMember]
        public Nullable<int> UserRoleId { get; set; }
        [DataMember]
        private bool hasMessage = false;
        /// <summary>
        /// Check if the user profile has any message
        /// </summary>
        [DataMember]
        public bool HasMessage { get { return hasMessage; } set { hasMessage = value; } }
        /// <summary>
        /// Profile Message
        /// </summary>
        [DataMember, StringLength(150)]
        public string Message { get; set; }
        /// <summary>
        /// User Profile ID
        /// </summary>
        [DataMember]
        public int UserId { get; set; }
       
        /// <summary>
        /// First Name
        /// </summary>
        [DataMember, StringLength(150)]
        public string FirstName { get; set; }
        /// <summary>
        /// Last Name
        /// </summary>
        [DataMember, StringLength(150)]
        public string LastName { get; set; }

        public string StreetNumber { get; set; }
        
        /// <summary>
        /// Email Address
        /// </summary>
        [DataMember]
        public string Email { get; set; }
        /// <summary>
        /// Status
        /// </summary>
        [DataMember, StringLength(15)]
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

        /// <summary>
        /// User Role, the role will determine the access level for each user on any page.
        /// </summary>
     
        [DataMember]
        public virtual UserRole UserRole { get; set; }
    }
}