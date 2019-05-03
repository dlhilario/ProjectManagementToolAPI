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
    public class Projects
    {
        [Key]
        public int ID { get; set; }
        [DataMember, StringLength(150)]
        public string ProjectName { get; set; }
        [DataMember, StringLength(150)]
        public string ProjectScope { get; set; }
        [DataMember, StringLength(10)]
        public string Lot { get; set; }
        [DataMember]
        public Nullable<int> CompanyID { get; set; }
        [DataMember]
        public Nullable<int> StreetNumber { get; set; }
        [DataMember, StringLength(150)]
        public string StreetAddress { get; set; }
        [DataMember, StringLength(50)]
        public string City { get; set; }
        [DataMember, StringLength(25)]
        public string State { get; set; }
        [DataMember]
        public Nullable<int> ZipCode { get; set; }
        [DataMember, StringLength(150)]
        public string Zone { get; set; } 
        [DataMember]
        public Nullable<DateTime> StartDate { get; set; }
        [DataMember]
        public Nullable<DateTime> EstimatedCompletionDate { get; set; }
        [DataMember]
        public Nullable<int> UpdatedByUserID { get; set; }
        [DataMember]
        public DateTime CreatedDate { get; set; }
        [DataMember]
        public DateTime ModifiedDate { get; set; }
        [DataMember, StringLength(25)]
        public string Status { get; set; }
        [DataMember]
        public bool IsDeleted { get; set; }
  
        [DataMember]
        public virtual List<Comments> CommentList { get; set; }
        [DataMember]
        public virtual List<Attachments> AttachmentList { get; set; }
        [DataMember]
        public virtual List<MaterialList> MaterialList { get; set; }
        
    }

}