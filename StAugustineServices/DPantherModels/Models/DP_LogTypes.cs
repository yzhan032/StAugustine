using System;
using System.Collections.Generic;

namespace DPantherModels.Models
{
    public partial class DP_LogTypes
    {
        public DP_LogTypes()
        {
            this.DP_Logs = new List<DP_Logs>();
        }

        public int typeID { get; set; }
        public string type { get; set; }
        public string description { get; set; }
        public virtual ICollection<DP_Logs> DP_Logs { get; set; }
    }
}
