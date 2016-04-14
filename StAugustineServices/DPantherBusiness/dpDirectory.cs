using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Collections;
using System.IO;
using System.Data;
using System.Web;



namespace DPantherBusiness
{
    /// <summary>
    /// dPanther directory class
    /// </summary>
    public class dpDirectory
    {
        // Fields: 
       private string _dirPath;
       private string _webUrl;
       private string _dirType;
       private string _applicationName;
       private string _applicationCode;       
              

        // Property implementation: 
        /// <summary>
        /// directory path
        /// </summary>
        public string dirPath
       {
          get
          {
              return _dirPath;
          }

          set
          {
              _dirPath = value;
          }
       }

        /// <summary>
        /// the web access url of the directory
        /// </summary>
        public string webUrl
       {
          get
          {
              return _webUrl;
          }
          set
          {
              _webUrl = value;
          }
       }

        /// <summary>
        /// directory type: [1-kmlHarvest;2-kmlPublish ;3-kmlPublishURL;4-pubRequest;5-pubTpl;6-geoporatlTemp]
        /// </summary>
        public string dirType
        {
            get
            {
                return _dirType;
            }
            set
            {
                _dirType = value;
            }
        }

        /// <summary>
        /// application name 
        /// </summary>
        public string applicationName
        {
            get
            {
                return _applicationName;
            }
            set
            {
                _applicationName = value;
            }
        }

        /// <summary>
        /// applicaiton code in the database
        /// </summary>
        public string applicationCode
        {
            get
            {
                return _applicationCode;
            }
            set
            {
                _applicationCode = value;
            }
        }

        /// <summary>
        /// directory class to manage application directories
        /// </summary>
        /// <param name="strType">
        /// the type name of the directory
        /// </param>
        /// <param name="strAppCode">
        /// application code
        /// </param>
        public dpDirectory(string strType=null, string strAppCode=null)
        {
            try
            {
                
                Hashtable ht = new Hashtable();
                ht.Add("applicationCode", strAppCode);
                ht.Add("dirType", strType);

                DataSet dsDir = DBO.exeDataset("DPanther_DirectoryGetByAppTypeName", "DPantherClean", ht);

                iniAttribues(dsDir);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void iniAttribues(DataSet dsReturn)
        {
            try
            {
                DataTable dt = dsReturn.Tables[0];
                DataRow dr = dt.Rows[0];
                this._applicationCode = dr["applicationCode"].ToString();
                this._applicationName = dr["applicationName"].ToString();
                this._dirPath = dr["dirPath"].ToString();
                this._dirType = dr["dirType"].ToString();
                this._webUrl = dr["webUrl"].ToString();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// get [dirPath] from dir table
        /// </summary>
        /// <param name="strType">
        /// type ID
        /// </param>
        /// <param name="strAppID">
        /// application ID
        /// </param>
        /// <returns>
        /// return the path as a string
        /// </returns>
        public string getDIRbyTypeApp(string strType, string strAppID)
        {
            string strDir = "";
            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("dirType", strType);
                ht.Add("appID", strAppID);

                strDir = (string)DBO.returnOneFieldValue("DPanther_DirectoryGetByAppType", "DPantherClean", ht);
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
            return strDir;
        }

        /// <summary>
        /// get [dirPath] from view [dpView_appDirs]
        /// </summary>
        /// <param name="strType">
        /// [disType] : "purlTemp", "geoporatlTemp", "pubTpl"
        /// </param>
        /// <param name="strAppCode">
        /// [applicationCode] : "dpAdmin", "WAWASH". "CGM"
        /// </param>
        /// <returns>
        /// [dirPath]@string: the access path based on the request
        /// </returns>
        public static string getDIRbyTypeAppcode(string strType, string strAppCode)
        {
            string strDir = "";
            try
            {
                DataTable dt =  dtDirectory(strType, strAppCode);
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    strDir = dr["dirPath"].ToString();
                }
                else
                {
                    strDir = "Requested Dir does not found.";
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return strDir;
        }

        /// <summary>
        /// get [webUrl] from view [dpView_appDirs]
        /// </summary>
        /// <param name="strType">
        /// [disType] : "purlTemp", "geoporatlTemp", "pubTpl"
        /// </param>
        /// <param name="strAppCode">
        /// [applicationCode] : "dpAdmin", "WAWASH". "CGM"
        /// </param>
        /// <returns>
        /// [webUrl]@string: the web access path based on the request
        /// </returns>
        public static string getDIRWebbyTypeAppcode(string strType, string strAppCode)
        {
            string strDir = "";
            try
            {
                DataTable dt = dtDirectory(strType, strAppCode);
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    strDir = dr["webUrl"].ToString();
                }
                else
                {
                    strDir = "Requested Dir does not found.";
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return strDir;
        }

        /// <summary>
        /// get [webUrl] from view [dpView_appDirs] for a service
        /// </summary>
        /// <param name="strType">
        /// [disType] : "purlTemp", "geoporatlTemp", "pubTpl"
        /// </param>
        /// <param name="strAppCode">
        /// [applicationCode] : "dpAdmin", "WAWASH". "CGM"
        /// </param>
        /// <param name="serviceName">
        /// [dirName] : "dpPurlService"
        /// </param>
        /// <returns>
        /// [webUrl]@string: the web access path based on the request
        /// </returns>
        public static string getServiceURLbyTypeAppcode(string strType, string strAppCode, string serviceName)
        {
            string strDir = "";
            try
            {
                DataTable dt = dtDirectory(strType, strAppCode);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dr = dt.Rows[i];
                        if (dr["dirName"].ToString() == serviceName)
                        {
                            strDir = dr["webUrl"].ToString();
                        }
                    }                   
                    
                }
                else
                {
                    strDir = "Requested Dir does not found.";
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return strDir;
        }

        /// <summary>
        /// Method to get all information from view [dpView_appDirs]
        /// </summary>
        /// <param name="strType">
        /// [disType] : "purlTemp", "geoporatlTemp", "pubTpl"
        /// </param>
        /// <param name="strAppCode">
        /// [applicationCode] : "dpAdmin", "WAWASH". "CGM"
        /// </param>
        /// <returns>
        /// datatable: [dirPath], [webUrl], [dirType], [applicationName], [applicationCode], [webAccess] 
        /// </returns>
        private static DataTable dtDirectory(string strType, string strAppCode)
        {
            DataTable dt = new DataTable();
            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("dirType", strType);
                ht.Add("applicationCode", strAppCode);

                DataSet ds  = DBO.exeDataset("DPanther_DirectoryGetByAppTypeName", "DPantherClean", ht);
                dt = ds.Tables[0];
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return dt;
        }
    }
}
