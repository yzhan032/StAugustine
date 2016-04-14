using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using System.Data;
using DPantherBusiness;
using System.Net;
using System.Web;
using System.Collections;
using System.Configuration;
using System.IO;
using System.Xml.Linq;
using DPantherModels;
using DPantherModels.dpModels;

namespace dpRestService
{
    // Start the service and browse to http://<machine_name>:<port>/Service1/help to view the service's generated help page
    // NOTE: By default, a new instance of the service is created for each call; change the InstanceContextMode to Single if you want
    // a single instance of the service to process all calls.	
    /// <summary>
    /// dPanther RESTful service: digital collection
    /// </summary>
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    // NOTE: If the service is renamed, remember to update the global.asax.cs file
    public class dpCollectionService
    {
        // TODO: Implement the collection resource that will contain the SampleItem instances
        private string strUser = "";
        private string strUserAddress = "";
        /// <summary>
        /// user name
        /// </summary>
        private string username = ConfigurationManager.AppSettings["domainuser"];

        /// <summary>
        /// domain name
        /// </summary> 
        private string domain = ConfigurationManager.AppSettings["domainname"];

        /// <summary>
        /// user password
        /// </summary>
        private string password = ConfigurationManager.AppSettings["domainuserpw"];

        private string dirPath = dpDirectory.getDIRbyTypeAppcode("colRepo", "dpAdmin");

        /// <summary>
        /// get detailed information of digital collections from dPanther database (for the dpanther right panel full citation, list of files and map it)
        /// </summary>
        /// <param name="finum">FI number: FI12041019 (fuzzy query in the database, FI number could be FI1204)</param>
        /// <returns>
        /// </returns>
        [OperationContract]
        [WebGet(UriTemplate = "/collections/testByFI/{bibid}/{VID=00001}", ResponseFormat = WebMessageFormat.Json)]
        public dpItem testByFI(string bibid, string vid = "00001")
        {
            dpItem li = new dpItem();
            try
            {


                Hashtable ht = new Hashtable();
                ht.Add("FINum", bibid);
                ht.Add("vid", vid);

                string strSpName = "DPanther_ItemGetByFI";
                DataSet dsResult = DBO.exeDataset(strSpName, "DPantherClean", ht);
                DataTable dtResult = dsResult.Tables[0];
                if (dtResult.Rows.Count > 0)
                {

                    DataRow dr = dtResult.Rows[0];

                    this.iniItem(ref li, dr);


                }
            }
            catch (Exception ex)
            {
                Log.addLog(strUser + strUserAddress, "Get list by FI",
                    "Get detail for FI " + bibid + " failed because of " + ex.Message.ToString(),
                    "4",
                    "19");
                throw (ex);
            }

            return li;

        }

        /// <summary>
        /// get detailed information of digital collections from dPanther database (for the dpanther right panel full citation, list of files and map it)
        /// </summary>
        /// <param name="finum">FI number: FI12041019 (fuzzy query in the database, FI number could be FI1204)</param>
        /// <returns>
        /// </returns>
        [OperationContract]
        [WebGet(UriTemplate = "/collections/getDetailByFI/{bibid}/{VID=00001}", ResponseFormat = WebMessageFormat.Json)]
        public dpItem getColDetailByFI(string bibid,string vid="00001")
        {
            dpItem li = new dpItem();
            string hostName = dpDirectory.getServiceURLbyTypeAppcode("services", "dpAdmin", "dpCollectionService");
            strUser = "";
            strUserAddress = HttpContext.Current.Request.UserHostAddress.ToString();
            try
            {


                Hashtable ht = new Hashtable();
                ht.Add("FINum", bibid);
                ht.Add("vid",vid);

                string strSpName = "DPanther_ItemGetByFI";
                DataSet dsResult = DBO.exeDataset(strSpName, "DPantherClean", ht);
                DataTable dtResult = dsResult.Tables[0];
                if (dtResult.Rows.Count > 0)
                {
                   
                        DataRow dr = dtResult.Rows[0];

                        this.iniItem(ref li, dr, hostName);
                      
                  
                }
                //get aggregation code list
                DataTable dtResultAgg = dsResult.Tables[1];
                ArrayList aggList = new ArrayList();

                if (dtResultAgg.Rows.Count > 0)
                {
                    for (int i = 0; i < dtResultAgg.Rows.Count; i++)
                    {
                        aggList.Add(dtResultAgg.Rows[i]["Code"].ToString());     
                    }

                }
                li.aggregationCode = aggList;

                //get wordmark icon url list
                DataTable dtResultWordMark = dsResult.Tables[2];
                //ArrayList wordMark = new ArrayList();
                List<WordMark> liwordMark = new List<WordMark>();

                if (dtResultWordMark.Rows.Count > 0)
                {
                    for (int i = 0; i < dtResultWordMark.Rows.Count; i++)
                    {
                        WordMark wordMark = new WordMark();

                        wordMark.Icon_url=dtResultWordMark.Rows[i]["Icon_URL"].ToString();
                        wordMark.IconID = dtResultWordMark.Rows[i]["IconID"].ToString();
                        wordMark.Icon_Name = dtResultWordMark.Rows[i]["Icon_Name"].ToString();
                        wordMark.link = dtResultWordMark.Rows[i]["Link"].ToString();
                        wordMark.Title = dtResultWordMark.Rows[i]["Title"].ToString();
                        liwordMark.Add(wordMark);
                    }

                }

                li.wordMark = liwordMark;
                Log.addLog(strUser + strUserAddress, "Get detail by FI",
                    "Get list for FI " + bibid + " successfully.",
                    "4",
                    "19");

                //check if this record has KML attached
                Hashtable ht_kml = new Hashtable();
                ht_kml.Add("FINum", bibid);


                string strSpKml = "DPanther_KMLGetByFI";
                DataSet dsResult_KML = DBO.exeDataset(strSpKml, "DPantherClean", ht_kml);
                DataTable dtResult_KML = dsResult_KML.Tables[0];
                if (dtResult_KML.Rows.Count > 0)
                {
                    for (int i = 0; i < dtResult_KML.Rows.Count; i++)
                    {
                        DPantherModels.dpModels.dpKML kml = new DPantherModels.dpModels.dpKML();
                        DataRow dr = dtResult_KML.Rows[i];

                        this.iniKML(ref kml, dr);

                        li.kml = kml;                        
                    }
                }

            }
            catch (Exception ex)
            {
                Log.addLog(strUser + strUserAddress, "Get list by FI",
                    "Get detail for FI " + bibid + " failed because of " + ex.Message.ToString(),
                    "4",
                    "19");
                throw (ex);
            }

            return li;
        }
    
     
        //method to initialize a collection object;
        private void iniCollectionBrief(ref dpCollectionBrief collection, DataRow dr)
        {
            try
            {
                collection.BibID = dr["BibID"].ToString();
                collection.Title = dr["Title"].ToString();                

            }
            catch (Exception ex)
            {
                Log.addLog("", "initial collection brief", "initial collection object faild because of " + ex.Message.ToString(), "4", "19");
                throw ex;
            }
        }    

        //method to initialize item detail object;
        private void iniItem(ref dpItem Item, DataRow dr, string hostName="")
        {
            try
            {
                Item.BibID = dr["BibID"].ToString();
                Item.VID = dr["VID"].ToString();
                Item.mainLat = dr["MainLatitude"].ToString();
                Item.mainLng = dr["MainLongitude"].ToString();                      
                Item.publishPlace = dr["publishPlace"].ToString();
                Item.mainThumbnail = dr["mainThumbnail"].ToString();
                Item.BibID = GeneralFunction.getFIString(Item.BibID.ToString());
                if (hostName != "")
                {
                    //get the download link from the mets file;
                    string strPath = "\\" + Item.BibID.Substring(0, 2)
                                + "\\" + Item.BibID.Substring(2, 2)
                                + "\\" + Item.BibID.Substring(4, 2)
                                + "\\" + Item.BibID.Substring(6, 2)
                                + "\\" + Item.BibID.Substring(8, 2)
                                + "\\" + Item.VID + "\\";
                    ArrayList downPath = GeneralFunction.getPath(strPath + Item.BibID + "_" + Item.VID + ".mets.xml");

                    string downPathNew = GeneralFunction.getPathNew(strPath + Item.BibID + "_" + Item.VID + ".mets.xml");
                    Item.downloadtree = downPathNew;


                    Item.downloadlink = downPath;
                    //search file in directory: strPath
                    string dirSobekContentPath = dpDirectory.getDIRbyTypeAppcode("sobekCitationUrl", "dpAdmin");

                    Item.htmlContent = GeneralFunction.getSobekContentHTMLfromURL(dirSobekContentPath + Item.BibID + "/" + Item.VID + "/citation", hostName);
                }
                //Item.htmlContent = GeneralFunction.getSobekContentHTML(strPath + "sobek_files\\" + Item.BibID + "_" + Item.VID + ".html");              


            }
            catch (Exception ex)
            {
                Log.addLog("", "initial item", "initial item object faild because of " + ex.Message.ToString(), "4", "19");
                throw ex;
            }
        }

        private void iniKML(ref DPantherModels.dpModels.dpKML kml, DataRow dr)
        {
            try
            {
                kml.kmlid = Convert.ToInt16(dr["kmlid"]);
                kml.path = dr["path"].ToString();
                kml.description = dr["description"].ToString();
                kml.keyword = dr["keyword"].ToString();
                kml.userCreated = dr["userCreated"].ToString();
                kml.dateCreated = dr["dateCreated"].ToString();
                kml.FINum = dr["FINum"].ToString();

            }
            catch (Exception ex)
            {
                Log.addLog("", "initial kml", "initial kml object faild because of " + ex.Message.ToString(), "4", "19");
                throw ex;
            }
        }

    }
}