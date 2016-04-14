using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Collections;

namespace DPantherModels.dpModels
{
    [DataContract]
    public class dpItem
    {
        [DataMember(Name = "BibID")]
        public string BibID { get; set; }

        [DataMember(Name = "VID")]
        public string VID { get; set; }

        [DataMember(Name = "DownloadLink")]
        public ArrayList downloadlink { get; set; }

        [DataMember(Name = "downloadTree")]
        public string downloadtree { get; set; }

        [DataMember(Name = "htmlContent")]
        public string htmlContent { get; set; }

        [DataMember(Name = "publishPlace")]
        public string publishPlace { get; set; }

        [DataMember(Name = "mainLat")]
        public string mainLat { get; set; }

        [DataMember(Name = "mainLng")]
        public string mainLng { get; set; }

        [DataMember(Name = "mainThumb")]
        public string mainThumbnail { get; set; }

        [DataMember(Name = "kml")]
        public DPantherModels.dpModels.dpKML kml { get; set; }

        [DataMember(Name = "aggregationCode")]
        public ArrayList aggregationCode { get; set; }

        [DataMember(Name = "wordMark")]
        public List<WordMark> wordMark { get; set; }
    }
    /// <summary>
    /// wordMark object
    /// </summary>
    [DataContract]
    public class WordMark
    {
        [DataMember]
        public string IconID { get; set; }
      
        [DataMember]
        public string Icon_Name { get; set; }

        [DataMember]
        public string Icon_url { get; set; }

        [DataMember]
        public string link { get; set; }

        [DataMember]
        public string Height { get; set; }

        [DataMember]
        public string Title { get; set; }
    }
}
