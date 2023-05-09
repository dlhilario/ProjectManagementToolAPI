using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace projectManagementToolWebAPI.Model
{
    [DataContract]
    public class Companies
    {
        [Key]
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public Nullable<int> UserID { get; set; }
        [DataMember, StringLength(150)]
        public string Name { get; set; }
        [DataMember, StringLength(150)]
        public string EmailAdress { get; set; }
        [DataMember]
        public Nullable<int> StreetNumber { get; set; }
        [DataMember, StringLength(150)]
        public string Address1 { get; set; }
        [DataMember, StringLength(150)]
        public string Address2 { get; set; }
        [DataMember, StringLength(150)]
        public string City { get; set; }
        [DataMember, StringLength(50)]
        public string State { get; set; }
        [DataMember]
        public Nullable<int> ZipCode { get; set; }
        [DataMember, StringLength(12)]
        public string PhoneNumber { get; set; }
        [DataMember, StringLength(150)]
        public string BusinessUrl { get; set; }
        [DataMember, StringLength(12)]
        public string ContactPhoneNumber { get; set; }
        [DataMember]
        public DateTime CreatedDate { get; set; }
        [DataMember, StringLength(25)]
        public string Status { get; set; }
        [DataMember]
        public bool IsDeleted { get; set; }
        [DataMember]
        public DateTime ModifiedDate { get; set; }
        [DataMember]
        public Nullable<int> UpdatedByUserID { get; set; }

        [DataMember]
        public virtual ICollection<Projects> Projects { get; set; }
    }
}