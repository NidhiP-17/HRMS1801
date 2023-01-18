using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class DBConnector : IDisposable
    {
        /// <summary>
        /// Flag: Has Dispose already been called
        /// </summary>
        bool disposed;

        SqlConnection sqlConnection;
        SqlCommand sqlCommand;

        private string connectionString = @"Data Source=DESKTOP-S44CFG2\SQLSERVER2017;Initial Catalog=HRMS;User ID=sa;Password=sql@123;";
        public DBConnector(string inlineQuery, bool isSP)
        {
            this.sqlConnection = new SqlConnection(this.connectionString);
            this.sqlCommand = new SqlCommand(inlineQuery, this.sqlConnection)
            {
                CommandType = isSP ? CommandType.StoredProcedure : CommandType.Text
            };
            sqlCommand.Connection.Open();
        }

        public void AddInParameterWithValue(string name, object value)
        {
            SqlParameter param = new SqlParameter(name, value)
            {
                Direction = ParameterDirection.Input
            };
            sqlCommand.Parameters.Add(param);
        }

        //public void AddInTableParameterWithValue(string name, object value)
        //{
        //    MySqlParameter param = new MySqlParameter(name, value)
        //    {
        //        Direction = ParameterDirection.Input
        //    };
        //    param.MySqlDbType = SqlDbType.Structured;
        //    sqlCommand.Parameters.Add(param);
        //}


        public void AddOutParameterWithType(string name, SqlDbType dbType)
        {
            SqlParameter param = new SqlParameter(name, dbType)
            {

                Direction = ParameterDirection.Output
            };
            if (name == "@Message")
            {
                param.Size = 100;
            }
            sqlCommand.Parameters.Add(param);
        }

        public void AddInOutParameterWithValue(string name, object value)
        {
            SqlParameter param = new SqlParameter(name, value)
            {
                Direction = ParameterDirection.InputOutput
            };
            sqlCommand.Parameters.Add(param);
        }

        public void AddReturnParameterWithValue(string name, object value)
        {
            SqlParameter param = new SqlParameter(name, value)
            {
                Direction = ParameterDirection.ReturnValue
            };
            sqlCommand.Parameters.Add(param);
        }

        public void AddInParameter(SqlParameter parameter)
        {
            sqlCommand.Parameters.Add(parameter);
        }
        public void AddInParameter(string name, SqlDbType dbType, int size, string sourceColumn)
        {
            sqlCommand.Parameters.Add(name, dbType, size, sourceColumn);
        }
        public void AddOutParameter(string name, SqlDbType dbType, int size)
        {
            SqlParameter param = new SqlParameter(name, dbType, size)
            {
                Direction = ParameterDirection.ReturnValue
            };
            sqlCommand.Parameters.Add(param);
        }

        public object GetParamaeterValue(string parameter)
        {
            return sqlCommand.Parameters[parameter].Value ?? 0;
        }

        public int ExceuteNonQuery()
        {
            return sqlCommand.ExecuteNonQuery();
        }

        public SqlDataReader ExecuteReader()
        {
            return sqlCommand.ExecuteReader();
        }

        public object ExecuteScalar()
        {
            return sqlCommand.ExecuteScalar();
        }

        public DataTable GetDataTable()
        {
            DataSet ds = new DataSet();
            using (var adapter = new SqlDataAdapter(sqlCommand))
            {
                adapter.Fill(ds);
            }

            return ds != null && ds.Tables.Count > 0 ? ds.Tables[0] : null;
        }

        public DataSet GetDataSet()
        {
            DataSet ds = new DataSet();
            using (var adapter = new SqlDataAdapter(sqlCommand))
            {
                adapter.Fill(ds);
            }

            return ds;
        }




        /// <summary>
        /// Destructor of Common Execute query class.
        /// </summary>
        ~DBConnector()
        {
            this.Dispose(false);
        }


        /// <summary>
        /// Dispose the resource which is allocated for this object.
        /// </summary>
        /// <param name="disposing"></param>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Release the all allocated resource.
        /// </summary>
        /// <param name="disposing">Flg for object is already dispose by system</param>
        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed) return;

            if (disposing)
            {
                if (this.sqlConnection != null)
                {
                    this.sqlConnection.Close();
                    this.sqlConnection.Dispose();
                    this.sqlConnection = null;
                }

                if (this.sqlCommand != null)
                {
                    this.sqlCommand.Dispose();
                    this.sqlCommand = null;
                }
                ////Clean all memeber and release resource.
            }

            // Free any unmanaged objects here.
            disposed = true;
        }
    }
}
