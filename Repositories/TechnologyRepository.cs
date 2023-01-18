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
    public class TechnologyRepository
    {
        /// <summary>
        /// Flag: Has Dispose already been called
        /// </summary>
        bool disposed;
        private TechnologyDataAccess instance;
        #region "=================== Constructor =============================="
        public TechnologyRepository()
        {
            this.instance = new TechnologyDataAccess();
        }

        ~TechnologyRepository()
        {
            this.Dispose(false);
        }
        #endregion

        public ResponseSingleModel<string> CreateTechnology(string item, long userId)
        {
            var result = new ResponseSingleModel<string>();
            var message = string.Empty;
            result.Response = instance.CreateTechnology(item, out message, userId);
            result.Message = message;
            return result;
        }

        public ResponseCollectionModel<TechnologyModel> GetTechnologyList(long userId, out string message)
        {
            var result = new ResponseCollectionModel<TechnologyModel>();
            var dt = instance.ListTechnologies(userId, out message);
            var lst = DataAccessUtility.ConvertToList<TechnologyModel>(dt);
            result.Response = lst;
            result.Status = Constants.WebApiStatusOk;
            result.Message = "";
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
