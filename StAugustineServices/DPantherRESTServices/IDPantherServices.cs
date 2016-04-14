using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Collections.Specialized;
using System.Collections;

namespace DPantherRESTServices
{

    #region IDPantherServices Interface

    /// <summary>
    /// dPanther digital object services
    /// </summary>
    [ServiceContract]
    public interface IDPantherServices
    {
        //POST operation
        //[OperationContract]
        //[WebInvoke(UriTemplate = "", Method = "POST")]
        //string CreateDigitalObject(DigitalObject createPerson);

       
        /// <summary>
        /// get 20 digital object by aggregation and search criteria: 'CGM', 'condition', '1', '1', '1', '20'
        /// </summary>
        /// <param name="aggregation">"CGM","DRR"</param>
        /// <param name="condition">        
        /// combined multiple facet conditions and 1 search condition (up to 5 in total);
        /// format: ~term^attribute
        /// **e.g.  default:"1" return the whole dataset; 
        ///         [facet conditions] language:English;SubjectGeoAre:Coral Gables (Fla.) [advanced search condition] title: florida
        ///         ~English^Language~Coral Gables (Fla.)^SubjectGeoArea~florida^Title
        ///         
        /// </param>
        /// <param name="startYear">default:"1"</param>
        /// <param name="endYear">default:"1"</param>
        /// <param name="page">default:"1"; current page number</param>
        /// <param name="pageLimit">20</param>
        /// <returns>top 20 digital objects by aggregation,total number and year ranges</returns>
        [OperationContract]
        [WebGet(UriTemplate = "/AggSearchPage/{aggregation}/{condition}/{startYear}/{endYear}/{page}/{pageLimit}/{sort='title'}",
            ResponseFormat = WebMessageFormat.Json)]
        ReturnedDigitalObject GetDigitalObjectByAggregationPage(string aggregation, string condition, string startYear, string endYear, string page, string pageLimit, string sort);

       
        /// <summary>
        /// get the top 20 digital object by location: '25.7195457','-80.2786619','1','20'
        /// </summary>
        /// <param name="aggregation">"CGM","DRR"</param>
        /// <param name="strLat">latitude</param>
        /// <param name="strLng">longitude</param>
        /// <param name="page">default:"1"</param>
        /// <param name="pageLimit">20</param>
        /// <returns>a list of digital objects of selected location, total number and year ranges</returns>
        [OperationContract]
        [WebGet(UriTemplate = "/AggSearchByLocation/{aggregation}/{strLat}/{strLng}/{page}/{pageLimit}",
            ResponseFormat = WebMessageFormat.Json)]
        ReturnedDigitalObject GetDigitalObjectByLocationPage(string aggregation, string strLat, string strLng, string page, string pageLimit);

       
        //PUT Operation
        //[OperationContract]
        //[WebInvoke(UriTemplate = "{id}", Method = "PUT")]
        //string UpdateDigitalObject(string id);

        //DELETE Operation
        //[OperationContract]
        //[WebInvoke(UriTemplate = "{id}", Method = "DELETE")]
        //void DeleteDigitalObject(string id);

    }

    #endregion 


    #region Digital Object

    /// <summary>
    /// dPanther digital collection detailed information
    /// </summary>
    [DataContract]
    public class DigitalObject
    {   
        /// <summary>
        /// digital item title
        /// </summary>
        [DataMember]
        public string Title;

        /// <summary>
        /// list of digital item subjects
        /// </summary>
        [DataMember]
        public string[] Subjects;

        /// <summary>
        /// description of digital item
        /// </summary>
        [DataMember]
        public string Description;

        /// <summary>
        /// contributor of digital item
        /// </summary>
        [DataMember]
        public string Contributor;

        /// <summary>
        /// create date
        /// </summary>
        [DataMember]
        public string CreateDate;

        /// <summary>
        /// publish year
        /// </summary>
        [DataMember]
        public string PublishYear;

        /// <summary>
        /// digital item type
        /// </summary>
        [DataMember]
        public string[] Type;

        /// <summary>
        /// format
        /// </summary>
        [DataMember]
        public string Format;

        /// <summary>
        /// rights
        /// </summary>
        [DataMember]
        public string Rights;  
      
        /// <summary>
        /// publish place
        /// </summary>
        [DataMember]
        public string publishPlace;

        /// <summary>
        /// download link
        /// </summary>
        [DataMember]
        public ArrayList DownloadLink;

        /// <summary>
        /// identifier: FI number
        /// </summary>
        [DataMember]
        public string Identifier;

        /// <summary>
        /// authors
        /// </summary>
        [DataMember]
        public string[] Author;

        /// <summary>
        /// publisher
        /// </summary>
        [DataMember]
        public string Publisher;

        /// <summary>
        /// thumbnail 
        /// </summary>
        [DataMember]
        public string Thumbnail;

        /// <summary>
        /// geolocation lattitude
        /// </summary>
        [DataMember]
        public string mainLat;

        /// <summary>
        /// geolocation longitude
        /// </summary>
        [DataMember]
        public string mainLng;

        /// <summary>
        /// BibID: FI number
        /// </summary>
        [DataMember]
        public string BibID;

        /// <summary>
        /// VID:00001
        /// </summary>
        [DataMember]
        public string VID;

        /// <summary>
        /// static HTML page with metadata information
        /// </summary>
        [DataMember]
        public string htmlContent;

        /// <summary>
        /// HTML treeview of file list
        /// </summary>
        [DataMember]
        public string downloadTree;
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
        public string Title;

        /// <summary>
        /// digital item authors
        /// </summary>
        [DataMember]
        public string[] Author;

        /// <summary>
        /// publish year
        /// </summary>
        [DataMember]
        public string PublishYear;

        /// <summary>
        /// digital item type
        /// </summary>
        [DataMember]
        public string[] Type;

        /// <summary>
        /// thumbnail
        /// </summary>
        [DataMember]
        public string Thumbnail;

        /// <summary>
        /// geolocation lattitude
        /// </summary>
        [DataMember]
        public string mainLat;

        /// <summary>
        /// geolocation longitude
        /// </summary>
        [DataMember]
        public string mainLng;

        /// <summary>
        /// publish place
        /// </summary>
        [DataMember]
        public string publishPlace;

        /// <summary>
        /// list of download link
        /// </summary>
        [DataMember]
        public ArrayList DownloadLink;

        /// <summary>
        /// HTML treeview of file list
        /// </summary>
        [DataMember]
        public string downloadTree;

        /// <summary>
        /// Bib ID: FI number
        /// </summary>
        [DataMember]
        public string BibID;

        /// <summary>
        /// VID:00001
        /// </summary>
        [DataMember]
        public string VID;

        /// <summary>
        /// static HTML page with metadata information
        /// </summary>
        [DataMember]
        public string htmlContent;
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
        public string Attribute;

        /// <summary>
        /// number of items
        /// </summary>
        [DataMember]
        public string Count;
    }


    /// <summary>
    /// geo location object
    /// </summary>
    [DataContract]
    public class GeoLocationObject
    {
        /// <summary>
        /// lattitude
        /// </summary>
        [DataMember]
        public string Lat;//test

        /// <summary>
        /// longitude
        /// </summary>
        [DataMember]
        public string Log;

        /// <summary>
        /// address
        /// </summary>
        [DataMember]
        public string Address;
    }

    /// <summary>
    /// digital collection object returned from database
    /// </summary>
    [DataContract]
    public class ReturnedDigitalObject
    {
        /// <summary>
        /// total number of items
        /// </summary>
        [DataMember]
        public string Total;

        /// <summary>
        /// current page
        /// </summary>
        [DataMember]
        public string Pager;

        /// <summary>
        /// maximum year
        /// </summary>
        [DataMember]
        public string MaxYear;

        /// <summary>
        /// minimum year
        /// </summary>
        [DataMember]
        public string MinYear;

        /// <summary>
        /// facet object list returned from database
        /// </summary>
        [DataMember]
        public ReturnedFacetObject FacetObjectList;

        /// <summary>
        /// digital object list
        /// </summary>
        [DataMember]
        public List<DigitalObjectBrif> DigitalObjectList;

        /// <summary>
        /// application information returned from database
        /// </summary>
        [DataMember]
        public ReturnedAppObject appInfo;

        /// <summary>
        /// list of geolocation
        /// </summary>
        [DataMember]
        public List<GeoLocationObject> geoObjectList;

        /// <summary>
        /// list of FI numbers
        /// </summary>
        [DataMember]
        public List<string> FIlist;
    }

    /// <summary>
    /// list of facet result from database [subjectTopic, publisher, subjectgeoArea, author, language]
    /// </summary>
    public class ReturnedFacetObject
    {
        /// <summary>
        /// facet by subject topic
        /// </summary>
        [DataMember]
        public List<FacetObject> SubjectTopic;

        /// <summary>
        /// facet by publisher
        /// </summary>
        [DataMember]
        public List<FacetObject> Publisher;

        /// <summary>
        /// facet by location area
        /// </summary>
        [DataMember]
        public List<FacetObject> SubjectGeoArea;

        /// <summary>
        /// facet by author
        /// </summary>
        [DataMember]
        public List<FacetObject> Author;

        /// <summary>
        /// facet by language
        /// </summary>
        [DataMember]
        public List<FacetObject> Language;

        /// <summary>
        /// facet by type
        /// </summary>
        [DataMember]
        public List<FacetObject> Type;

        /// <summary>
        /// facet by aggregation
        /// </summary>
        [DataMember]
        public List<FacetObject> Aggregation;

    }

    /// <summary>
    /// application object
    /// </summary>
    [DataContract]
    public class ReturnedAppObject
    {
        /// <summary>
        /// application logo
        /// </summary>
        [DataMember]
        public string appLogo;

        /// <summary>
        /// application title
        /// </summary>
        [DataMember]
        public string appTitle;

        /// <summary>
        /// application type
        /// </summary>
        [DataMember]
        public string appType;

        /// <summary>
        /// application footer
        /// </summary>
        [DataMember]
        public string appFooter;
    }

       

    #endregion
}