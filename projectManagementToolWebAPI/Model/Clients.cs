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
    public class Clients
    {
        [Key]
        public int ID { get; set; }

        [DataMember]
        public string ClientName { get; set; }

        [DataMember]
        public int UserId { get; set; }
        [DataMember]

        public virtual List<Projects> ProjectList { get; set; }
    }
}