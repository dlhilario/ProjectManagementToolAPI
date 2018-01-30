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
        public int ID { get; set; }
        [DataMember]
        public string FileName { get; set; }
        [DataMember]
        public string Extention { get; set; }
        [DataMember]
        public string FileType { get; set; }
        [DataMember]
        public DateTime CreatedDate { get; set; }
    }
}