using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using HRMS.Classes;

namespace HRMS.Models
{
    public class ItemCategory
    {
        private string mstrModule = "Model=>ItemCategory";
        [Key]
        public int ItemCatID { get; set; }

        [Required(ErrorMessage = "Category name is required")]
        [DisplayName("Category Name")]
        public string? ItemCatName { get; set; }
        [DisplayName("Active")]
        public bool IsActive { get; set; }
        public List<ItemCategory>? getAllCategories(string? strSearch, string strConnections)
        {
            string strQry;
            SqlCommand? sqlComm;
            List<ItemCategory>? listItemCategory = null;
            try
            {
                listItemCategory = new List<ItemCategory>();

                if (string.IsNullOrEmpty(strSearch))
                {
                    strQry = "SELECT ItemCatID, ItemCatName, IsActive FROM ItemCategory";

                }
                else
                {
                    strQry = "SELECT ItemCatID, ItemCatName, IsActive FROM ItemCategory WHERE ItemCatName LIKE'%" + strSearch + "%'";
                }

                using (SqlConnection SqlConn = new SqlConnection(strConnections))
                {
                    SqlConn.Open();
                    sqlComm = new SqlCommand(strQry, SqlConn);

                    using (SqlDataReader reader = sqlComm.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ItemCategory item = new ItemCategory();
                            item.ItemCatID = reader.GetInt32("ItemCatID");
                            item.ItemCatName = reader.GetString("ItemCatName");
                            item.IsActive = reader.GetBoolean("IsActive");
                            listItemCategory.Add(item);
                        }
                        reader.Close();
                    }
                    SqlConn.Close();
                }
            }
            catch (SqlException SqlEx)
            {
                Global.gintErrorNo = 100;
                Global.gstrErrorDesc = mstrModule + "=>getAllCategories=>" + SqlEx.Message;
            }
            catch (Exception ex)
            {
                Global.gintErrorNo = 101;
                Global.gstrErrorDesc = mstrModule + "=>getAllCategories=>" + ex.Message;
            }
            finally
            {
                sqlComm = null;
            }
            return listItemCategory;
        }
        public void GetItemCategory(int? id, string strConnections)
        {
            SqlCommand? sqlComm;
            SqlDataReader? sqlDataReader;
            try
            {
                using (SqlConnection SqlConn = new SqlConnection(strConnections))
                {
                    SqlConn.Open();
                    sqlComm = new SqlCommand("SELECT ItemCatID, ItemCatName, IsActive FROM ItemCategory WHERE ItemCatID = '" + id + "'", SqlConn);
                    sqlDataReader = sqlComm.ExecuteReader();

                    if (sqlDataReader.Read())
                    {
                        ItemCatID = Convert.ToInt16(sqlDataReader["ItemCatID"]);
                        ItemCatName = sqlDataReader["ItemCatName"].ToString();
                        IsActive = Convert.ToBoolean(sqlDataReader["IsActive"]);

                    }
                    SqlConn.Close();
                    SqlConn.Dispose();
                }
            }
            catch (SqlException SqlEx)
            {
                Global.gintErrorNo = 100;
                Global.gstrErrorDesc = mstrModule + "=>GetItemCategory=>" + SqlEx.Message;
            }
            catch (Exception ex)
            {
                Global.gintErrorNo = 101;
                Global.gstrErrorDesc = mstrModule + "=>GetItemCategory=>" + ex.Message;
            }
            finally
            {
                sqlComm = null;
                sqlDataReader = null;
            }
        }
        public void DeleteItemCategory(int? id, string strConnections)
        {
            SqlCommand? sqlComm;
            try
            {
                using (SqlConnection SqlConn = new SqlConnection(strConnections))
                {
                    SqlConn.Open();
                    sqlComm = new SqlCommand("DELETE FROM ItemCategory WHERE ItemCatID = '" + id + "'", SqlConn);
                    sqlComm.ExecuteNonQuery();
                    SqlConn.Close();
                }

            }
            catch (SqlException SqlEx)
            {
                Global.gintErrorNo = 100;
                Global.gstrErrorDesc = mstrModule + "=>DeleteItemCategory=>" + SqlEx.Message;
            }
            catch (Exception ex)
            {
                Global.gintErrorNo = 101;
                Global.gstrErrorDesc = mstrModule + "=>DeleteItemCategory=>" + ex.Message;
            }
            finally
            {
                sqlComm = null;
            }
        }
        public void AddEditItemCategory(int Id, ItemCategory itemCategory, string strConnections)
        {
            SqlCommand? sqlComm;
            try
            {
                using (SqlConnection SqlConn = new SqlConnection(strConnections))
                {
                    SqlConn.Open();

                    if (Id > 0)
                    {
                        sqlComm = new SqlCommand("EditItemCategory", SqlConn);
                    }
                    else
                    {
                        sqlComm = new SqlCommand("AddItemCategory", SqlConn);
                    }
                    sqlComm.Parameters.Clear();

                    sqlComm.CommandType = CommandType.StoredProcedure;

                    if (Id > 0)
                    {
                        sqlComm.Parameters.AddWithValue("@ItemCatID", Id);
                    }
                    sqlComm.Parameters.AddWithValue("@ItemCatName", itemCategory.ItemCatName);
                    sqlComm.Parameters.AddWithValue("@IsActive", itemCategory.IsActive);
                    sqlComm.ExecuteNonQuery();
                    SqlConn.Close();

                }

            }
            catch (SqlException SqlEx)
            {
                Global.gintErrorNo = 100;
                Global.gstrErrorDesc = mstrModule + "=>AddEditItemCategory=>" + SqlEx.Message;
            }
            catch (Exception ex)
            {
                Global.gintErrorNo = 101;
                Global.gstrErrorDesc = mstrModule + "=>AddEditItemCategory=>" + ex.Message;
            }
            finally
            {
                sqlComm = null;
            }

        }
    }
}
