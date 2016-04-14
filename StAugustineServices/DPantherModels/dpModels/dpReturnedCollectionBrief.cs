using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Web;
using System.Collections.Specialized;
using System.Collections;

namespace DPantherModels.dpModels
{
   

    [DataContract]
    public class dpReturnedCollectionBrief
    {
        /// <summary>
        /// total number of items
        /// </summary>
        [DataMember]
        public string Total { get; set; }

        /// <summary>
        /// current page
        /// </summary>
        [DataMember]
        public string Pager { get; set; }

        /// <summary>
        /// maximum year
        /// </summary>
        [DataMember]
        public string MaxYear { get; set; }

        /// <summary>
        /// minimum year
        /// </summary>
        [DataMember]
        public string MinYear { get; set; }

        /// <summary>
        /// list of FI numbers
        /// </summary>
        [DataMember]
        public List<string> FIlist;
        /// <summary>
        /// facet object list returned from database
        /// </summary>
        [DataMember]
        public ReturnedFacetObject FacetObjectList { get; set; }

        /// <summary>
        /// digital object list
        /// </summary>
        [DataMember]
        public List<DigitalObjectBrif> DigitalObjectList { get; set; }

    }

    /// <summary>
    /// digital collection brief information
    /// </summary>
    [DataContract]
    public class DigitalObjectBrif
    {
        /// <summary>
        /// digital item title
        /// </summary>
        [DataMember]
        public string Title { get; set; }

        /// <summary>
        /// digital item authors
        /// </summary>
        [DataMember]
        public string[] Author { get; set; }

        /// <summary>
        /// publish year
        /// </summary>
        [DataMember]
        public string PublishYear { get; set; }

        /// <summary>
        /// digital item type
        /// </summary>
        [DataMember]
        public string[] Type { get; set; }

        /// <summary>
        /// thumbnail
        /// </summary>
        [DataMember]
        public string Thumbnail { get; set; }

        /// <summary>
        /// Bib ID: FI number
        /// </summary>
        [DataMember]
        public string BibID { get; set; }

        /// <summary>
        /// VID:00001
        /// </summary>
        [DataMember]
        public string VID { get; set; }

        [DataMember]
        public ArrayList DownloadLink { get; set; }

    }

    /// <summary>
    /// list of facet result from database [subjectTopic, publisher, subjectgeoArea, author, language]
    /// </summary>
    [DataContract]
    public class ReturnedFacetObject
    {
        /// <summary>
        /// facet by subject topic
        /// </summary>
        [DataMember]
        public List<FacetObject> SubjectTopic { get; set; }

        /// <summary>
        /// facet by publisher
        /// </summary>
        [DataMember]
        public List<FacetObject> Publisher { get; set; }

        /// <summary>
        /// facet by location area
        /// </summary>
        [DataMember]
        public List<FacetObject> SubjectGeoArea { get; set; }

        /// <summary>
        /// facet by author
        /// </summary>
        [DataMember]
        public List<FacetObject> Author { get; set; }

        /// <summary>
        /// facet by language
        /// </summary>
        [DataMember]
        public List<FacetObject> Language { get; set; }

        /// <summary>
        /// facet by type
        /// </summary>
        [DataMember]
        public List<FacetObject> Type { get; set; }

        /// <summary>
        /// facet by aggregation
        /// </summary>
        [DataMember]
        public List<FacetObject> Aggregation { get; set; }

    }


    /// <summary>
    /// facet result object: [facet attribute:count]
    /// </summary>
    [DataContract]
    public class FacetObject
    {
        /// <summary>
        /// facet attribute:[subjectTopic, publisher, subjectGeoArea, Author, Language]
        /// </summary>
        [DataMember]
        public string Attribute { get; set; }

        /// <summary>
        /// number of items
        /// </summary>
        [DataMember]
        public string Count { get; set; }
    }

   

   
}
