using System;
using System.Collections.Generic;

namespace DPantherModels.Models
{
    public partial class DP_Identifier
    {
        public int sysID { get; set; }
        public string identifier { get; set; }
        public Nullable<System.DateTime> createDate { get; set; }
        public string createUser { get; set; }
        public string description { get; set; }
        public Nullable<int> appID { get; set; }
        public virtual DP_Applicatons DP_Applicatons { get; set; }
    }
}
