using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace projectManagementToolWebAPI.Model
{

    [DataContract]
    public class ProjectStatus
    {
        [Key]
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public string Value { get; set; }
        [DataMember]
        public string Text { get; set; }
        [DataMember]
        public bool Active { get; set; }
        [DataMember]
        public bool Selected { get; set; }
    }
}