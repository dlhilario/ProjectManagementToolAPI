using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace projectManagementToolWebAPI.Model
{
    [DataContract]
    public class Users
    {
        [Key]
        [DataMember]
        public int ID { get; set; }
        [DataMember, StringLength(55)]
        public string FirstName { get; set; }
        [DataMember, StringLength(55)]
        public string LastName { get; set; }
        [DataMember, StringLength(55), Required]
        public string UserName { get; set; }
        [DataMember, StringLength(100)]
        public string Password { get; set; }
        [DataMember, StringLength(10)]
        public string SaltPassword { get; set; }
        [DataMember, Required]
        public int UserRoleId { get; set; }
        [DataMember]
        public Nullable<int> UserProfileID { get; set; }
        [DataMember]
        public Nullable<int> CompanyId { get; set; }
        [DataMember, StringLength(50), Required]
        public string Email { get; set; }
        [DataMember, StringLength(20),]
        public string Status { get; set; }
        [DataMember]
        public bool Active { get; set; }
        [DataMember]
        public bool LoggedIn { get; set; }
        [DataMember]
        public DateTime LastLogin { get; set; }
        [DataMember]
        public DateTime CreatedDate { get; set; }
        [DataMember]
        public DateTime ModifiedDate { get; set; }
        [DataMember]
        public UserRole UserRole { get; set; }
        [DataMember]
        public UserProfile UserProfile { get; set; }


    }

}