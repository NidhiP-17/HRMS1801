using Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class ItemCategoryDataAccess : IDisposable
    {
        /// <summary>
        /// Flag: Has Dispose already been called
        /// </summary>
        bool disposed;
        #region "=================== Constructor =============================="
        public ItemCategoryDataAccess()
        {
        }

        ~ItemCategoryDataAccess()
        {
            this.Dispose(false);
        }
        #endregion

        public DataTable ListCategories(long userId, out string message)
        {
            DataTable dt = null;
            using (DBConnector connector = new DBConnector("sp_GetCategories", true))
            {
                //connector.AddInParameterWithValue("@userId", userId);
                dt = connector.GetDataTable();
                message = "ok";
            }
            return dt;
        }

        public DataTable GetCategories(long userId, out string message)
        {
            DataTable dt = null;
            using (DBConnector connector = new DBConnector("sp_GetAllCategories", true))
            {
                //connector.AddInParameterWithValue("@userId", userId);
                dt = connector.GetDataTable();
                message = "ok";
            }
            return dt;
        }

        public string CreateItemCategory(ItemCategoryModel item, out string message, long userId)
        {
            int rowEffected = 0;
            using (DBConnector connector = new DBConnector("sp_CreateItemCategory", true))
            {
                connector.AddInParameterWithValue("@itemCatName", item.ItemCatName);
                connector.AddInParameterWithValue("@isActive", item.IsActive);
                connector.AddInParameterWithValue("@createdBy", userId);
                connector.AddInParameterWithValue("@createdDate", DateTime.Now);
                connector.AddOutParameterWithType("@Message", SqlDbType.VarChar);
                rowEffected = connector.ExceuteNonQuery();
                message = connector.GetParamaeterValue("@Message").ToString();
            }

            return message;
        }

        public string UpdateItemCategory(ItemCategoryModel item, out string message, long userId)
        {
            int rowEffected = 0;
            using (DBConnector connector = new DBConnector("sp_UpdateItemCategory", true))
            {
                connector.AddInParameterWithValue("@itemCatId", item.ItemCatID);
                connector.AddInParameterWithValue("@itemCatName", item.ItemCatName);
                connector.AddInParameterWithValue("@isActive", item.IsActive);
                connector.AddInParameterWithValue("@updatedBy", userId);
                connector.AddInParameterWithValue("@updatedDate", DateTime.Now);
                connector.AddOutParameterWithType("@Message", SqlDbType.VarChar);
                rowEffected = connector.ExceuteNonQuery();
                message = connector.GetParamaeterValue("@Message").ToString();
            }

            return message;
        }
        public string DeleteItemCategory(int itemCatId, out string message, long userId)
        {
            int rowEffected = 0;
            using (DBConnector connector = new DBConnector("sp_DeleteItemCategory", true))
            {
                connector.AddInParameterWithValue("@itemCatId", itemCatId);
                connector.AddInParameterWithValue("@deletedBy", userId);
                connector.AddInParameterWithValue("@deletedDate", DateTime.Now);
                connector.AddOutParameterWithType("@Message", SqlDbType.VarChar);
                rowEffected = connector.ExceuteNonQuery();
                message = connector.GetParamaeterValue("@Message").ToString();
            }

            return message;
        }
        public DataTable GetItemCategoryById(int itemId)
        {
            DataTable dt = null;
            using (DBConnector connector = new DBConnector("sp_GetItemCategoryById", true))
            {
                connector.AddInParameterWithValue("@itemCatId", itemId);
                dt = connector.GetDataTable();
            }
            return dt;
        }
        public string UpdateStatus(int id, bool IsActive, out string message, long userId)
        {
            int rowEffected = 0;
            using (DBConnector connector = new DBConnector("sp_UpdateItemCategoryByStatus", true))
            {
                connector.AddInParameterWithValue("@itemCategoryId", id);
                connector.AddInParameterWithValue("@isActive", IsActive);
                connector.AddInParameterWithValue("@updatedBy", userId);
                connector.AddInParameterWithValue("@updatedDate", DateTime.Now);
                connector.AddOutParameterWithType("@Message", SqlDbType.VarChar);
                rowEffected = connector.ExceuteNonQuery();
                message = connector.GetParamaeterValue("@Message").ToString();
            }

            return message;
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
