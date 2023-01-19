using DataAccess;
using Models.ResponseModel;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class LaunchDateRepository : IDisposable
    {
        /// <summary>
        /// Flag: Has Dispose already been called
        /// </summary>
        bool disposed;
        private LaunchDateDataAccess instance;
        #region "=================== Constructor =============================="
        public LaunchDateRepository()
        {
            this.instance = new LaunchDateDataAccess();
        }

        ~LaunchDateRepository()
        {
            this.Dispose(false);
        }
        #endregion

        public ResponseSingleModel<string> Create(LaunchDateModel project, long userId)
        {
            var result = new ResponseSingleModel<string>();
            var message = string.Empty;
            result.Response = instance.CreateLaunchDate(project, out message, userId);
            //result.Status = result.Response ? Constants.WebApiStatusOk : Constants.WebApiStatusFail;
            result.Message = message;
            return result;
        }
        public ResponseSingleModel<LaunchDateModel> GetLaunchDate()
        {
            var result = new ResponseSingleModel<LaunchDateModel>();
            var dt = instance.GetLaunchDate();
            var lst = DataAccessUtility.ConvertToList<LaunchDateModel>(dt);
            LaunchDateModel detail = lst.Count > 0 ? lst[0] : null;
            result.Response = detail;
            result.Status = detail != null ? Constants.WebApiStatusOk : Constants.WebApiStatusFail;
            result.Message = "Ok";
            return result;
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
