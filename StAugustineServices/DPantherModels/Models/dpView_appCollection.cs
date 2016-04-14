using System;
using System.Collections.Generic;

namespace DPantherModels.Models
{
    public partial class dpView_appCollection
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public bool Search_Parent_Only { get; set; }
        public int ChildID { get; set; }
        public int ParentID { get; set; }
        public string applicationName { get; set; }
        public string appDesc { get; set; }
        public string url { get; set; }
        public string centerPoint_X { get; set; }
        public string centerPoint_Y { get; set; }
        public string colDesc { get; set; }
        public Nullable<int> applicationID { get; set; }
        public string Type { get; set; }
        public string appLogo { get; set; }
        public string appImg { get; set; }
    }
}
