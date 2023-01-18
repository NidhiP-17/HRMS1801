using HRMS.Models;
using System.Data;
using System.Data.SqlClient;

namespace HRMS.Classes
{
    public class Global
    {
        public static string gstrUserName = "";
        public static int gintErrorNo = 0;
        public static string gstrErrorDesc = "";

        //Constant
        public const string SessionKeyId = "ID";
        public const string SessionKeyName = "Name";
        public static bool GetSession(string? SessionKey)
        {
            if (string.IsNullOrEmpty(SessionKey))
            {
                return false;
            }
            else
            {
                return true;

            }
        }
        public static bool getIDValue(string strQry, string strConnections)
        {
            SqlCommand sqlComm;
            SqlDataReader sqlDataReader;

            try
            {
                using (SqlConnection SqlConn = new SqlConnection(strConnections))
                {
                    SqlConn.Open();
                    sqlComm = new SqlCommand(strQry, SqlConn);
                    sqlDataReader = sqlComm.ExecuteReader();

                    if (sqlDataReader.Read())
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                    sqlComm = null;
                    sqlDataReader = null;

                }
            }
            catch
            {
                return false;
            }
        }
        public static string? getValue(string strQry, string strConnections)
        {
            SqlCommand sqlComm;
            SqlDataReader sqlDataReader;

            try
            {
                using (SqlConnection SqlConn = new SqlConnection(strConnections))
                {
                    SqlConn.Open();
                    sqlComm = new SqlCommand(strQry, SqlConn);
                    sqlDataReader = sqlComm.ExecuteReader();

                    if (sqlDataReader.Read())
                    {
                        return sqlDataReader[0].ToString();
                    }
                    else
                    {
                        return "";

                    }
                    sqlComm = null;
                    sqlDataReader = null;
                }
            }
            catch
            {
                return "";
            }
        }
    }

}
