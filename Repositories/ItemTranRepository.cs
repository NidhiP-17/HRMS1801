using DataAccess;
using Models.ResponseModel;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace Repositories
{
    public class ItemTranRepository
    {
        /// <summary>
        /// Flag: Has Dispose already been called
        /// </summary>
        bool disposed;
        private ItemTranDataAccess instance;
        #region "=================== Constructor =============================="
        public ItemTranRepository()
        {
            this.instance = new ItemTranDataAccess();
        }

        ~ItemTranRepository()
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
        public ResponseSingleModel<string> Create(int itemId, decimal amount, int quantity, long userId)
        {
            var result = new ResponseSingleModel<string>();
            var message = string.Empty;
            result.Response = instance.CreateItemTran(itemId,amount, quantity, out message, userId);
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
        public ResponseCollectionModel<ItemModel> GetPlaceOrder(long empId)
        {
            var result = new ResponseCollectionModel<ItemModel>();
            var dt = instance.GetPlaceOrder(empId);
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
        //public ResponseSingleModel<ItemModel> GetYear()
        //{
        //    var result = new ResponseSingleModel<ItemModel>();
        //    var dt = instance.GetYear();
        //    var lst = DataAccessUtility.ConvertToList<ItemModel>(dt);
        //    ItemModel detail = lst.Count > 0 ? lst[0] : null;
        //    result.Response = detail;
        //    result.Status = detail != null ? Constants.WebApiStatusOk : Constants.WebApiStatusFail;
        //    result.Message = "Ok";
        //    return result;
        //}
        public DataTable GetYear()
        {
            DataTable dt = null;
            using (DBConnector connector = new DBConnector("sp_GetYear", true))
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
