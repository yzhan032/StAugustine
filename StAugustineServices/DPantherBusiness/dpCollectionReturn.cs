using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using DPantherModels;
using DPantherModels.dpModels;
using System.Web;
using System.Configuration;
using System.IO;

namespace DPantherBusiness
{
    public static class dpCollectionReturn
    {
        public static dpReturnedCollectionBrief getDigitalObjectByAggregationPage(string strAgg, string condition, string strStartYear, string strEndYear, string strPage, string strPageLimit, string sort, string needFacet, string order="0")
        {
            dpReturnedCollectionBrief reDO = new dpReturnedCollectionBrief();
            List<DigitalObjectBrif> reDOList = new List<DigitalObjectBrif>();
            //List<DigitalObjectBrif> reDOListWhole = new List<DigitalObjectBrif>();
          
            ReturnedFacetObject reFacetObj = new ReturnedFacetObject();
        
            try
            {
                Hashtable hs = new Hashtable();
                if (strAgg == "1")
                {                  
                    hs.Add("aggregationcode", "all");
                }
                else
                {
                    hs.Add("aggregationcode", strAgg);                   
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

                                //valid search term; todo: need to move to General function in business;
                                #region valid search term
                                if (strFormatTerm.IndexOf('.') != -1)
                                { strFormatTerm = strFormatTerm.Remove(strFormatTerm.IndexOf('.')); }
                                strFormatTerm = strFormatTerm.Replace(".", "");
                                strFormatTerm = strFormatTerm.Replace(",", " ");

                                strFormatTerm = strFormatTerm.Replace(")", "");
                                strFormatTerm = strFormatTerm.Replace("(", "");
                                strFormatTerm = strFormatTerm.Replace("$", "/");
                                strFormatTerm = strFormatTerm.Replace("@", "&");
                                strFormatTerm = strFormatTerm.Replace("|", "?");
                                strFormatTerm = strFormatTerm.Replace("[", "");
                                strFormatTerm = strFormatTerm.Replace("]", "");
                                strFormatTerm = strFormatTerm.Replace("{", "");
                                strFormatTerm = strFormatTerm.Replace("}", "");
                                strFormatTerm = '"' + strFormatTerm + '"';
                                #endregion

                                hs.Add(strterm, strFormatTerm);
                                string strFieldCode = strFacetSearch[i].Split('^')[1];
                                //get the code for the facet field; todo:need to move to general function
                                #region get facet code
                                switch (strFieldCode)
                                {
                                    case "Language":
                                        strFieldCode = "3";
                                        break;
                                    case "Format":
                                        strFieldCode = "22";
                                        break;
                                    case "Identifier":
                                        strFieldCode = "23";
                                        break;
                                    case "Holding_Location":
                                        strFieldCode = "16";
                                        break;
                                    case "Genre":
                                        strFieldCode = "8";
                                        break;
                                    case "Publisher":
                                        strFieldCode = "5";
                                        break;
                                    case "Subject_Keyword":
                                        strFieldCode = "7";
                                        break;
                                    case "SubjectTopic":
                                        strFieldCode = "7";
                                        break;
                                    case "SubjectGeoArea":
                                        strFieldCode = "10";
                                        break;
                                    case "Author":
                                        strFieldCode = "4";
                                        break;
                                    case "Title":
                                        strFieldCode = "1";
                                        break;
                                    case "Type":
                                        strFieldCode = "2";
                                        break;
                                    case "Author $ Creator":
                                        strFieldCode = "4";
                                        break;
                                    case "Aggregation":
                                        strFieldCode = "120";
                                        break;
                                    case "Full Citation":
                                        strFieldCode = "-1";
                                        break;
                                    // ADD FOR FULL CITATION PAGE
                                    case "TI":
                                        strFieldCode = "1";
                                        break;
                                    case "BI":
                                        strFieldCode = "23";
                                        break;
                                    case "AU":
                                        strFieldCode = "4";
                                        break;
                                    case "SU":
                                        strFieldCode = "29";
                                        break;
                                    case "CO":
                                        strFieldCode = "11";
                                        break;
                                    case "ST":
                                        strFieldCode = "12";
                                        break;
                                    case "CT":
                                        strFieldCode = "13";
                                        break;
                                    case "CI":
                                        strFieldCode = "14";
                                        break;
                                    case "PP":
                                        strFieldCode = "6";
                                        break;
                                    case "SP":
                                        strFieldCode = "10";
                                        break;
                                    case "TY":
                                        strFieldCode = "2";
                                        break;
                                    case "LA":
                                        strFieldCode = "3";
                                        break;
                                    case "PU":
                                        strFieldCode = "5";
                                        break;
                                    case "GE":
                                        strFieldCode = "8";
                                        break;
                                    case "TA":
                                        strFieldCode = "9";
                                        break;
                                    case "DO":
                                        strFieldCode = "21";
                                        break;
                                    case "AT":
                                        strFieldCode = "31";
                                        break;
                                    case "TL":
                                        strFieldCode = "20";
                                        break;
                                    case "NO":
                                        strFieldCode = "18";
                                        break;
                                    case "ID":
                                        strFieldCode = "17";
                                        break;
                                    case "FR":
                                        strFieldCode = "26";
                                        break;
                                    case "TB":
                                        strFieldCode = "36";
                                        break;

                                }
                                #endregion

                                hs.Add(strfield, strFieldCode);
                              
                            }
                        }
                    }

                }
                else
                {
                    hs.Add("term1", "1");
                }

                hs.Add("include_private", false);
                hs.Add("pagesize", 25);
                hs.Add("pagenumber", 1);
                hs.Add("sort", 1);
                hs.Add("minpagelookahead", 1);
                hs.Add("maxpagelookahead", 5000);
                hs.Add("lookahead_factor", 20);

                if (needFacet == "1")
                {
                    hs.Add("include_facets", true);
                }
                else
                {
                    hs.Add("include_facets", false);                   
                }
                hs.Add("facettype1", 3);
                hs.Add("facettype2", 4);
                hs.Add("facettype3", 5);
                hs.Add("facettype4", 7);
                hs.Add("facettype5", 10);
                hs.Add("facettype6", 2);
                hs.Add("facettype7", 120);

                //metadateType: 3-language; 4-creator; 5-publisher 7: subject topic; 10 subject geographic area                
                hs.Add("total_items", ""); // output
                if (order == "1")
                {
                    hs.Add("pageOrder", 1);
                }
                string strSql = "DPanther_FacetSearch_Paged";
                if (sort == "date")
                {
                    strSql = "DPanther_FacetSearch_PagedByDate";
                }
                DataSet ds = DBO.exeDataset(strSql, "DPantherClean", hs);

                string strFI = "";

                //successfully excute the store procedure
                if (ds.Tables.Count > 1)//has data returned
                {
                    DataTable dt = ds.Tables[0];
                    reDO.Total = dt.Rows.Count.ToString();
                    reDO.Pager = strPage;
                    int startNum = (Convert.ToInt32(strPage) - 1) * Convert.ToInt32(strPageLimit);
                    int endNum = (Convert.ToInt32(strPage) - 1) * Convert.ToInt32(strPageLimit) + Convert.ToInt32(strPageLimit);
                    if (endNum >= Convert.ToInt32(reDO.Total))
                    {
                        endNum = Convert.ToInt32(reDO.Total);
                    }
                    if (dt.Rows.Count > 0)
                    {
                        //return one page data
                        for (int i = startNum; i < endNum; i++)
                        {
                            DataRow dr = dt.Rows[i];
                            strFI = addListwizFacet(ref reDOList, dr);

                        }
                        //return the location dataset
                        List<string> fiNo = new List<string>();
                        for (int j = 0; j < dt.Rows.Count; j++)
                        {
                            DataRow drWhole = dt.Rows[j];                          
                            
                            fiNo.Add(drWhole["BibID"].ToString().Trim().Trim('|').Trim() + "-" + drWhole["VID"].ToString().Trim().Trim('|').Trim());
                        }
                        reDO.FIlist = fiNo;
                     
                    }
                    reDO.DigitalObjectList = reDOList;

                    if (needFacet == "1")
                    {
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
                            dtAgg = ds.Tables[1];
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
                    }

                    Hashtable hsagg = new Hashtable();
                    hsagg.Add("aggcode", strAgg);
                    if (strAgg == "CGM" || strAgg == "cgm" || strAgg=="usach" || strAgg=="USACH")
                    {
                        string strSqlgetYear = "DPanther_GetpubYear";
                        DataSet dsYear = DBO.exeDataset(strSqlgetYear, "DPantherClean", hsagg);
                        reDO.MaxYear = dsYear.Tables[0].Rows[0]["maxYear"].ToString();
                        reDO.MinYear = dsYear.Tables[0].Rows[0]["minYear"].ToString();
                    }
                    else
                    {
                        reDO.MaxYear = DateTime.Now.Year.ToString();
                        reDO.MinYear = "0";
                    }
                }
               
            }

            catch (Exception ex)
            {            
                throw ex;
            }
            return reDO;
        }

        /// <summary>
        /// test function
        /// </summary>
        /// <param name="strAgg"></param>
        /// <param name="condition"></param>
        /// <param name="strStartYear"></param>
        /// <param name="strEndYear"></param>
        /// <param name="strPage"></param>
        /// <param name="strPageLimit"></param>
        /// <param name="sort"></param>
        /// <param name="needFacet"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public static dpReturnedCollectionBrief getDigitalObjectByAggregationPageTest(string strAgg, string condition, string strStartYear, string strEndYear, string strPage, string strPageLimit, string sort, string needFacet, string order = "0")
        {
            dpReturnedCollectionBrief reDO = new dpReturnedCollectionBrief();
            List<DigitalObjectBrif> reDOList = new List<DigitalObjectBrif>();
            //List<DigitalObjectBrif> reDOListWhole = new List<DigitalObjectBrif>();

            ReturnedFacetObject reFacetObj = new ReturnedFacetObject();

            try
            {
                Hashtable hs = new Hashtable();
                if (strAgg == "1")
                {
                    hs.Add("aggregationcode", "all");
                }
                else
                {
                    hs.Add("aggregationcode", strAgg);
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
                    //add by Meng 7/1/2015 get metadataTypeID from database
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

                                //valid search term; todo: need to move to General function in business;
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

                                hs.Add(strterm, strFormatTerm);
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
                                 //get metadataTypeID
                                if (strFieldCode == "Aggregation")
                                {
                                    strFieldCode = "120";
                                }
                                else if(strFieldCode == "Language")
                                {
                                    strFieldCode = "3";
                                }
                                 else if(strFieldCode == "Publisher")
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

                hs.Add("include_private", false);
                hs.Add("pagesize", 25);
                hs.Add("pagenumber", 1);
                hs.Add("sort", 1);
                hs.Add("minpagelookahead", 1);
                hs.Add("maxpagelookahead", 5000);
                hs.Add("lookahead_factor", 20);

                if (needFacet == "1")
                {
                    hs.Add("include_facets", true);
                }
                else
                {
                    hs.Add("include_facets", false);
                }
                hs.Add("facettype1", 3);
                hs.Add("facettype2", 4);
                hs.Add("facettype3", 5);
                hs.Add("facettype4", 7);
                hs.Add("facettype5", 10);
                hs.Add("facettype6", 2);
                hs.Add("facettype7", 120);

                //metadateType: 3-language; 4-creator; 5-publisher 7: subject topic; 10 subject geographic area                
                hs.Add("total_items", ""); // output
                if (order == "1")
                {
                    hs.Add("pageOrder", 1);
                }
                string strSql = "DPanther_FacetSearch_Paged";
                if (sort == "date")
                {
                    strSql = "DPanther_FacetSearch_PagedByDate";
                }
                DataSet ds = DBO.exeDataset(strSql, "DPantherClean", hs);

                string strFI = "";

                //successfully excute the store procedure
                if (ds.Tables.Count > 1)//has data returned
                {
                    DataTable dt = ds.Tables[0];
                    reDO.Total = dt.Rows.Count.ToString();
                    reDO.Pager = strPage;
                    int startNum = (Convert.ToInt32(strPage) - 1) * Convert.ToInt32(strPageLimit);
                    int endNum = (Convert.ToInt32(strPage) - 1) * Convert.ToInt32(strPageLimit) + Convert.ToInt32(strPageLimit);
                    if (endNum >= Convert.ToInt32(reDO.Total))
                    {
                        endNum = Convert.ToInt32(reDO.Total);
                    }
                    if (dt.Rows.Count > 0)
                    {
                        //return one page data
                        for (int i = startNum; i < endNum; i++)
                        {
                            DataRow dr = dt.Rows[i];
                            strFI = addListwizFacet(ref reDOList, dr);

                        }
                        //return the location dataset
                        List<string> fiNo = new List<string>();
                        for (int j = 0; j < dt.Rows.Count; j++)
                        {
                            DataRow drWhole = dt.Rows[j];
                            fiNo.Add(drWhole["BibID"].ToString().Trim().Trim('|').Trim() + "-" + drWhole["VID"].ToString().Trim().Trim('|').Trim());

                        }
                        reDO.FIlist = fiNo;

                    }
                    reDO.DigitalObjectList = reDOList;

                    if (needFacet == "1")
                    {
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
                            dtAgg = ds.Tables[1];
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
                    }

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
                }

            }

            catch (Exception ex)
            {
                throw ex;
            }
            return reDO;
        }

        /// <summary>
        /// get the list of facet result
        /// </summary>
        /// <param name="dt">facet datatable from DB by facet type</param>
        /// <returns>a list of the facet result including value and count</returns>
        private static List<FacetObject> facetResult(DataTable dt)
        {
            List<FacetObject> liFO = new List<FacetObject>();
            try
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];
                    FacetObject fo = new FacetObject();
                    fo.Attribute = dr["MetadataValue"].ToString();
                    fo.Count = dr["Metadata_Count"].ToString();
                    if (fo.Attribute != "iFIU" && fo.Attribute != "ALL")
                    { liFO.Add(fo); }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return liFO;
        }

        /// <summary>
        /// bulid the list of digital collections with detailed information
        /// </summary>
        /// <param name="li">returned list of digital collections</param>
        /// <param name="dr">data record from DB</param>
        /// <returns>bibID of each record</returns>
        private static string addListwizFacet(ref List<DigitalObjectBrif> li, DataRow dr)
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
               
                digiBrif.BibID = dr["BibID"].ToString().Trim().Trim('|').Trim();
                digiBrif.VID = dr["VID"].ToString().Trim();
              

                strFI = digiBrif.BibID;

                //get the download link from the mets file;
                string strPath = "\\" + digiBrif.BibID.Substring(0, 2)
                                + "\\" + digiBrif.BibID.Substring(2, 2)
                                + "\\" + digiBrif.BibID.Substring(4, 2)
                                + "\\" + digiBrif.BibID.Substring(6, 2)
                                + "\\" + digiBrif.BibID.Substring(8, 2)
                                + "\\" + digiBrif.VID + "\\";
                ArrayList downPath = GeneralFunction.getPath(strPath + digiBrif.BibID + "_" + digiBrif.VID + ".mets.xml");
                digiBrif.DownloadLink = downPath;
                li.Add(digiBrif);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return strFI;
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
