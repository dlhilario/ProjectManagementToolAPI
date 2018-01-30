using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace projectManagementToolWebAPI.Model
{
    [DataContract]
    public class Users
    {
        [Key]
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
        [DataMember, EmailAddress, StringLength(50), Required]
        public string Email { get; set; }
        [DataMember, StringLength(20), ]
        public string Status { get; set; }
        [DataMember]
        public bool Active { get; set; }
        [DataMember]
        public bool loggedIn { get; set; }
        [DataMember]
        public DateTime LastLogin { get; set; }
        [DataMember]
        public DateTime CreatedDate { get; set; }
        [DataMember]
        public DateTime ModifiedDate { get; set; }

        [DataMember]
        public virtual List<Clients> ClientsList { get; set; }

    }
 
}