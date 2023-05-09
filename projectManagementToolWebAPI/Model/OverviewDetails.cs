using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace projectManagementToolWebAPI.Model
{
    public class OverviewDetails
    {
        public int TotalProjects { get; set; }
        public int TotalOpen { get; set; }
        public int TotalDeleted { get; set; }
        public int TotalCanceled { get; set; }
        public int TotalOnHold { get; set; }
        public int TotalCompleted { get; set; }
        public int TotalPending { get; set; }
    }
}