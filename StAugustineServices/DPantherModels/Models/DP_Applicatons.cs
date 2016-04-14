using System;
using System.Collections.Generic;

namespace DPantherModels.Models
{
    public partial class DP_Applicatons
    {
        public DP_Applicatons()
        {
            this.DP_Identifier = new List<DP_Identifier>();
            this.DP_Logs = new List<DP_Logs>();
        }

        public int applicationID { get; set; }
        public string applicationName { get; set; }
        public string description { get; set; }
        public string url { get; set; }
        public string applicationCode { get; set; }
        public string centerPoint_X { get; set; }
        public string centerPoint_Y { get; set; }
        public string appLogo { get; set; }
        public string appTitle { get; set; }
        public string appType { get; set; }
        public string appFooter { get; set; }
        public string aggregationCode { get; set; }
        public string appImg { get; set; }
        public virtual ICollection<DP_Identifier> DP_Identifier { get; set; }
        public virtual ICollection<DP_Logs> DP_Logs { get; set; }
    }
}
