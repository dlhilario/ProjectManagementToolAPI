using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace projectManagementToolWebAPI.Model
{
    [DataContract]
    public class ErrorLog
    {

        [Key,DataMember]
        public int ID { get; set; }
        [DataMember, StringLength(50)]
        public string UserName { get; set; }
        [DataMember, StringLength(150)]
        public string ErrorMessage { get; set; }
        [DataMember]
        public string StackTrace { get; set; }
        [DataMember, StringLength(150)]
        public string Method { get; set; }
        [DataMember]
        public DateTime TimeStamp { get; set; }
    }
}