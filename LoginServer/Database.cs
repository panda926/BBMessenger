using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using ChatEngine;

namespace LoginServer
{    
    class Database : BaseInfo
    {
        private static Database _instance = null;

        public static string _IP = null;

        public static Database GetInstance()
        {
            if (_instance == null)
            {
                _instance = new Database();
            }

            return _instance;
        }

        private string GetConnectionString()
        {
            return "Data Source=" + _IP + ",9260" + ";Initial Catalog=dbChat;Persist Security Info=True;User ID=sa;Password=sasa;Connection Timeout=15";            
        }

        private SqlConnection ConnectDb()
        {
            string connectionString = GetConnectionString();

            SqlConnection connection = new SqlConnection(connectionString);

            try
            {
                connection.Open();
            }
            catch (Exception)
            {
                connection = null;
            }

            return connection;
        }

        private DataRowCollection GetData(string queryString)
        {
            SqlConnection connection = ConnectDb();
            if (connection == null)
                return null;

            DataRowCollection rows = null;

            try
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                SqlDataAdapter adapter = new SqlDataAdapter(command);

                DataSet dataSet = new DataSet();
                adapter.Fill(dataSet);

                if (dataSet.Tables.Count > 0)
                {
                    rows = dataSet.Tables[0].Rows;
                }
                else
                {                    
                }
            }
            catch (Exception er)
            {                
            }
            finally
            {
                connection.Close();
            }

            return rows;
        }

        public int GetUsingState()
        {
            int nResult = 0;
            string strOwnIP = GetIPAddress();

            string strQuery = string.Format("select * from tblUsingState where IP='{0}'", strOwnIP);
            DataRowCollection rows = GetData(strQuery);
            if (rows == null)
                return -1;

            if(rows.Count > 0)
            {
                nResult = ConvToInt(rows[0]["State"]);
            }

            return nResult;
        }

        public string GetIPAddress()
        {
            string strIPAddress = string.Empty;

            IPHostEntry IPHost = Dns.GetHostByName(Dns.GetHostName());            
            strIPAddress = IPHost.AddressList[0].ToString();

            return strIPAddress;
        }
    }
}
