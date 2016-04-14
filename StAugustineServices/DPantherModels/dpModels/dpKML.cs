using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DPantherModels.dpModels
{
    /// <summary>
    /// dPanther KML class
    /// </summary>
    public class dpKML
    {
        /// <summary>
        /// KML ID in dPanther database
        /// </summary>
        public int kmlid { get; set; }

        /// <summary>
        /// KML file web url: http://dpanther.fiu.edu/dpContent/kml/CGM/FI05130001.kmz
        /// </summary>
        public string path { get; set; }

        /// <summary>
        /// KML file description :FI05130001
        /// </summary>
        public string description { get; set; }

        /// <summary>
        /// KML keyword :FI05130001
        /// </summary>
        public string keyword { get; set; }

        /// <summary>
        /// user created
        /// </summary>
        public string userCreated { get; set; }

        /// <summary>
        /// date created
        /// </summary>
        public string dateCreated { get; set; }

        /// <summary>
        /// FI number: FI05130001
        /// </summary>
        public string FINum { get; set; }
    }
}
