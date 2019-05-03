using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace projectManagementToolWebAPI.Model
{
    public static class Status
    {
        public const string DELETED = "_DELETED_";
        public const string CANCELED = "_CANCELED_";
        public const string VOIDED = "_VOIDED_";
        public const string PENDING = "_DENDING_";
        public const string ON_HOLD = "_HOLD_";
        public const string COMPLETED = "_COMPLETED_";
        public const string OPEN = "_OPEN_";
        public const string ACTIVE = "_ACTIVE_";
    }
}