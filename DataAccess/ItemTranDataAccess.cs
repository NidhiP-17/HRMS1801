using Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class ItemTranDataAccess : IDisposable
    {
        /// <summary>
        /// Flag: Has Dispose already been called
        /// </summary>
        bool disposed;
        #region "=================== Constructor =============================="
        public ItemTranDataAccess()
        {
        }

        ~ItemTranDataAccess()
        {
            this.Dispose(false);
        }
        #endregion
        public DataTable GetItems(long userId, out string message)
        {
            DataTable dt = null;
            using (DBConnector connector = new DBConnector("sp_GetAllItems", true))
            {
                connector.AddInParameterWithValue("@userId", userId);
                dt = connector.GetDataTable();
                message = "ok";
            }
            return dt;
        }
        public string CreateItemTran(int itemId, decimal amount, int quantity, out string message, long userId)
        {
            int rowEffected = 0;
            using (DBConnector connector = new DBConnector("sp_CreateItemTran", true))
            {
                connector.AddInParameterWithValue("@empId", userId);
                connector.AddInParameterWithValue("@itemId", itemId);
                connector.AddInParameterWithValue("@todayDate", DateTime.Now);
                connector.AddInParameterWithValue("@amount", amount);
                connector.AddInParameterWithValue("@quantity", quantity);
                connector.AddOutParameterWithType("@Message", SqlDbType.VarChar);
                rowEffected = connector.ExceuteNonQuery();
                message = connector.GetParamaeterValue("@Message").ToString();
            }

            return message;
        }

        public string UpdateItem(ItemModel item, out string message, long userId)
        {
            int rowEffected = 0;
            using (DBConnector connector = new DBConnector("sp_UpdateItem", true))
            {
                connector.AddInParameterWithValue("@itemId", item.ItemId);
                connector.AddInParameterWithValue("@itemName", item.ItemName);
                connector.AddInParameterWithValue("@itemDesc", item.ItemDesc);
                connector.AddInParameterWithValue("@amount", item.Amount);
                connector.AddInParameterWithValue("@itemCatID", item.ItemCatID);
                connector.AddInParameterWithValue("@isActive", item.IsActive);
                connector.AddInParameterWithValue("@imageUrl", item.ImageName);
                connector.AddInParameterWithValue("@updatedBy", userId);
                connector.AddInParameterWithValue("@updatedDate", DateTime.Now);
                connector.AddOutParameterWithType("@Message", SqlDbType.VarChar);
                rowEffected = connector.ExceuteNonQuery();
                message = connector.GetParamaeterValue("@Message").ToString();
            }

            return message;
        }
        public string DeleteItem(int itemId, out string message, long userId)
        {
            int rowEffected = 0;
            using (DBConnector connector = new DBConnector("sp_DeleteItem", true))
            {
                connector.AddInParameterWithValue("@itemId", itemId);
                connector.AddInParameterWithValue("@deletedBy", userId);
                connector.AddInParameterWithValue("@deletedDate", DateTime.Now);
                connector.AddOutParameterWithType("@Message", SqlDbType.VarChar);
                rowEffected = connector.ExceuteNonQuery();
                message = connector.GetParamaeterValue("@Message").ToString();
            }

            return message;
        }

        public DataTable GetItemByCategoryId(int categoryId)
        {
            DataTable dt = null;
            using (DBConnector connector = new DBConnector("sp_GetItemByCategoryId", true))
            {
                connector.AddInParameterWithValue("@itemCatId", categoryId);
                dt = connector.GetDataTable();
            }
            return dt;
        }

        public DataTable GetItemById(int itemId)
        {
            DataTable dt = null;
            using (DBConnector connector = new DBConnector("sp_GetItemById", true))
            {
                connector.AddInParameterWithValue("@itemId", itemId);
                dt = connector.GetDataTable();
            }
            return dt;
        }

        public DataTable GetYear()
        {
            DataTable dt = null;
            using (DBConnector connector = new DBConnector("sp_GetYear", true))
            {
                dt = connector.GetDataTable();
            }
            return dt;
        }

        public DataTable GetPlaceOrder(long empId)
        {
            DataTable dt = null;
            using (DBConnector connector = new DBConnector("sp_GetPlacedOrder", true))
            {
                connector.AddInParameterWithValue("@empId", empId);
                connector.AddInParameterWithValue("@todayDate", Convert.ToDateTime(DateTime.Now).ToString("yyyy/MM/dd"));
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
