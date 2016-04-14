using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dpRestService
{
    /// <summary>
    /// dPanther digital collection class   
    /// </summary>
    public class dpCollection
    {
        /// <summary>
        /// FI number: | FI12040602
        /// </summary>
        public string BibID { get; set; }

        /// <summary>
        /// Title:  | Granada Entrance. Coral Gables, Florida
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Thumbnail: 3341990__FI12040602_A_jp2_thm.jpg
        /// </summary>
        public string MainThumbnail { get; set; }

        /// <summary>
        /// main JEPG: 3341990__FI12040602_A_jp2_.jpg
        /// </summary>
        public string MainJPEG { get; set; }

        /// <summary>
        /// Publisher of the collection
        /// </summary>
        public string Publisher { get; set; }

        /// <summary>
        /// Author: Fishbaugh, W. A. (William A.)
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// spatial reference: P|25.763611,-80.275555
        /// </summary>
        public string Spatial_KML { get; set; }

        /// <summary>
        /// aggregation code: [CGM,DRR...]
        /// </summary>
        public string AggregationCodes { get; set; }

        /// <summary>
        /// Institution: Florida International University
        /// </summary>
        public string Institution_Display { get; set; }

        /// <summary>
        /// Edition
        /// </summary>
        public string Edition_Display { get; set; }

        /// <summary>
        /// Subjects: Coral Gables (Fla.)--Buildings, structures, etc.--Pictorial works|Coral Gables (Fla.)--History--Pictorial works|Historic sites--Florida|Urban design
        /// </summary>
        public string Subjects_Display { get; set; }

        /// <summary>
        /// Disk Size (MB): 10439
        /// </summary>
        public string DiskSize_MB { get; set; }

        /// <summary>
        /// Last saved date: 2014-01-16 12:10:14.703
        /// </summary>
        public string LastSaved { get; set; }

        /// <summary>
        /// 0: display in the system; 1: not display in the system
        /// </summary>
        public string Deleted { get; set; }

        /// <summary>
        /// VID: 00001 (use to get the path of collection)
        /// </summary>
        public string VID { get; set; }

        /// <summary>
        /// Description or notes of the item
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Language: English/French/Spanish
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// List of files including file path: c:/file/FI01010100_1.PDF|c:/file/FI01010100_2.PDF
        /// </summary>
        public string FileList { get; set; }
        /// <summary>
        /// true: upload the record to dpanther server; false: don't upload the record to dpanther server
        /// </summary>
        public string uploadUser { get; set; }
        /// <summary>
        /// true: record save to dcc tracking table; false: don't save the record to dcc tracking table
        /// </summary>
        public bool tracking { get; set; }
       
    }
}