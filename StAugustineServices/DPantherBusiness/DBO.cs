using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Reflection;

namespace DPantherBusiness
{
    /// <summary>
    /// database object class
    /// </summary>
    public static class DBO
    {
        /// <summary>
        /// execute a store procedure to return a dataset
        /// </summary>
        /// <param name="sSQL">The name of the store procedure</param>
        /// <param name="dbName">The name of the database</param>
        /// <param name="hParameter">the parameter table</param>
        /// <returns>
        /// a dataset of the result
        /// </returns>
        public static DataSet exeDataset(string sSQL, string dbName = null, Hashtable hParameter = null)
        {
            
            DataSet dsRet = new DataSet();
            Config cf = new Config(dbName);
            using (SqlConnection con = cf.GetConnection())
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter()) 
                {
                    con.Open();
                    adapter.SelectCommand = new SqlCommand(sSQL, con);
                    adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                    BuildSqlParameterCollection(hParameter, adapter.SelectCommand.Parameters);
                    adapter.Fill(dsRet);
                    con.Close();
                }
            }
            return dsRet;
        }

        /// <summary>
        /// execute a store procedure to return a dataset
        /// </summary>
        /// <param name="sSQL">The name of the store procedure</param>
        /// <param name="dbName">The name of the database</param>
        /// <returns>
        /// a dataset of the result
        /// </returns>
        public static DataSet exeDatasetText(string sSQL, string dbName = null)
        {

            DataSet dsRet = new DataSet();
            Config cf = new Config(dbName);
            using (SqlConnection con = cf.GetConnection())
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter())
                {
                    con.Open();
                    adapter.SelectCommand = new SqlCommand(sSQL, con);
                    adapter.SelectCommand.CommandType = CommandType.Text;
                    adapter.Fill(dsRet);
                    con.Close();
                }
            }
            return dsRet;
        }

        /// <summary>
        /// execute a sql statement to return a single value
        /// </summary>
        /// <param name="sSQL">the sql statement</param>
        /// <param name="dbName">The name of the database</param>
        /// <returns>
        /// a object value
        /// </returns>
        public static object returnOneFieldValue(string sSQL, string dbName = null)
        {
            object fieldValue = null;
            Config cf = new Config(dbName);
            using (SqlConnection con = cf.GetConnection())
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(sSQL, con))
                {
                    cmd.CommandType = CommandType.Text;
                    SqlDataReader rd = cmd.ExecuteReader();
                    if (rd.Read()) fieldValue = rd.GetValue(0);
                }
                con.Close();
            }
            return fieldValue;

        }

        /// <summary>
        /// execute a store procedure to return a single value 
        /// </summary>
        /// <param name="sSQL">The name of the store procedure</param>
        /// <param name="dbName">The name of the database</param>
        /// <param name="hParameter">the parameter table</param>
        /// <returns>
        /// a object value
        /// </returns>
        public static object returnOneFieldValue(string sSQL, string dbName = null, Hashtable hParameter = null)
        {
            object fieldValue = null;
            Config cf = new Config(dbName);
            using (SqlConnection con = cf.GetConnection())
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(sSQL, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    BuildSqlParameterCollection(hParameter, cmd.Parameters);
                    SqlDataReader rd = cmd.ExecuteReader();
                    if (rd.Read()) fieldValue = rd.GetValue(0);
                }
                con.Close();
            }
            return fieldValue;

        }

        /// <summary>
        /// execute a store procedure to  get a return value from SP
        /// </summary>
        /// <param name="sSQL">The name of the store procedure</param>
        /// <param name="outputID">The name of the output parameter</param>
        /// <param name="dbName">The name of the database</param>
        /// <param name="hParameter">the parameter table</param>
        /// <returns>
        /// a object value returned from SP
        /// </returns>
        public static object returnValueOfSP(string outputID,string sSQL, string dbName = null, Hashtable hParameter = null)
        {
        
            object fieldValue = null;
            Config cf = new Config(dbName);
            using (SqlConnection con = cf.GetConnection())
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(sSQL, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                   
                    BuildSqlParameterCollection(hParameter, cmd.Parameters);

                    cmd.Parameters.Add(new SqlParameter(outputID, SqlDbType.Int));
                    cmd.Parameters[outputID].Direction = ParameterDirection.Output;
                    SqlDataReader rd = cmd.ExecuteReader();
                    //if (rd.Read()) fieldValue = rd.GetValue(0);
                    fieldValue = (int)cmd.Parameters[outputID].Value;
                }
                con.Close();
            }
            return fieldValue;

        }

        /// <summary>
        /// check if the data esist based a sql statement
        /// </summary>
        /// <param name="sSQL">
        /// the sql statement
        /// </param>
        /// <param name="dbName">The name of the database</param>
        /// <returns>
        /// a bool flag
        /// </returns>
        public static bool hasRecords(string sSQL, string dbName = null)
        {
            bool hasFlag = false;
            Config cf = new Config(dbName);
            using (SqlConnection con = cf.GetConnection())
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(sSQL, con))
                {
                    cmd.CommandType = CommandType.Text;
                    SqlDataReader rd = cmd.ExecuteReader();
                    hasFlag = rd.HasRows;
                }
                con.Close();
            }
            return hasFlag;
        }

        /// <summary>
        /// check if the database connection can be established
        /// </summary>
        /// <param name="dbName">The name of the database</param>
        /// <returns>
        /// a bool flag
        /// </returns>
        public static bool dbChecker(string dbCnn)
        {
            bool hasFlag = true;
            
            using (SqlConnection con = new SqlConnection(dbCnn))
            {
                try
                {
                    con.Open();

                    con.Close();
                }
                catch (Exception ex)
                {
                    //throw ex;
                    hasFlag = false;
                }

            }
            return hasFlag;
        }

        /// <summary>
        /// ExecuteNonQuery for insert, update, and delete function based on a sql statement
        /// </summary>
        /// <param name="sSQL">The sql statement</param>
        /// <param name="dbName">The name of the database</param>
        public static void exeSQL(string sSQL, string dbName = null)
        {
            Config cf = new Config(dbName);
            using (SqlConnection con = cf.GetConnection())
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(sSQL, con))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                }
                con.Close();
            }
        }

        /// <summary>
        /// ExecuteNonQuery for insert, update, and delete function based on a sql statement with parameters
        /// </summary>
        /// <param name="sSQL">The sql statement</param>
        /// <param name="dbName">The name of the database</param>
        /// <param name="hParameter">the parameters table</param>
        public static void exeSQLText(string sSQL, string dbName = null, Hashtable hParameter = null)
        {
            Config cf = new Config(dbName);
            using (SqlConnection con = cf.GetConnection())
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter())
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sSQL, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        BuildSqlParameterCollection(hParameter, cmd.Parameters);
                        cmd.ExecuteNonQuery();
                    }
                    con.Close();

                }
            }
        }

        /// <summary>
        /// ExecuteNonQuery for insert, update, and delete function based on a store procedure
        /// </summary>
        /// <param name="SPName">the name of the store procedure</param>
        /// <param name="dbName">The name of the database</param>
        /// <param name="hParameter">the parameters table</param>
        public static void exeSQL(string SPName, string dbName = null, Hashtable hParameter = null)
        {
            Config cf = new Config(dbName);
            using (SqlConnection con = cf.GetConnection())
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter())
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(SPName, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        BuildSqlParameterCollection(hParameter, cmd.Parameters);
                        cmd.ExecuteNonQuery();
                    }
                    con.Close();

                }
            }
        }
        /// <summary>
        /// convert hashtable to sql parameter list
        /// </summary>       
        /// <param name="hParameter">the parameters table</param>
        /// <returns>
        /// a sql parameter list
        /// </returns>
        public static SqlParameter[] BuildSqlParameters(Hashtable hParameter)
        {
            if (hParameter == null)
                return null;

            List<SqlParameter> lParams = new List<SqlParameter>();
            foreach (string paramName in hParameter.Keys)
            {
                lParams.Add(new SqlParameter(paramName, hParameter[paramName]));
            }
            return lParams.ToArray();
        }

        /// <summary>
        /// set a collection of sql parameters from a hashtable 
        /// </summary>       
        /// <param name="hParameter">the hashtable</param>
        /// <param name="paramCollection">the sqlparameterCollection to be set</param>
        public static void BuildSqlParameterCollection(Hashtable hParameter, SqlParameterCollection paramCollection)
        {
            if (hParameter == null)
                return;
            foreach (string paramName in hParameter.Keys)
            {
                paramCollection.Add(new SqlParameter(paramName, hParameter[paramName]));
            }
        }

        /// <summary>
        /// return the XML string of a specific search result;
        /// </summary>
        /// <param name="procedureName">name of a store procedure</param>
        /// <param name="hParameter">the parameters table</param>
        /// <param name="dbName">database name</param>
        /// <returns>string of the XML result</returns>
        public static string GetXmlStringFromStoredProcedure(string procedureName, Hashtable hParameter, string dbName = null)
        {
            StringBuilder result = new StringBuilder();
            Config cf = new Config(dbName);
            using (SqlConnection con = cf.GetConnection())
            {

                con.Open();
                using (SqlCommand cmd = new SqlCommand(procedureName, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    BuildSqlParameterCollection(hParameter, cmd.Parameters);
                    System.Xml.XmlReader xmlReader = cmd.ExecuteXmlReader();
                    xmlReader.Read();
                    while (!xmlReader.EOF)
                    {
                        result.Append(xmlReader.ReadOuterXml());
                    }
                }
                con.Close();
            }
            return result.ToString();
        }

        /// <summary>
        /// return the busunessObject list from a store procedure; 
        /// To be tested
        /// </summary>
        /// <param name="procName">name of a store procedure</param>
        /// <param name="dbName">database name</param>
        /// <param name="hParameter">the parameters table</param>
        /// <typeparam name="T">generic type, e.g. int,float,bool</typeparam>
        /// <returns>a generic list of businessObject</returns>
        public static List<T> GetBusinessObjectListFromProc<T>(string procName, string dbName = null, Hashtable hParameter = null) where T : new()
        {

            List<T> result = new List<T>();
            DataSet dsRet = new DataSet();
            Config cf = new Config(dbName);
            using (SqlConnection dbConnection = cf.GetConnection())
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter())
                {
                    adapter.SelectCommand = new SqlCommand(procName, dbConnection);
                    adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                    BuildSqlParameterCollection(hParameter, adapter.SelectCommand.Parameters);
                    adapter.Fill(dsRet);
                }
            }

            if (dsRet.Tables[0] != null)
            {
                foreach (DataRow row in dsRet.Tables[0].Rows)
                {
                    T businessEntity = new T();
                    SetPropertiesFromDataRow(row, businessEntity);
                    result.Add(businessEntity);
                }
            }
            return result;
        }

        /// <summary>
        /// return the busunessObject list from a dataset; 
        /// To be tested
        /// </summary>
        /// <param name="dsSet">name of a dataset</param>
        /// <param name="indexTable">index table of the dataset</param>
        /// <typeparam name="T">generic type, e.g. int,float,bool</typeparam>
        /// <returns>a generic list of businessObject</returns>
        public static List<T> GetBusinessObjectListFromDataSet<T>(DataSet dsSet, int indexTable) where T : new()
        {
            List<T> result = new List<T>();

            if (dsSet.Tables[indexTable] != null) 
            {
                foreach (DataRow row in dsSet.Tables[indexTable].Rows)
                {
                    T businessEntity = new T();
                    SetPropertiesFromDataRow(row, businessEntity);
                    result.Add(businessEntity);
                }
            }
            return result;
        }

        /// <summary>
        /// set property value from a datarow 
        /// </summary>       
        /// <param name="dataRow">the parameter row</param>
        /// <param name="businessEntity">the propertyInfo object</param>
        private static void SetPropertiesFromDataRow(DataRow dataRow, object businessEntity)
        {
            if (dataRow == null)
                return;
            PropertyInfo[] propertyInfos;
            propertyInfos = businessEntity.GetType().GetProperties();
            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                if (dataRow.Table.Columns.Contains(propertyInfo.Name))
                    propertyInfo.SetValue(businessEntity, dataRow[propertyInfo.Name].ToString(), null);

            }
        }
    }
}
