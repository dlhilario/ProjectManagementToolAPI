using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace projectManagementToolWebAPI.Model
{
    [DataContract]
    public class Attachments
    {
        [Key]
        [DataMember]
        public int ID { get; set; }

        [DataMember, StringLength(150)]
        public string FileName { get; set; }

        [DataMember]
        public int FileSize { get; set; }

        [DataMember, StringLength(50)]
        public string FileType { get; set; }

        [DataMember, Column(TypeName = "image")]
        public byte[] Document { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public Nullable<DateTime> CreatedDate { get; set; }
        [DataMember]
        public Nullable<DateTime> ModifiedDate { get; set; }
        [DataMember]
        public Nullable<int> ModifiedBy { get; set; }
        [DataMember, ForeignKey("Projects")]
        public Nullable<int> ProjectID { get; set; }

        [DataMember]
        public Projects Projects { get; set; }
    }
}