using Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class TechnologyDataAccess : IDisposable
    {
        /// <summary>
        /// Flag: Has Dispose already been called
        /// </summary>
        bool disposed;
        #region "=================== Constructor =============================="
        public TechnologyDataAccess()
        {
        }

        ~TechnologyDataAccess()
        {
            this.Dispose(false);
        }
        #endregion

        public string CreateTechnology(string item, out string message, long userId)
        {
            int rowEffected = 0;
            using (DBConnector connector = new DBConnector("sp_CreateTechnology", true))
            {
                connector.AddInParameterWithValue("@name", item);
                connector.AddInParameterWithValue("@createdBy", userId);
                connector.AddInParameterWithValue("@createdDate", System.DateTime.UtcNow);
                connector.AddOutParameterWithType("@Message", SqlDbType.VarChar);
                rowEffected = connector.ExceuteNonQuery();
                message = connector.GetParamaeterValue("@Message").ToString();
            }

            return message;
        }
        public DataTable ListTechnologies(long userId, out string message)
        {
            DataTable dt = null;
            using (DBConnector connector = new DBConnector("sp_ListTechnology", true))
            {
                dt = connector.GetDataTable();
                message = "ok";
            }
            return dt;
        }

        #region ========================= Dispose Method ==============
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed) return;

            if (disposing)
            {

                ////TODO: Clean all memeber and release resource.
            }

            // Free any unmanaged objects here.
            disposed = true;
        }

        #endregion
    }
}
