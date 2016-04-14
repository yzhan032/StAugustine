#region references
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using System.ServiceModel;
using System.ServiceModel.Activation;
using DPantherBusiness;
#endregion

namespace DPantherRESTServices
{
    /// <summary>
    /// dPanther digital object service
    /// </summary>
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class DigitalObjectService : IDPantherServices
    {
        /// <summary>
        /// the buffer size use to get digital object by coordinates
        /// **No longer use in the new system
        /// </summary>
        public double dbDistance05mil = 0.005;    
        
        
        
        /// <summary>
        /// get all ditigal objects by aggregation:"DRR","florida","1","1","1","0";
        /// ***No longer use in the new system
        /// </summary>
        /// <param name="strAgg">"CGM","WAWASH","DRR"</param>
        /// <param name="condition">default:"1"; the search criteria which user input</param>
        /// <param name="strFileType">default:"1";"book","Photograph","Archival","Unknown"</param>
        /// <param name="strStartYear">default:"1"</param>
        /// <param name="strEndYear">default:"1"</param>
        /// <param name="strTest">0</param>
        /// <returns>the list of ditital object</returns>
        public List<DigitalObject> GetDigitalObjectByAggregation(string strAgg, string condition, string strFileType, string strStartYear, string strEndYear, string strTest)
        {
            List<DigitalObject> li = new List<DigitalObject>();

            int iError = 0;

            string strApp = "92";
            if (strAgg == "DRR")
            {
                strApp = "2";
            }
            try
            {

                if (strTest == "0")
                {
                    Hashtable hs = new Hashtable();
                    if (strAgg == "1")
                    {
                        //test
                        hs.Add("aggregation", "");
                    }
                    else
                    {
                        hs.Add("aggregation", strAgg);
                    }
                    if (condition == "1")
                    {
                        hs.Add("citation", "");
                    }
                    else
                    {
                        hs.Add("citation", condition);
                    }
                    if (strFileType == "1")
                    {
                        hs.Add("type", "");
                    }
                    else
                    {
                        hs.Add("type", strFileType);
                    }
                    if (strStartYear == "1")
                    {
                        //hs.Add("startYear", "1800");
                        hs.Add("startYear", "");
                    }
                    else
                    {
                        hs.Add("startYear", strStartYear);
                    }
                    if (strStartYear == "1")
                    {
                        //hs.Add("endYear", Convert.ToString(DateTime.Now.Year));
                        hs.Add("endYear", "");
                    }
                    else
                    {
                        hs.Add("endYear", strEndYear);
                    }
                    string strSql = "DPanther_SearchAgg";
                    DataSet ds = DBO.exeDataset(strSql, "DPantherClean", hs);
                    DataTable dt = ds.Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            iError = i;
                            DataRow dr = dt.Rows[i];
                            addList(ref li, dr);
                        }
                    }
                }
                else
                {
                    
                }

                Log.addLog("", "Search", "Get digital object by aggregation success. Aggregation Code:'" + strAgg + "'", "2", strApp);
            }
            catch (Exception ex)
            {
                Log.addLog("", "Search", "Get digital object by aggregation faild. Aggregation Code:'" + strAgg + "', Because of " + ex.Message.ToString(), "2", strApp);
                throw ex;
            }
            return li;
        }

        /// <summary>
        /// get 20 digital object by aggregation and search criteria: 'CGM', 'condition', '1', '1', '1', '20'
        /// </summary>
        /// <param name="strAgg">"CGM","DRR"</param>
        /// <param name="condition">        
        /// combined multiple facet conditions and 1 search condition (up to 5 in total);
        /// format: ~term^attribute
        /// **e.g.  default:"1" return the whole dataset; 
        ///         [facet conditions] language:English;SubjectGeoAre:Coral Gables (Fla.) [advanced search condition] title: florida
        ///         ~English^Language~Coral Gables (Fla.)^SubjectGeoArea~florida^Title
        ///         
        /// </param>
        /// <param name="strStartYear">default:"1"</param>
        /// <param name="strEndYear">default:"1"</param>
        /// <param name="strPage">default:"1"; current page number</param>
        /// <param name="strPageLimit">20</param>
        /// <returns>top 20 digital objects by aggregation,total number and year ranges</returns>
        public ReturnedDigitalObject GetDigitalObjectByAggregationPage(string strAgg, string condition, string strStartYear, string strEndYear, string strPage, string strPageLimit, string sort="title")
        {
            ReturnedDigitalObject reDO = new ReturnedDigitalObject();
            List<DigitalObjectBrif> reDOList = new List<DigitalObjectBrif>();
            //List<DigitalObjectBrif> reDOListWhole = new List<DigitalObjectBrif>();
            List<GeoLocationObject> reGeoList = new List<GeoLocationObject>();
            ReturnedFacetObject reFacetObj = new ReturnedFacetObject();
            ReturnedAppObject reAppObj = new ReturnedAppObject();
            //string hostName = HttpContext.Current.Request.Url.Host;
            string hostName = dpDirectory.getServiceURLbyTypeAppcode("services", "dpAdmin", "dpCollectionService");
            try
            {
                Hashtable hs = new Hashtable();               
                    if (strAgg == "1")
                    {
                        //test
                        hs.Add("aggregationcode", "all");                       
                    }
                    else
                    {
                        hs.Add("aggregationcode", strAgg);
                        //hs.Add("term1", "1");
                    }
                   
                    if (strAgg != "1")
                    {
                        hs.Add("field1", -1);
                    }
                    hs.Add("link2", 0);
                    hs.Add("startYear", strStartYear);
                    hs.Add("endYear", strEndYear);
                    if (condition != "1")
                    {
                        string strsp = "DPanther_GetMetadataTypeID";
                        DataSet dsmetaType = DBO.exeDataset(strsp, "DPantherClean");
                        DataTable dtmetaType = dsmetaType.Tables[0];
                        //set first search condtion for all collection
                        hs.Add("term1", "1");
                        //add search conditions "term2#field2^term3#field3"

                        string[] strFacetSearch = condition.Split('~');

                        if (strFacetSearch.Length > 0)
                        {
                            for (int i = 0; i < strFacetSearch.Length; i++)
                            {
                                if (strFacetSearch[i] != "")
                                {
                                    string strterm = "term";
                                    string strfield = "field";
                                    string strlink = "link";
                                    //term1 has been used, start from term2 and field2
                                    strterm += (i + 2).ToString();
                                    strfield += (i + 2).ToString();
                                    strlink += (i + 2).ToString();
                                    string strFormatTerm = strFacetSearch[i].Split('^')[0].Replace(" ", " ");

                                    //valid search term;todo: need to move to General function in business;
                                    #region valid search term
                                    //if (strFormatTerm.IndexOf('.') != -1)
                                    //{ strFormatTerm = strFormatTerm.Remove(strFormatTerm.IndexOf('.')); }
                                    //strFormatTerm = strFormatTerm.Replace(".", "");
                                    //strFormatTerm = strFormatTerm.Replace(",", " ");

                                    //strFormatTerm = strFormatTerm.Replace(")", "");
                                    //strFormatTerm = strFormatTerm.Replace("(", "");
                                    //strFormatTerm = strFormatTerm.Replace("$", "/");
                                    //strFormatTerm = strFormatTerm.Replace("@", "&");
                                    //strFormatTerm = strFormatTerm.Replace("|", "?");
                                    //strFormatTerm = strFormatTerm.Replace("[", "");
                                    //strFormatTerm = strFormatTerm.Replace("]", "");
                                    //strFormatTerm = strFormatTerm.Replace("{", "");
                                    //strFormatTerm = strFormatTerm.Replace("}", "");
                                    strFormatTerm = '"' + strFormatTerm + '"';
                                    #endregion

                                    //if (strAgg == "1")
                                    //{ hs.Add("term1", strFormatTerm); }
                                    //else
                                    //{
                                        hs.Add(strterm, strFormatTerm);
                                    //}


                                    string strFieldCode = strFacetSearch[i].Split('^')[1];

                                    if (strFieldCode[0] == '+')
                                    {
                                        hs.Add(strlink, "0");
                                        strFieldCode = strFieldCode.Remove(0, 1);
                                    }
                                    else if (strFieldCode[0] == '-')
                                    {
                                        hs.Add(strlink, "2");
                                        strFieldCode = strFieldCode.Remove(0, 1);
                                    }
                                    else if (strFieldCode[0] == '=')
                                    {
                                        hs.Add(strlink, "1");
                                        strFieldCode = strFieldCode.Remove(0, 1);
                                    }


                                    if (strFieldCode == "Aggregation")
                                    {
                                        strFieldCode = "120";
                                    }
                                    else if (strFieldCode == "Language")
                                    {
                                        strFieldCode = "3";
                                    }
                                    else if (strFieldCode == "Publisher")
                                    {
                                        strFieldCode = "5";
                                    }
                                    else if (strFieldCode == "Type")
                                    {
                                        strFieldCode = "2";
                                    }
                                    else if (strFieldCode == "Author")
                                    {
                                        strFieldCode = "4";
                                    }
                                    else if (strFieldCode == "SubjectGeoArea")
                                    {
                                        strFieldCode = "10";
                                    }
                                    else if (strFieldCode == "SubjectTopic")
                                    {
                                        strFieldCode = "7";
                                    }

                                    else
                                    {
                                        strFieldCode = getMetadataTypeID(dtmetaType, strFieldCode);
                                    }
                                    hs.Add(strfield, strFieldCode);
                                   
                                }
                            }
                        }

                    }
                    else
                    {
                        hs.Add("term1", "1");
                    }
               
                hs.Add("include_private", true);
                hs.Add("pagesize", 25);
                hs.Add("pagenumber", 1);
                hs.Add("sort", 1);
                hs.Add("minpagelookahead", 1);
                hs.Add("maxpagelookahead", 5000);
                hs.Add("lookahead_factor", 20);
                hs.Add("include_facets", true);

                //metadateType: 3-language; 4-creator; 5-publisher 7: subject topic; 10 subject geographic area
                hs.Add("facettype1", 3);
                hs.Add("facettype2", 4);
                hs.Add("facettype3", 5);
                hs.Add("facettype4", 7);
                hs.Add("facettype5", 10);
                hs.Add("facettype6", 2);
                hs.Add("facettype7", 120);
                hs.Add("total_items", ""); // output

                string strSql = "DPanther_FacetSearch_Paged";
                if (sort == "date")
                {
                    strSql = "DPanther_FacetSearch_PagedByDate";
                }
                DataSet ds = DBO.exeDataset(strSql, "DPantherClean", hs);

                string strFI = "";

                //successfully excute the store procedure
                if (ds.Tables.Count > 5)//has data returned
                {
                    DataTable dt = ds.Tables[0];
                    reDO.Total = dt.Rows.Count.ToString();
                    reDO.Pager = strPage;
                    int startNum = (Convert.ToInt16(strPage) - 1) * Convert.ToInt16(strPageLimit);
                    int endNum = (Convert.ToInt16(strPage) - 1) * Convert.ToInt16(strPageLimit) + Convert.ToInt16(strPageLimit);
                    if (endNum >= Convert.ToInt16(reDO.Total))
                    {
                        endNum = Convert.ToInt16(reDO.Total);
                    }
                    if (dt.Rows.Count > 0)
                    {
                        //return one page data
                        for (int i = startNum; i < endNum; i++)
                        {
                            DataRow dr = dt.Rows[i];
                            strFI = addListwizFacet(ref reDOList, dr,hostName);
                            
                            ////return the location dataset
                            //DataRow drWhole = dt.Rows[i];
                            //addgeoList(ref reGeoList, drWhole);
                        }

                        ////todo: another loop to udpate searched statistics;
                        //for (int i = endNum; i < dt.Rows.Count; i++)
                        //{                           
                        //    DataRow drWhole = dt.Rows[i];
                        //    addgeoList(ref reGeoList, drWhole);
                        //}

                        //return the location dataset
                        List<string> fiNo = new List<string>();
                        for (int j = 0; j < dt.Rows.Count; j++)
                        {
                            DataRow drWhole = dt.Rows[j];
                            addgeoList(ref reGeoList, drWhole);
                            fiNo.Add(drWhole["BibID"].ToString().Trim().Trim('|').Trim());

                        }
                        reDO.FIlist = fiNo;
                    }
                    
                    //reDO.DigitalObjectWholeList = reDOListWhole;
                    reDO.geoObjectList = reGeoList;
                    reDO.DigitalObjectList = reDOList;
                    DataTable dtlanguage = new DataTable();
                    DataTable dtCeator = new DataTable();
                    DataTable dtpublisher = new DataTable();
                    DataTable dtSubjTopic = new DataTable();
                    DataTable dtSubGeoArea = new DataTable();
                    DataTable dtType = new DataTable();
                    DataTable dtAgg = new DataTable();
                    if (strAgg == "1") // search against all collections,contain one more table for collections
                    {
                        dtlanguage = ds.Tables[2];
                        dtCeator = ds.Tables[3];
                        dtpublisher = ds.Tables[4];
                        dtSubjTopic = ds.Tables[5];
                        dtSubGeoArea = ds.Tables[6];
                        dtType = ds.Tables[7];
                        dtAgg = ds.Tables[8];
                    }
                    else
                    { 
                        dtlanguage = ds.Tables[1];
                        dtCeator = ds.Tables[2];
                        dtpublisher = ds.Tables[3];
                        dtSubjTopic = ds.Tables[4];
                        dtSubGeoArea = ds.Tables[5];
                        dtType = ds.Tables[6];
                        dtAgg = ds.Tables[7];
                    }

                    reFacetObj.Language = facetResult(dtlanguage);
                    reFacetObj.Author = facetResult(dtCeator);
                    reFacetObj.Publisher = facetResult(dtpublisher);
                    reFacetObj.SubjectTopic = facetResult(dtSubjTopic);
                    reFacetObj.SubjectGeoArea = facetResult(dtSubGeoArea);
                    reFacetObj.Type = facetResult(dtType);
                    reFacetObj.Aggregation = facetResult(dtAgg);


                    reDO.FacetObjectList = reFacetObj;

                    if (strAgg == "CGM" || strAgg == "cgm" || strAgg == "usach" || strAgg == "USACH")
                    {
                        Hashtable hsaggPubYear = new Hashtable();
                        hsaggPubYear.Add("aggcode", "%" + strAgg + "%");
                        string strSqlgetYear = "DPanther_GetpubYear";
                        DataSet dsYear = DBO.exeDataset(strSqlgetYear, "DPantherClean", hsaggPubYear);
                        reDO.MaxYear = dsYear.Tables[0].Rows[0]["maxYear"].ToString();
                        reDO.MinYear = dsYear.Tables[0].Rows[0]["minYear"].ToString();
                    }
                    else
                    {
                        reDO.MaxYear = DateTime.Now.Year.ToString();
                        reDO.MinYear = "0";
                    }

                    //get application Header info

                    //Hashtable hsagg = new Hashtable();
                    //hsagg.Add("aggcode", strAgg);
                    //string strSqlgetApp = "DPanther_GetAppHeader";
                    //DataSet dsApp = DBO.exeDataset(strSqlgetApp, "DPantherClean", hsagg);
                    //if (dsApp.Tables[0].Rows.Count > 0)
                    //{
                    //    reAppObj.appLogo = dsApp.Tables[0].Rows[0]["appLogo"].ToString();
                    //    reAppObj.appTitle = dsApp.Tables[0].Rows[0]["appTitle"].ToString();
                    //    reAppObj.appFooter = dsApp.Tables[0].Rows[0]["appFooter"].ToString();
                    //    reAppObj.appType = dsApp.Tables[0].Rows[0]["appType"].ToString();
                    //}
                    //else
                    //{
                    //    reAppObj.appLogo = "NA";
                    //    reAppObj.appTitle = "NA";
                    //    reAppObj.appFooter = "NA";
                    //    reAppObj.appType = "NA";
                    //}
                    //reDO.appInfo = reAppObj;
                }
                else//no data returned
                {
                    
                }

               
            }

            catch (Exception ex)
            {
                Log.addLog("", "Search", "Get pages digital object by aggregation failed. Aggregation Code:'"+strAgg+"', because of " + ex.Message.ToString(), "2", "92");
                throw ex;
            }
            Log.addLog("", "Search", "Get paged digital object by aggregation success. Aggregation Code:'"+strAgg+"'", "2", "92");
            
            return reDO;
        }
        
               
        /// <summary>
        /// get the top 20 digital object by location: '25.7195457','-80.2786619','1','20'
        /// </summary>
        /// <param name="strAgg">"CGM","DRR"</param>
        /// <param name="strLat">latitude</param>
        /// <param name="strLong">longitude</param>
        /// <param name="strPage">default:"1"</param>
        /// <param name="strPageLimit">20</param>
        /// <returns>a list of digital objects of selected location, total number and year ranges</returns>
        public ReturnedDigitalObject  GetDigitalObjectByLocationPage(string strAgg, string strLat, string strLong, string strPage, string strPageLimit)
        {
            ReturnedDigitalObject reDO = new ReturnedDigitalObject();
            List<DigitalObjectBrif> reDOList = new List<DigitalObjectBrif>();
            //List<DigitalObjectBrif> reDOListWhole = new List<DigitalObjectBrif>();
            List<GeoLocationObject> reGeoList = new List<GeoLocationObject>();
            ReturnedFacetObject reFacetObj = new ReturnedFacetObject();
            ReturnedAppObject reAppObj = new ReturnedAppObject();
            string hostName = dpDirectory.getDIRWebbyTypeAppcode("dpCollectionService", "dpAdmin");
            try
            {
                Hashtable hs = new Hashtable();
                if (strAgg == "1")
                {
                    //test
                    hs.Add("aggregationcode", "all");
                }
                else
                {
                    hs.Add("aggregationcode", strAgg);
                }

                hs.Add("lat1", strLat);
                hs.Add("long1", strLong);             

               

                hs.Add("include_private", true);
                hs.Add("pagesize", 25);
                hs.Add("pagenumber", 1);
                hs.Add("sort", 1);
                hs.Add("minpagelookahead", 1);
                hs.Add("maxpagelookahead", 100);
                hs.Add("lookahead_factor", 20);
                hs.Add("include_facets", true);

                ///metadateType: 3-language; 4-creator; 5-publisher 7: subject topic; 10 subject geographic area
                hs.Add("facettype1", 3);
                hs.Add("facettype2", 4);
                hs.Add("facettype3", 5);
                hs.Add("facettype4", 7);
                hs.Add("facettype5", 10);
                hs.Add("total_items", ""); // output
                hs.Add("total_titles", ""); // output
                string strSql = "DPanther_GetCollectionByCoordinates";
                DataSet ds = DBO.exeDataset(strSql, "DPantherClean", hs);
                if (ds.Tables.Count > 5)
                {
                    DataTable dt = ds.Tables[1];
                    reDO.Total = dt.Rows.Count.ToString();
                    reDO.Pager = strPage;
                    int startNum = (Convert.ToInt16(strPage) - 1) * Convert.ToInt16(strPageLimit);
                    int endNum = (Convert.ToInt16(strPage) - 1) * Convert.ToInt16(strPageLimit) + Convert.ToInt16(strPageLimit);
                    if (endNum >= Convert.ToInt16(reDO.Total))
                    {
                        endNum = Convert.ToInt16(reDO.Total);
                    }
                    if (dt.Rows.Count > 0)
                    {
                        //return one page data
                        for (int i = startNum; i < endNum; i++)
                        {
                            DataRow dr = dt.Rows[i];
                            addListwizFacet(ref reDOList, dr,hostName);
                        }
                        for (int j = 0; j < dt.Rows.Count; j++)
                        {
                            DataRow drWhole = dt.Rows[j];

                            addgeoList(ref reGeoList, drWhole);

                        }
                       
                    }
                    reDO.geoObjectList = reGeoList;
                    reDO.DigitalObjectList = reDOList;
                    DataTable dtlanguage = ds.Tables[2];
                    DataTable dtCeator = ds.Tables[3];
                    DataTable dtpublisher = ds.Tables[4];
                    DataTable dtSubjTopic = ds.Tables[5];
                    DataTable dtSubGeoArea = ds.Tables[6];

                    reFacetObj.Language = facetResult(dtlanguage);
                    reFacetObj.Author = facetResult(dtCeator);
                    reFacetObj.Publisher = facetResult(dtpublisher);
                    reFacetObj.SubjectTopic = facetResult(dtSubjTopic);
                    reFacetObj.SubjectGeoArea = facetResult(dtSubGeoArea);

                    reDO.FacetObjectList = reFacetObj;
                    Hashtable hsagg = new Hashtable();
                    hsagg.Add("aggcode", strAgg);
                    string strSqlgetYear = "DPanther_GetpubYear";
                    DataSet dsYear = DBO.exeDataset(strSqlgetYear, "DPantherClean", hsagg);
                    reDO.MaxYear = dsYear.Tables[0].Rows[0]["maxYear"].ToString();
                    reDO.MinYear = dsYear.Tables[0].Rows[0]["minYear"].ToString();

                    //get application Header info


                    //string strSqlgetApp = "DPanther_GetAppHeader";
                    //DataSet dsApp = DBO.exeDataset(strSqlgetApp, "DPantherClean", hsagg);
                    //reAppObj.appLogo = dsApp.Tables[0].Rows[0]["appLogo"].ToString();
                    //reAppObj.appTitle = dsApp.Tables[0].Rows[0]["appTitle"].ToString();
                    //reDO.appInfo = reAppObj;
                }
                else
                {
                    //no result
                }

            }

            catch (Exception ex)
            {
                Log.addLog("", "Search", "Get pages digital object by locaiton failed. Aggregation Code:'" + strAgg + "', because of " + ex.Message.ToString(), "2", "92");
                throw ex;
            }
            Log.addLog("", "Search", "Get paged digital object by locaiton success. Aggregation Code:'" + strAgg + "'", "2", "92");

            return reDO;
        }

               

        /// <summary>
        /// bulid the list of digital collections with detailed information
        /// **no longer use in the new system
        /// </summary>
        /// <param name="li">returned list of digital collections</param>
        /// <param name="dr">data record from DB</param>
        private void addList(ref List<DigitalObject> li, DataRow dr)
        {
            try
            {
                DigitalObject digi = new DigitalObject();
                digi.Author = dr["author"].ToString().Split('|');
                digi.Contributor = dr["contributor"].ToString().Trim();
                digi.CreateDate = dr["createDate"].ToString().Trim();
                digi.Description = dr["description"].ToString().Trim().Trim('|').Trim();   
                digi.Format = dr["format"].ToString().Trim();
                digi.Identifier = dr["identifier"].ToString().Trim();
                digi.publishPlace = dr["publishPlace"].ToString().Trim();
                if (dr["publishYear"].ToString().Trim() == "1800" || dr["publishYear"].ToString() == "")
                {
                    string strPublishYear = dr["PubDate"].ToString().Trim('[').Trim(']').Trim('?');
                    string[] strarr = strPublishYear.Split('-');
                    digi.PublishYear = strarr[0];
                }
                else
                {
                    digi.PublishYear = dr["publishYear"].ToString().Trim();
                }
                digi.Rights = dr["copyRight"].ToString().Trim().Trim('|').Trim();   
                digi.Subjects = dr["subjects"].ToString().Trim().Trim('|').Split('|');
                //processing the document:remove unnecessary "." and white trailing white spaces 
                int j = 0;
                while (j < digi.Subjects.Length)
                {
                    int remove = 0;
                    digi.Subjects[j] = digi.Subjects[j].Replace(".", "").Trim();
                    //check if contains"--"
                    if (digi.Subjects[j].IndexOf("--") != -1)
                    {
                        digi.Subjects[j] = digi.Subjects[j].Substring(0,digi.Subjects[j].IndexOf("--")).Replace(".", "").Trim();
                        //check if duplicated
                        for (int k = 0; k < j; k++)
                        {
                            if (j < digi.Subjects.Length)
                            {
                                if (digi.Subjects[j] == digi.Subjects[k])
                                {
                                    List<string> temparr = new List<string>(digi.Subjects);
                                    temparr.RemoveAt(j);
                                    digi.Subjects = temparr.ToArray();
                                    remove = 1;

                                }
                            }

                        }
 
                    }
                    if (remove == 0)
                        j++;
                }
               
              

                //
                digi.Thumbnail = dr["thumbnail"].ToString().Trim();
                digi.Title = dr["title"].ToString().Trim();
                digi.Type = dr["type"].ToString().Trim().Trim('|').Split('|');
                digi.mainLat = dr["MainLatitude"].ToString().Trim();
                digi.mainLng = dr["MainLongitude"].ToString().Trim();
                if (digi.mainLat == "" && digi.mainLng == "")
                {
                    string strKML = dr["Spatial_KML"].ToString().Trim();
                    if (strKML != "")
                    {
                        string[] Strarr = strKML.Split('|');
                        string strCoordiantes = Strarr[1];
                        string[] strarr_coo = strCoordiantes.Split(',');
                        digi.mainLat = strarr_coo[0];
                        digi.mainLng = strarr_coo[1];
                    }
                    else
                    {
                        digi.mainLat = "N/A";
                        digi.mainLng = "N/A";
                    }
                }
                digi.BibID = dr["BibID"].ToString().Trim().Trim('|').Trim();
                digi.VID = dr["VID"].ToString().Trim();

                //get the download link from the mets file;
                string strPath = "\\" + digi.BibID.Substring(0, 2)
                                + "\\" + digi.BibID.Substring(2, 2)
                                + "\\" + digi.BibID.Substring(4, 2)
                                + "\\" + digi.BibID.Substring(6, 2)
                                + "\\" + digi.BibID.Substring(8, 2)
                                + "\\" + digi.VID + "\\" + "sobek_files\\" + digi.BibID + "_" + digi.VID;
                
                ArrayList downPath = GeneralFunction.getPath(strPath + ".mets.xml");

                //string downPathNew = GeneralFunction.getPathNew(strPath + ".mets.xml");
                //digi.downloadTree = downPathNew;
                digi.DownloadLink = downPath;

                digi.htmlContent = GeneralFunction.getSobekContentHTML(strPath + ".html");

                digi.Publisher = dr["publisher"].ToString().Trim();
                li.Add(digi);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        /// <summary>
        /// bulid the list of digital collections with detailed information
        /// </summary>
        /// <param name="li">returned list of digital collections</param>
        /// <param name="dr">data record from DB</param>
        /// <returns>bibID of each record</returns>
        private string addListwizFacet(ref List<DigitalObjectBrif> li, DataRow dr, string hostName)
        {
            string strFI = "";
            try
            {
                DigitalObjectBrif digiBrif = new DigitalObjectBrif();
                digiBrif.Author = dr["author"].ToString().Split('|');

                if (dr["publishYear"].ToString().Trim() == "1800" || dr["publishYear"].ToString() == "")
                {
                    string strPublishYear = dr["PubDate"].ToString().Trim('?');
                    string[] strarr = strPublishYear.Split('-');
                    digiBrif.PublishYear = strarr[0];
                }
                else
                {
                    digiBrif.PublishYear = dr["publishYear"].ToString().Trim();
                }
                digiBrif.Title = dr["title"].ToString().Trim();
                digiBrif.Type = dr["type"].ToString().Trim().Trim('|').Split('|');
                digiBrif.Thumbnail = dr["thumbnail"].ToString().Trim();
                digiBrif.mainLat = dr["MainLatitude"].ToString().Trim();
                digiBrif.mainLng = dr["MainLongitude"].ToString().Trim();

                if (digiBrif.mainLat == "" && digiBrif.mainLng == "")
                {
                    string strKML = dr["Spatial_KML"].ToString().Trim();
                    if (strKML != "")
                    {
                        string[] Strarr = strKML.Split('|');
                        string strCoordiantes = Strarr[1];
                        string[] strarr_coo = strCoordiantes.Split(',');
                        digiBrif.mainLat = strarr_coo[0];
                        digiBrif.mainLng = strarr_coo[1];
                    }
                    else
                    {
                        digiBrif.mainLat = "N/A";
                        digiBrif.mainLng = "N/A";
                    }
                }
                digiBrif.BibID = dr["BibID"].ToString().Trim().Trim('|').Trim();
                digiBrif.VID = dr["VID"].ToString().Trim();
                digiBrif.publishPlace = dr["publishPlace"].ToString().Trim();

                strFI = digiBrif.BibID;

                //get the download link from the mets file;
                string strPath = "\\" + digiBrif.BibID.Substring(0, 2)
                                + "\\" + digiBrif.BibID.Substring(2, 2)
                                + "\\" + digiBrif.BibID.Substring(4, 2)
                                + "\\" + digiBrif.BibID.Substring(6, 2)
                                + "\\" + digiBrif.BibID.Substring(8, 2)
                                + "\\" + digiBrif.VID + "\\";

                ArrayList downPath = GeneralFunction.getPath(strPath + digiBrif.BibID + "_" + digiBrif.VID + ".mets.xml");

                string downPathNew = GeneralFunction.getPathNew(strPath + digiBrif.BibID + "_" + digiBrif.VID + ".mets.xml");
                digiBrif.downloadTree = downPathNew;

                string dirSobekContentPath = dpDirectory.getDIRbyTypeAppcode("sobekCitationUrl", "dpAdmin");
                digiBrif.DownloadLink = downPath;
                digiBrif.htmlContent = GeneralFunction.getSobekContentHTMLfromURL(dirSobekContentPath + digiBrif.BibID + "/" + digiBrif.VID + "/citation", hostName);// GeneralFunction.getSobekContentHTML(strPath + "sobek_files\\" + digiBrif.BibID + "_" + digiBrif.VID + ".html");
                //digiBrif.htmlContent ="";

                li.Add(digiBrif);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return strFI;
        }
        
        /// <summary>
        /// add location to a distinct location list
        /// </summary>
        /// <param name="li">returned distinct location list</param>
        /// <param name="dr">data record from DB</param>
        private void addgeoList(ref List<GeoLocationObject> li, DataRow dr)
        {
            try
            {
                GeoLocationObject digiBrif = new GeoLocationObject();
                bool duplicate = false;
                digiBrif.Lat = dr["MainLatitude"].ToString().Trim();
                digiBrif.Log = dr["MainLongitude"].ToString().Trim();
                digiBrif.Address = dr["Title"].ToString().Trim();

                if (digiBrif.Lat == "" && digiBrif.Log == "")
                {
                    string strKML = dr["Spatial_KML"].ToString().Trim();
                    if (strKML != "")
                    {
                        string[] Strarr = strKML.Split('|');
                        string strCoordiantes = Strarr[1];
                        string[] strarr_coo = strCoordiantes.Split(',');
                        digiBrif.Lat = strarr_coo[0];
                        digiBrif.Log = strarr_coo[1];
                    }
                   
                }
                for (int i = 0; i < li.Count; i++)
                {
                    if (li[i].Lat == digiBrif.Lat && li[i].Log == digiBrif.Log)
                    {
                        duplicate = true;
                        break;
                    }
                }
                if (!duplicate)
                {
                    li.Add(digiBrif);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        /// <summary>
        /// get the list of facet result
        /// </summary>
        /// <param name="dt">facet datatable from DB by facet type</param>
        /// <returns>a list of the facet result including value and count</returns>
        private List<FacetObject> facetResult(DataTable dt)
        {
            List<FacetObject> liFO = new List<FacetObject>();
            try
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];
                    FacetObject fo = new FacetObject();
                    fo.Attribute=dr["MetadataValue"].ToString();
                    fo.Count=dr["Metadata_Count"].ToString();
                    if (fo.Attribute != "iFIU" && fo.Attribute!="ALL")
                    { liFO.Add(fo); }
                }                   
            
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return liFO;
        }
        
        private static string getMetadataTypeID(DataTable dt, string sobekCode) {
            string metadataTypeID = "-1";
            for (int i = 0; i <dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                if (sobekCode == dr["SobekCode"].ToString())
                {
                    metadataTypeID = dr["MetadataTypeID"].ToString();
                    break;
                }

            }
            return metadataTypeID;
        }
           
        
    }    
}
