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
    public class Attachments
    {
        [Key]
        [DataMember]
        public int ID { get; set; }
        [DataMember, StringLength(150)]
        public string FileName { get; set; }
        [DataMember, StringLength(25)]
        public string Extention { get; set; }
        [DataMember, StringLength(50)]
        public string FileType { get; set; }
        [DataMember]
        public string Document { get; set; }
        [DataMember]
        public DateTime CreatedDate { get; set; }
        [DataMember]
        public Nullable<int> ProjectID { get; set; }
    }
}