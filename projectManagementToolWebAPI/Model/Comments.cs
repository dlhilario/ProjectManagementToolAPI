using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        [Required]
        [DataMember]
        public string Comment { get; set; }
        [DataMember]
        public Nullable<DateTime> Time_Stamp { get; set; }
        [DataMember]
        public Nullable<int> UpdatedByUserID { get; set; }

        [DataMember,ForeignKey("Projects")]
        public Nullable<int> ProjectID { get; set; }
        [DataMember]
        public Projects Projects { get; set; }

    }
   
}