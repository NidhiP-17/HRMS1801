using Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class LaunchDateDataAccess : IDisposable
    {
        /// <summary>
        /// Flag: Has Dispose already been called
        /// </summary>
        bool disposed;
        #region "=================== Constructor =============================="
        public LaunchDateDataAccess()
        {
        }

        ~LaunchDateDataAccess()
        {
            this.Dispose(false);
        }
        #endregion

        public string CreateLaunchDate(LaunchDateModel item, out string message, long userId)
        {
            int rowEffected = 0;
            using (DBConnector connector = new DBConnector("sp_CreateLaunchDate", true))
            {
                connector.AddInParameterWithValue("@launchDate", item.launchdate);
                connector.AddOutParameterWithType("@Message", SqlDbType.VarChar);
                rowEffected = connector.ExceuteNonQuery();
                message = connector.GetParamaeterValue("@Message").ToString();
            }

            return message;
        }

        public DataTable GetLaunchDate()
        {
            DataTable dt = null;
            using (DBConnector connector = new DBConnector("sp_GetLaunchDate", true))
            {
                dt = connector.GetDataTable();
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
