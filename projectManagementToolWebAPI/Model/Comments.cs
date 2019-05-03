using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Runtime.Serialization;

namespace projectManagementToolWebAPI.Model
{
    [DataContract]
    public class Comments
    {
        [Key]
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public string Comment { get; set; }
        [DataMember]
        public DateTime Time_Stamp { get; set; }
        [DataMember]
        public Nullable<int> UpdatedByUserID { get; set; }
        [DataMember]
        public Nullable<int> ProjectID { get; set; }

    }
   
}