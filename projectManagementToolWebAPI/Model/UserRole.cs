using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace projectManagementToolWebAPI.Model
{
    [DataContract]
    public class UserRole
    {
        [Key, DataMember]
        public int ID { get; set; }
        [DataMember]
        public string Role { get; set; }
        [DataMember]
        public string Description { get; set; }
    }
}