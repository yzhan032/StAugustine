using System;
using System.Collections.Generic;

namespace DPantherModels.Models
{
    public partial class DP_Logs
    {
        public int idLog { get; set; }
        public string userInfo { get; set; }
        public Nullable<System.DateTime> opTime { get; set; }
        public string action { get; set; }
        public string description { get; set; }
        public Nullable<int> typeID { get; set; }
        public Nullable<int> applicationID { get; set; }
        public virtual DP_Applicatons DP_Applicatons { get; set; }
        public virtual DP_LogTypes DP_LogTypes { get; set; }
    }
}
