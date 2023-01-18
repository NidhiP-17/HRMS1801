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
    public class ItemCategoryRepository : IDisposable
    {
        /// <summary>
        /// Flag: Has Dispose already been called
        /// </summary>
        bool disposed;
        private ItemCategoryDataAccess instance;
        #region "=================== Constructor =============================="
        public ItemCategoryRepository()
        {
            this.instance = new ItemCategoryDataAccess();
        }

        ~ItemCategoryRepository()
        {
            this.Dispose(false);
        }
        #endregion
        public ResponseCollectionModel<ItemCategoryModel> GetList(long userId, out string message)
        {
            var result = new ResponseCollectionModel<ItemCategoryModel>();
            var dt = instance.ListCategories(userId, out message);
            var lst = DataAccessUtility.ConvertToList<ItemCategoryModel>(dt);
            result.Response = lst;
            result.Status = Constants.WebApiStatusOk;
            result.Message = "";
            return result;
        }
        public ResponseCollectionModel<ItemCategoryModel> GetCategories(long userId, out string message)
        {
            var result = new ResponseCollectionModel<ItemCategoryModel>();
            var dt = instance.GetCategories(userId, out message);
            var lst = DataAccessUtility.ConvertToList<ItemCategoryModel>(dt);
            result.Response = lst;
            result.Status = Constants.WebApiStatusOk;
            result.Message = "";
            return result;
        }
        public ResponseSingleModel<string> Create(ItemCategoryModel item, long userId)
        {
            var result = new ResponseSingleModel<string>();
            var message = string.Empty;
            result.Response = instance.CreateItemCategory(item, out message, userId);
            //result.Status = result.Response ? Constants.WebApiStatusOk : Constants.WebApiStatusFail;
            result.Message = message;
            return result;
        }
        public ResponseSingleModel<string> Update(ItemCategoryModel project, long userId)
        {
            var result = new ResponseSingleModel<string>();
            var message = string.Empty;
            result.Response = instance.UpdateItemCategory(project, out message, userId);
            //result.Status = result.Response ? Constants.WebApiStatusOk : Constants.WebApiStatusFail;
            result.Message = message;
            return result;
        }
        public ResponseSingleModel<string> Delete(int id, long userId)
        {
            var result = new ResponseSingleModel<string>();
            var message = string.Empty;
            result.Response = instance.DeleteItemCategory(id, out message, userId);
            //result.Status = result.Response ? Constants.WebApiStatusOk : Constants.WebApiStatusFail;
            result.Message = message;
            return result;
        }
        public ResponseSingleModel<ItemCategoryModel> GetCategoryById(int itemcatId)
        {
            var result = new ResponseSingleModel<ItemCategoryModel>();
            var dt = instance.GetItemCategoryById(itemcatId);
            var lst = DataAccessUtility.ConvertToList<ItemCategoryModel>(dt);
            ItemCategoryModel detail = lst.Count > 0 ? lst[0] : null;
            result.Response = detail;
            result.Status = detail != null ? Constants.WebApiStatusOk : Constants.WebApiStatusFail;
            result.Message = "Ok";
            return result;
        }

        public ResponseSingleModel<string> UpdateStatus(int id, bool isActive, long userId)
        {
            var result = new ResponseSingleModel<string>();
            var message = string.Empty;
            result.Response = instance.UpdateStatus(id, isActive, out message, userId);
            //result.Status = result.Response ? Constants.WebApiStatusOk : Constants.WebApiStatusFail;
            result.Message = message;
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
