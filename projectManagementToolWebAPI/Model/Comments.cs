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
    public class Comments
    {
        [Key]
        public int ID { get; set; }
        [DataMember]
        public string Comment { get; set; }
        [DataMember]
        public DateTime Time_Stamp { get; set; }

    }
   
}