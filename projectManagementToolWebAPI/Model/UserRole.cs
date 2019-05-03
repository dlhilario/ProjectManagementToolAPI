using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace projectManagementToolWebAPI.Model
{
    [DataContract]
    public class UserRole
    {
        [Key, DataMember]
        public int ID { get; set; }
        [DataMember, StringLength(50)]
        public string Role { get; set; }
        [DataMember, StringLength(150)]
        public string Description { get; set; }
    }
}