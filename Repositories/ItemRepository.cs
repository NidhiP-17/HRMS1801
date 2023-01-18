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
    public class ItemRepository
    {
        /// <summary>
        /// Flag: Has Dispose already been called
        /// </summary>
        bool disposed;
        private ItemDataAccess instance;
        #region "=================== Constructor =============================="
        public ItemRepository()
        {
            this.instance = new ItemDataAccess();
        }

        ~ItemRepository()
        {
            this.Dispose(false);
        }
        #endregion
        public ResponseCollectionModel<ItemModel> GetList(long userId, out string message)
        {
            var result = new ResponseCollectionModel<ItemModel>();
            var dt = instance.GetItems(userId, out message);
            var lst = DataAccessUtility.ConvertToList<ItemModel>(dt);
            result.Response = lst;
            result.Status = Constants.WebApiStatusOk;
            result.Message = "";
            return result;
        }
        public ResponseSingleModel<string> Create(ItemModel item, long userId)
        {
            var result = new ResponseSingleModel<string>();
            var message = string.Empty;
            result.Response = instance.CreateItem(item, out message, userId);
            //result.Status = result.Response ? Constants.WebApiStatusOk : Constants.WebApiStatusFail;
            result.Message = message;
            return result;
        }
        public ResponseSingleModel<string> Update(ItemModel project, long userId)
        {
            var result = new ResponseSingleModel<string>();
            var message = string.Empty;
            result.Response = instance.UpdateItem(project, out message, userId);
            //result.Status = result.Response ? Constants.WebApiStatusOk : Constants.WebApiStatusFail;
            result.Message = message;
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

        public ResponseSingleModel<string> Delete(int id, long userId)
        {
            var result = new ResponseSingleModel<string>();
            var message = string.Empty;
            result.Response = instance.DeleteItem(id, out message, userId);
            //result.Status = result.Response ? Constants.WebApiStatusOk : Constants.WebApiStatusFail;
            result.Message = message;
            return result;
        }
        public ResponseCollectionModel<ItemModel> GetItemByCategoryId(int categoryId)
        {
            var result = new ResponseCollectionModel<ItemModel>();
            var dt = instance.GetItemByCategoryId(categoryId);
            var lst = DataAccessUtility.ConvertToList<ItemModel>(dt);
            result.Response = lst;
            result.Status = Constants.WebApiStatusOk;
            result.Message = "OK";
            return result;
        }
        public ResponseSingleModel<ItemModel> GetItemById(int itemId)
        {
            var result = new ResponseSingleModel<ItemModel>();
            var dt = instance.GetItemById(itemId);
            var lst = DataAccessUtility.ConvertToList<ItemModel>(dt);
            ItemModel detail = lst.Count > 0 ? lst[0] : null;
            result.Response = detail;
            result.Status = detail != null ? Constants.WebApiStatusOk : Constants.WebApiStatusFail;
            result.Message = "Ok";
            return result;
        }
        public ResponseSingleModel<string> CreateItemTran(ItemModel item, long userId)
        {
            var result = new ResponseSingleModel<string>();
            var message = string.Empty;
            result.Response = instance.CreateItem(item, out message, userId);
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
