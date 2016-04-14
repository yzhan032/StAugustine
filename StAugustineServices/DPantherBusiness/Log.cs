using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.IO;

namespace DPantherBusiness
{
    /// <summary>
    /// system log class
    /// </summary>
    public static class Log
    {
        /// <summary>
        /// add log to database
        /// </summary>
        /// <param name="userInfo">user name</param>
        /// <param name="strActioin">user action such as log in, save, update</param>
        /// <param name="strDesc">Detailed description of user action</param>
        /// <param name="strType">log type [1- dblog;2-searchlog;3-userlog;4-operationlog;5-filelog;6-errorlog]</param>
        /// <param name="strApp">application ID</param>
        public static void addLog(string userInfo, string strActioin, string strDesc, string strType, string strApp)
        {
            try
            {
                string strTime = DateTime.Now.ToString();
                Hashtable ht = new Hashtable();
                ht.Add("userInfo", userInfo);
                ht.Add("opTime", DateTime.Now.ToString());
                ht.Add("action", strActioin);
                ht.Add("description", strDesc);
                ht.Add("typeID", strType);
                ht.Add("applicationID", strApp);

                DBO.exeSQL("DPanther_logInsert", "DPantherClean", ht);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        /// <summary>
        /// add file log into dpLog/itemLog folder for any item search behaviors
        /// </summary>
        /// <param name="strUser">user id</param>
        /// <param name="strUserAddress">user's ip address</param>
        /// <param name="finums">array of FI numbers involved in this behaviors</param>
        /// <param name="condition">search condition</param>
        /// <returns>log status</returns>
        public static string addItemSearchLog(string strUser, string strUserAddress, string[] finums, string condition)
        {
            string strReturn = "";
            try
            {
                string strPath = @dpDirectory.getDIRbyTypeAppcode("dpLog", "dpAdmin");
                string strLogName = "itemLog_" + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString();
                StreamWriter w;
                if (!File.Exists(strPath + "\\" + strLogName + "_0.txt"))
                {
                    w = new StreamWriter(strPath + "\\" + strLogName + "_0.txt");
                }
                else
                {
                    //check how many files are there for one day
                    //check file size for the last log, create a new file when exceeding 20MB ()
                    string[] dirs = Directory.GetFiles(strPath, strLogName + "*");
                    int iLast = 0;
                    for (int i = 0; i < dirs.Length; i++)
                    {
                        string[] strarr = dirs[i].Split('_');
                        int iCount = Convert.ToInt16(strarr[strarr.Length - 1].Split('.')[0]);
                        if (iLast < iCount)
                            iLast = iCount;
                    }

                    FileInfo fLastLog = new FileInfo(strPath + "\\" + strLogName + "_" + iLast.ToString() + ".txt");

                    if (fLastLog.Length < 20971520)
                    {
                        w = File.AppendText(fLastLog.FullName);
                    }
                    else
                    {
                        w = new StreamWriter(strPath + "\\" + strLogName + "_" + (iLast + 1).ToString() + ".txt");
                    }

                }

                w.Write("\r\nLog Entry : ");
                w.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(),
                    DateTime.Now.ToLongDateString());
                w.WriteLine("  User:" + strUser);
                w.WriteLine("  UserAddress:" + strUserAddress);
                w.WriteLine("  FIs:{0}", string.Join(",", finums));
                w.WriteLine("  condition:{0}", condition);
                w.WriteLine("-------------------------------");
                w.Flush();
                w.Close();

                strReturn = "collection stat record success.";
            }
            catch (Exception ex)
            {
                strReturn = "collection stat update failed because " + ex.Message.ToString();
                Log.addLog(strUser + strUserAddress, "collection stat update",
                    "collection stat update failed because of " + ex.Message.ToString(),
                    "4",
                    "19");
                throw (ex);
            }

            return strReturn;
        }
    }
}
