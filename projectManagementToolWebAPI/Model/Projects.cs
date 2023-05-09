using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace projectManagementToolWebAPI.Model
{
    [DataContract]
    public class Projects
    {
        [Key]
        [DataMember]
        public int ID { get; set; }
        [Required]
        [DataMember, StringLength(150)]
        public string ProjectName { get; set; }
        [DataMember, StringLength(150)]
        public string ProjectScope { get; set; }
        [DataMember, StringLength(10)]
        public string Lot { get; set; }
        [Required]
        [DataMember]
        public Nullable<int> StreetNumber { get; set; }
        [Required]
        [DataMember, StringLength(150)]
        public string StreetAddress { get; set; }
        [Required]
        [DataMember, StringLength(50)]
        public string City { get; set; }
        [Required]
        [DataMember, StringLength(25)]
        public string State { get; set; }
        [Required]
        [DataMember]
        public Nullable<int> ZipCode { get; set; }
        [DataMember, StringLength(150)]
        public string Zone { get; set; }
        [Required]
        [DataMember]
        public Nullable<DateTime> StartDate { get; set; }
        [Required]
        [DataMember]
        public Nullable<DateTime> EstimatedCompletionDate { get; set; }
        [DataMember]
        public Nullable<int> UpdatedByUserID { get; set; }
        [DataMember]
        public Nullable<DateTime> CreatedDate { get; set; }
        [DataMember]
        public Nullable<DateTime> ModifiedDate { get; set; }
        [DataMember, StringLength(25)]
        public string Status { get; set; }
        [DataMember]
        public bool IsDeleted { get; set; }
        [DataMember]
        public double CostEstimate { get; set; }
        
        [DataMember]
        public virtual ICollection<Comments> CommentList { get; set; }

        [DataMember]
        public virtual ICollection<Attachments> AttachmentList { get; set; }

        [DataMember]
        public virtual ICollection<MaterialList> MaterialList { get; set; }

        [Required]
        [DataMember,ForeignKey("Companies")]
        public Nullable<int> CompanyID { get; set; }
        [DataMember]
        public  Companies Companies { get; set; }



    }

}