using HRMS.Classes;
using Microsoft.AspNetCore.Mvc.Rendering;
using Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Data;

namespace HRMS.Models
{
    public class Item
    {
        public string mstrSource = "";
        private string mstrModule = "Model=>Item";

        [Key]
        public int ItemId { get; set; }

        [Required(ErrorMessage = "Item name is required")]
        [DisplayName("Name")]
        public string? ItemName { get; set; }

        [DisplayName("Description")]
        public string? ItemDesc { get; set; }

        public int ItemCatID { get; set; }

        [DisplayName("Category")]
        public string? ItemCatName { get; set; }
        public int Quantity { get; set; }
        public List<ItemCategory>? ItemCategory { get; set; }

        [DisplayName("Active")]
        public bool IsActive { get; set; }
        public decimal Amount { get; set; }
        public decimal Rate { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DisplayName("Date")]
        public DateTime? ReportDate { get; set; }

        [Key]
        public int ImageId { get; set; }
        public string Title { get; set; }

        [DisplayName("Image Name")]
        public string? ImageName { get; set; }

        [DisplayName("Upload Image")]
        public IFormFile ImageFile { get; set; }
        public List<Item>? getAllItems(string? strSearch, string strConnections)
        {
            string strQry;
            SqlCommand? sqlComm;
            List<Item>? listItem = null;
            try
            {
                listItem = new List<Item>();

                if (string.IsNullOrEmpty(strSearch))
                {
                    strQry = "SELECT ItemId,ItemName, ItemDesc, tbl_ItemCategory.ItemCatID AS ItemCatID, ItemCatName, Amount, " +
                                        "tbl_ItemMaster.IsActive AS IsActive, ISNULL(imageUrl,'') AS imageUrl " +
                                "FROM tbl_ItemMaster, tbl_ItemCategory " +
                                "WHERE tbl_ItemMaster.ItemCatID = tbl_ItemCategory.ItemCatID AND tbl_ItemMaster.IsActive=1";
                }
                else
                {
                    strQry = "SELECT ItemId, ItemName, ItemDesc, tbl_ItemCategory.ItemCatID AS ItemCatID, ItemCatName, Amount, " +
                                       "tbl_ItemMaster.IsActive AS IsActive, ISNULL(imageUrl,'') AS imageUrl " +
                               "FROM tbl_ItemMaster, tbl_ItemCategory " +
                               "WHERE tbl_ItemMaster.ItemCatID = tbl_ItemCategory.ItemCatID " +
                               "AND  tbl_ItemMaster.IsActive=1 AND ItemName LIKE '%" + strSearch + "%' ";
                }

                //if (mstrSource == "ItemOrder")
                //{
                //    strQry = strQry + "AND tbl_ItemMaster.IsActive = 1 AND tbl_ItemCategory.IsActive = 1 ";
                //}

                using (SqlConnection SqlConn = new SqlConnection(strConnections))
                {
                    SqlConn.Open();
                    sqlComm = new SqlCommand(strQry, SqlConn);

                    using (SqlDataReader reader = sqlComm.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Item item = new Item();
                            item.ItemId = reader.GetInt32("ItemId");
                            item.ItemName = reader.GetString("ItemName");
                            item.ItemDesc = reader.GetString("ItemDesc");
                            item.ItemCatID = reader.GetInt32("ItemCatID");
                            item.ItemCatName = reader.GetString("ItemCatName");
                            item.IsActive = reader.GetBoolean("IsActive");
                            item.Amount = reader.GetDecimal("Amount");
                            item.ImageName = reader.GetString("imageUrl");
                            listItem.Add(item);
                        }
                        reader.Close();
                    }
                    SqlConn.Close();
                }
            }
            catch (SqlException SqlEx)
            {
                Global.gintErrorNo = 100;
                Global.gstrErrorDesc = mstrModule + "=>getAllItems=>" + SqlEx.Message;
            }
            catch (Exception ex)
            {
                Global.gintErrorNo = 101;
                Global.gstrErrorDesc = mstrModule + "=>getAllItems=>" + ex.Message;
            }
            finally
            {
                sqlComm = null;
            }
            return listItem;
        }
        public void GetItem(int? id, string strConnections)
        {
            SqlCommand? sqlComm;
            SqlDataReader? sqlDataReader;
            ItemCategory? ObjitemCategory = null;
            string strQry = "";
            try
            {
                ObjitemCategory = new ItemCategory();
                ItemCategory = ObjitemCategory.getAllCategories("", strConnections);

            }
            catch (Exception ex)
            {
                Global.gintErrorNo = 101;
                Global.gstrErrorDesc = mstrModule + "=>GetItem=>" + ex.Message;
            }
            try
            {
                using (SqlConnection SqlConn = new SqlConnection(strConnections))
                {
                    SqlConn.Open();
                    strQry = "SELECT ItemId, ItemName, ItemDesc, tbl_ItemCategory.ItemCatID AS ItemCatID, ItemCatName, Amount, " +
                                       "tbl_ItemMaster.IsActive AS IsActive, ISNULL(imageUrl,'') AS imageUrl " +
                               "FROM tbl_ItemMaster, tbl_ItemCategory " +
                               "WHERE tbl_ItemMaster.ItemCatID = tbl_ItemCategory.ItemCatID " +
                               "AND ItemId = '" + id + "'";

                    sqlComm = new SqlCommand(strQry, SqlConn);
                    sqlDataReader = sqlComm.ExecuteReader();

                    if (sqlDataReader.Read())
                    {
                        ItemId = Convert.ToInt16(sqlDataReader["ItemId"]);
                        ItemName = sqlDataReader["ItemName"].ToString();
                        ItemDesc = sqlDataReader["ItemDesc"].ToString();
                        ItemCatID = Convert.ToInt16(sqlDataReader["ItemCatID"]);
                        ItemCatName = sqlDataReader["ItemCatName"].ToString();
                        IsActive = Convert.ToBoolean(sqlDataReader["IsActive"]);
                        Amount = Convert.ToDecimal(sqlDataReader["Amount"]);
                        ImageName = sqlDataReader["imageUrl"].ToString();
                    }
                    SqlConn.Close();
                    SqlConn.Dispose();
                }
            }
            catch (SqlException SqlEx)
            {
                Global.gintErrorNo = 100;
                Global.gstrErrorDesc = mstrModule + "=>GetItem=>" + SqlEx.Message;
            }
            catch (Exception ex)
            {
                Global.gintErrorNo = 101;
                Global.gstrErrorDesc = mstrModule + "=>GetItem=>" + ex.Message;
            }
            finally
            {
                sqlComm = null;
                sqlDataReader = null;
            }
        }
        public void AddEditItem(int Id, Item item, string fileName, string strConnections)
        {

            SqlCommand? sqlComm;
            try
            {
                using (SqlConnection SqlConn = new SqlConnection(strConnections))
                {
                    SqlConn.Open();

                    if (Id > 0)
                    {
                        sqlComm = new SqlCommand("EditItem", SqlConn);
                    }
                    else
                    {
                        sqlComm = new SqlCommand("AddItem", SqlConn);
                    }
                    sqlComm.Parameters.Clear();

                    sqlComm.CommandType = CommandType.StoredProcedure;

                    if (Id > 0)
                    {
                        sqlComm.Parameters.AddWithValue("@ItemID", Id);
                    }
                    sqlComm.Parameters.AddWithValue("@ItemName", item.ItemName);
                    if (String.IsNullOrEmpty(item.ItemDesc))
                    {
                        sqlComm.Parameters.AddWithValue("@ItemDesc", "");
                    }
                    else
                    {
                        sqlComm.Parameters.AddWithValue("@ItemDesc", item.ItemDesc);
                    }
                    sqlComm.Parameters.AddWithValue("@Amount", item.Amount);
                    sqlComm.Parameters.AddWithValue("@ItemCatID", item.ItemCatID);
                    sqlComm.Parameters.AddWithValue("@IsActive", item.IsActive);
                    sqlComm.Parameters.AddWithValue("@imageUrl", fileName);
                    sqlComm.ExecuteNonQuery();
                    SqlConn.Close();

                }

            }
            catch (SqlException SqlEx)
            {
                Global.gintErrorNo = 100;
                Global.gstrErrorDesc = mstrModule + "=>AddEditItem=>" + SqlEx.Message;
            }
            catch (Exception ex)
            {
                Global.gintErrorNo = 101;
                Global.gstrErrorDesc = mstrModule + "=>AddEditItem=>" + ex.Message;
            }
            finally
            {
                sqlComm = null;
            }

        }
        public void DeleteItem(int? id, string strConnections)
        {
            SqlCommand? sqlComm;
            try
            {
                using (SqlConnection SqlConn = new SqlConnection(strConnections))
                {
                    SqlConn.Open();
                    sqlComm = new SqlCommand("DELETE FROM tbl_ItemMaster WHERE ItemId = '" + id + "'", SqlConn);
                    sqlComm.ExecuteNonQuery();
                    SqlConn.Close();
                }

            }
            catch (SqlException SqlEx)
            {
                Global.gintErrorNo = 100;
                Global.gstrErrorDesc = mstrModule + "=>DeleteItem=>" + SqlEx.Message;
            }
            catch (Exception ex)
            {
                Global.gintErrorNo = 101;
                Global.gstrErrorDesc = mstrModule + "=>DeleteItem=>" + ex.Message;
            }
            finally
            {
                sqlComm = null;
            }
        }
        public void AddItemTran(int? id, List<Item> listItemsTran, string strConnections)
        {
            SqlCommand? sqlComm;
            try
            {
                using (SqlConnection SqlConn = new SqlConnection(strConnections))
                {
                    SqlConn.Open();


                    foreach (var item in listItemsTran)
                    {
                        sqlComm = new SqlCommand("AddItemTran", SqlConn);

                        sqlComm.Parameters.Clear();

                        sqlComm.CommandType = CommandType.StoredProcedure;

                        sqlComm.Parameters.AddWithValue("@EmpID", id);
                        sqlComm.Parameters.AddWithValue("@ItemID", item.ItemId);
                        sqlComm.Parameters.AddWithValue("@TodayDate", DateTime.Now);
                        sqlComm.Parameters.AddWithValue("@Amount", item.Amount);
                        sqlComm.Parameters.AddWithValue("@Quantiy", item.Quantity);
                        sqlComm.ExecuteNonQuery();
                    }
                    SqlConn.Close();

                }

            }
            catch (SqlException SqlEx)
            {
                Global.gintErrorNo = 100;
                Global.gstrErrorDesc = mstrModule + "=>AddEditItem=>" + SqlEx.Message;
            }
            catch (Exception ex)
            {
                Global.gintErrorNo = 101;
                Global.gstrErrorDesc = mstrModule + "=>AddEditItem=>" + ex.Message;
            }
            finally
            {
                sqlComm = null;
            }

        }
        public List<Item>? getPlacedOrder(int? id, string TodayDate, string strConnections)
        {
            string strQry;
            SqlCommand? sqlComm;
            List<Item>? listItem = null;
            try
            {
                listItem = new List<Item>();

                strQry = "SELECT IT.ItemID,IM.ItemName, SUM(IT.Amount) AS Amount, SUM(IT.Quantiy) As Quantiy " +
                           "FROM tbl_ItemTran IT, tbl_ItemMaster IM " +
                           "WHERE IT.ItemID = IM.ItemId " +
                           "AND IT.TodayDate = '" + TodayDate + "'" +
                           "AND IT.EmpID = " + id + "" +
                           "GROUP BY IM.ItemName,IT.EmpID,IT.ItemID, IT.TodayDate ";


                using (SqlConnection SqlConn = new SqlConnection(strConnections))
                {
                    SqlConn.Open();
                    sqlComm = new SqlCommand(strQry, SqlConn);

                    using (SqlDataReader reader = sqlComm.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Item item = new Item();
                            item.ItemId = reader.GetInt32("ItemID");
                            item.ItemName = reader.GetString("ItemName");
                            item.Amount = reader.GetDecimal("Amount");
                            item.Quantity = Convert.ToInt32(reader.GetDecimal("Quantiy"));
                            listItem.Add(item);
                        }
                        reader.Close();
                    }
                    SqlConn.Close();
                }
            }
            catch (SqlException SqlEx)
            {
                Global.gintErrorNo = 100;
                Global.gstrErrorDesc = mstrModule + "=>getAllItems=>" + SqlEx.Message;
            }
            catch (Exception ex)
            {
                Global.gintErrorNo = 101;
                Global.gstrErrorDesc = mstrModule + "=>getAllItems=>" + ex.Message;
            }
            finally
            {
                sqlComm = null;
            }
            return listItem;
        }
        public void DeletePlacedItem(int? id, int? Empid, string Todaydate, string strConnections)
        {
            SqlCommand? sqlComm;
            string strQry;
            try
            {
                using (SqlConnection SqlConn = new SqlConnection(strConnections))
                {
                    SqlConn.Open();
                    strQry = "DELETE FROM tbl_ItemTran WHERE ItemId = " + id + " AND EmpID = " + Empid + " AND TodayDate = '" + Todaydate + "'";
                    sqlComm = new SqlCommand(strQry, SqlConn);
                    sqlComm.ExecuteNonQuery();
                    SqlConn.Close();
                }

            }
            catch (SqlException SqlEx)
            {
                Global.gintErrorNo = 100;
                Global.gstrErrorDesc = mstrModule + "=>DeletePlacedItem=>" + SqlEx.Message;
            }
            catch (Exception ex)
            {
                Global.gintErrorNo = 101;
                Global.gstrErrorDesc = mstrModule + "=>DeletePlacedItem=>" + ex.Message;
            }
            finally
            {
                sqlComm = null;
            }
        }
        public DataTable? getYear(string strConnections)
        {
            string strQry;

            DataTable dtYear;
            SqlDataAdapter? sqlAdp;
            try
            {

                strQry = "SELECT DISTINCT YEAR(TodayDate) AS Year FROM tbl_ItemTran ORDER BY YEAR(TodayDate) DESC ";

                using (SqlConnection SqlConn = new SqlConnection(strConnections))
                {
                    SqlConn.Open();
                    sqlAdp = new SqlDataAdapter(strQry, SqlConn);
                    dtYear = new DataTable();
                    sqlAdp.Fill(dtYear);
                    SqlConn.Close();
                }
                return dtYear;
            }
            catch (SqlException SqlEx)
            {
                Global.gintErrorNo = 100;
                Global.gstrErrorDesc = mstrModule + "=>getYear=>" + SqlEx.Message;
                return null;
            }
            catch (Exception ex)
            {
                Global.gintErrorNo = 101;
                Global.gstrErrorDesc = mstrModule + "=>getYear=>" + ex.Message;
                return null;
            }
            finally
            {
                sqlAdp = null;
            }

        }
        public SelectList ToSelectList(List<ReportFilter> lstskill)
        {
            try
            {
                List<SelectListItem> list = new List<SelectListItem>();

                foreach (ReportFilter item in lstskill)
                {
                    list.Add(new SelectListItem()
                    {
                        Text = item.Month,
                        Value = item.Month
                    });
                }

                return new SelectList(list, "Value", "Text");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        public SelectList ToSelectMonthList(List<ReportFilter> lstskill)
        {
            try
            {
                List<SelectListItem> list = new List<SelectListItem>();

                foreach (ReportFilter item in lstskill)
                {
                    list.Add(new SelectListItem()
                    {
                        Text = item.Month,
                        Value = item.MonthId.ToString()
                    });
                }

                return new SelectList(list, "Value", "Text");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        public List<Item>? getDayWiseOrder(int? id, DateTime Date, string strConnections)
        {
            string strQry;
            SqlCommand? sqlComm;
            List<Item>? listItem = null;
            try
            {
                listItem = new List<Item>();


                strQry = "SELECT IM.ItemID, IM.ItemName,SUM(IT.Quantiy) AS Quantiy,IT.Amount " +
                           "FROM tbl_ItemTran IT,tbl_ItemMaster IM " +
                             "WHERE IT.ItemID = IM.ItemId " +
                             "AND EmpID = " + Convert.ToInt16(id) + "" +
                             "AND CONVERT(CHAR(8),IT.TodayDate,112) = '" + Date.ToString("yyyyMMdd") + "' " +
                             "GROUP BY IM.ItemID,IM.ItemName,IT.AMOUNT ";

                using (SqlConnection SqlConn = new SqlConnection(strConnections))
                {
                    SqlConn.Open();
                    sqlComm = new SqlCommand(strQry, SqlConn);

                    using (SqlDataReader reader = sqlComm.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Item item = new Item();
                            item.ItemId = Convert.ToInt16(reader["ItemID"]);
                            item.ItemName = reader.GetString("ItemName");
                            item.Quantity = Convert.ToInt32(reader.GetDecimal("Quantiy"));
                            item.Rate = reader.GetDecimal("Amount");
                            item.Amount = reader.GetDecimal("Quantiy") * reader.GetDecimal("Amount");
                            listItem.Add(item);
                        }
                        reader.Close();
                    }
                    SqlConn.Close();
                }
            }
            catch (SqlException SqlEx)
            {
                Global.gintErrorNo = 100;
                Global.gstrErrorDesc = mstrModule + "=>getAllItems=>" + SqlEx.Message;
            }
            catch (Exception ex)
            {
                Global.gintErrorNo = 101;
                Global.gstrErrorDesc = mstrModule + "=>getAllItems=>" + ex.Message;
            }
            finally
            {
                sqlComm = null;
            }
            return listItem;
        }
        public bool GenerateOrder(string Todaydate, string strConnections)
        {
            SqlCommand? sqlComm;
            string strQry;
            try
            {
                using (SqlConnection SqlConn = new SqlConnection(strConnections))
                {
                    SqlConn.Open();

                    strQry = "UPDATE tbl_ItemTran SET ISGENERATED = 1 " +
                                "WHERE TodayDate = '" + Todaydate + "' " +
                                "AND ISNULL(ISGENERATED,0) = 0 ";

                    sqlComm = new SqlCommand(strQry, SqlConn);
                    sqlComm.ExecuteNonQuery();
                    SqlConn.Close();
                }
                return true;

            }
            catch (SqlException SqlEx)
            {
                Global.gintErrorNo = 100;
                Global.gstrErrorDesc = mstrModule + "=>GenerateOrder=>" + SqlEx.Message;
                return false;
            }
            catch (Exception ex)
            {
                Global.gintErrorNo = 101;
                Global.gstrErrorDesc = mstrModule + "=>GenerateOrder=>" + ex.Message;
                return false;
            }
            finally
            {
                sqlComm = null;
            }
        }
    }
}
