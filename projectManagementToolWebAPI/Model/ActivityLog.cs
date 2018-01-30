using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace projectManagementToolWebAPI.Model
{[DataContract]
    public class ActivityLog
    {
        [Key, DataMember]
        public int ID { get; set; }
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public string Message { get; set; }
        [DataMember]
        public string Method { get; set; }
        [DataMember]
        public  DateTime TimeStamp { get; set; }
    }
}