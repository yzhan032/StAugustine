using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data.SqlClient;


namespace DPantherBusiness
{
    /// <summary>
    /// database configuration class
    /// </summary>
    public class Config
    {
        /// <summary>
        /// dPanther database connection string
        /// </summary>
        public static string ConnectString;

        /// <summary>
        /// const of dPanther mail host
        /// </summary>
        public const string MailHost = "";

        /// <summary>
        /// const of dPanther web mail address
        /// </summary>
        public const string LevWebMasterEmailAddress = "";

        /// <summary>
        /// set up database connection string for different databases
        /// </summary>
        /// <param name="strName">project name [DPantherClean,BIKE,GLOWS]</param>
        public Config(string strName)
        {
            switch (strName)
            {
                case "DPantherClean":
                    ConnectString = ConfigurationManager.ConnectionStrings["DPantherCleanConnectionString"].ConnectionString;
                    break;                             
                default:
                    ConnectString = ConfigurationManager.ConnectionStrings["DPantherCleanConnectionString"].ConnectionString;
                    break;
            }
            
        }

        /// <summary>
        /// get a database sql connection
        /// </summary>
        /// <returns>a new instance of a sql connection</returns>
        public SqlConnection GetConnection()
        {            
            return (new SqlConnection(ConnectString));
        }
    }
}
