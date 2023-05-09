using System.Runtime.Serialization;

namespace projectManagementToolWebAPI.Model
{
    [DataContract]
    public enum StatusEnum : int
    {
        [EnumMember]
        _DELETED_ = 0,
        [EnumMember]
        _CANCELED_ = 1,
        [EnumMember]
        _VOIDED_ = 2,
        [EnumMember]
        _DENDING_ = 3,
        [EnumMember]
        _HOLD_ = 4,
        [EnumMember]
        _COMPLETED_ = 5,
        [EnumMember]
        _OPEN_ = 6,
        [EnumMember]
        _ACTIVE_ = 7
    }
}