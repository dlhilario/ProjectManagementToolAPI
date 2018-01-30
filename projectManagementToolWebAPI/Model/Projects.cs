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
        [DataMember]
        public int Client_ID { get; set; }
        [DataMember]
        public string ProjectName { get; set; }
        [DataMember]
        public string ProjectScope { get; set; }
        [DataMember]
        public string Lot { get; set; }
        [DataMember]
        public string StreetNumber { get; set; }
        [DataMember]
        public string StreetAddress { get; set; }
        [DataMember]
        public DateTime StartDate { get; set; }
        [DataMember]
        public DateTime EstimatedCompletionDate { get; set; }

        [DataMember]
        public virtual List<Comments> CommentList { get; set; }
        [DataMember]
        public virtual List<Attachments> AttachmentList { get; set; }
    }

}