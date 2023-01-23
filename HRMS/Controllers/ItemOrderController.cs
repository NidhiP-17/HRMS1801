using HRMS.Classes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HRMS.Models;
using Models;
using NuGet.Protocol.Plugins;
using Repositories;
using Microsoft.AspNetCore.Http;
using WebSite.Common;
using System.Data;
using System.Globalization;
using System.Collections.Generic;
using ReportFilter = HRMS.Models.ReportFilter;
using System.Data.SqlClient;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using Microsoft.AspNetCore.StaticFiles;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System;

namespace HRMS.Controllers
{
    [SessionTimeoutAttribute]
    public class ItemOrderController : Controller
    {
        private readonly IConfiguration configuration;
        List<Item>? mlstItem;
        string strSource = "ItemOrder";
        public ItemOrderController(IConfiguration config)
        {
            configuration = config;
        }
        public IActionResult Index(int? pageNumber, string Msg)
        {
            int pageSize = 15;
            List<Item>? listItem = null;
            Item? objItem = null;
            string msg = "";
            try
            {
                //if (Global.GetSession(HttpContext.Session.GetString(Global.SessionKeyName)) == false)
                //{
                //    return RedirectToAction(nameof(EmployeeModel), "Home"); ;
                //}

                HttpContext.Session.Remove("SearchString");

                if (string.IsNullOrEmpty(Msg))
                {
                    HttpContext.Session.Remove("cartItems");
                }
                else
                {
                    ViewBag.Message = Msg;
                }
                var client = new TcpClient("time.nist.gov", 13);
                DateTime localDateTime;
                objItem = new Item();
                listItem = new List<Item>();

                using (var streamReader = new StreamReader(client.GetStream()))
                {
                    var response = streamReader.ReadToEnd();
                    var utcDateTimeString = response.Substring(7, 17);
                    localDateTime = DateTime.ParseExact(utcDateTimeString, "yy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
                }
                if (localDateTime.Hour >= 16 || localDateTime.Hour <= 14)
                {
                    HttpContext.Session.Remove("cartItems");
                }
                objItem.mstrSource = strSource;
                listItem = objItem.getAllItems("", configuration.GetConnectionString("DefaultConnection"));

                mlstItem = HttpContext.Session.Get<List<Item>>("cartItems");
                ViewBag.Order = mlstItem;

                objItem = null;
                return View(PaginatedList<Item>.Create(listItem, pageNumber ?? 1, pageSize));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IActionResult OrderNow(int id, int? pageNumber)
        {
            Item? objItem;
            string Msg = "";
            try
            {
                //if (Global.GetSession(HttpContext.Session.GetString(Global.SessionKeyName)) == false)
                //{
                //    return RedirectToAction(nameof(EmployeeModel), "Home");
                //}

                HttpContext.Session.Remove("SearchString");
                var client = new TcpClient("time.nist.gov", 13);
                DateTime localDateTime;
                using (var streamReader = new StreamReader(client.GetStream()))
                {
                    var response = streamReader.ReadToEnd();
                    var utcDateTimeString = response.Substring(7, 17);
                    localDateTime = DateTime.ParseExact(utcDateTimeString, "yy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
                }
                var date = DateTime.Now;

                int intHour = date.Hour;

                if (intHour >= 14 && intHour < 16)
                {
                    var diffInSeconds = (localDateTime - date).TotalSeconds;
                    if (diffInSeconds <= 5)
                    {
                        objItem = new Item();
                        List<Item> listItems = new List<Item>();
                        objItem.GetItem(id, configuration.GetConnectionString("DefaultConnection"));

                        listItems.Add(new Item
                        {
                            ItemId = objItem.ItemId,
                            ItemName = objItem.ItemName,
                            Quantity = 1,
                            Amount = objItem.Amount
                        });

                        mlstItem = HttpContext.Session.Get<List<Item>>("cartItems");

                        if (id > 0)
                        {
                            if (mlstItem != null && mlstItem.Count > 0)
                            {
                                foreach (var x in mlstItem)
                                {
                                    var itemToChange = mlstItem.FirstOrDefault(d => d.ItemId == objItem.ItemId);
                                    if (itemToChange != null)
                                    {
                                        if (itemToChange.ItemId == x.ItemId)
                                        {
                                            x.Quantity = itemToChange.Quantity + 1;
                                        }
                                        else
                                        {
                                            continue;
                                        }

                                    }
                                    else
                                    {
                                        mlstItem.Add(new Item
                                        {
                                            ItemId = objItem.ItemId,
                                            ItemName = objItem.ItemName,
                                            Quantity = 1,
                                            Amount = objItem.Amount
                                        });
                                        break;
                                    }

                                }

                            }
                            else
                            {
                                mlstItem = listItems;
                            }

                            HttpContext.Session.Set<List<Item>>("cartItems", mlstItem);
                        }
                        decimal TotalAmount = 0;

                        foreach (var x in mlstItem)
                        {
                            TotalAmount = TotalAmount + (x.Quantity * x.Amount);
                        }
                        ViewBag.TotalAmount = TotalAmount;

                        ViewBag.Order = mlstItem;

                        Msg = "Item ordered successfully.";

                    }
                    else
                    {
                        Msg = "You change the system date! You can not order now";
                        return RedirectToAction("Index", new { pageNumber = pageNumber, Msg = Msg });
                    }

                }
                else
                {
                    Msg = "You may order between 2 to 4 only!";
                }

                return RedirectToAction("Index", new { pageNumber = pageNumber, Msg = Msg });

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }


        }
        public IActionResult Delete(int id, int? pageNumber)
        {
            try
            {
                //if (Global.GetSession(HttpContext.Session.GetString(Global.SessionKeyName)) == false)
                //{
                //    return RedirectToAction(nameof(EmployeeModel), "Home");
                //}

                HttpContext.Session.Remove("SearchString");

                mlstItem = HttpContext.Session.Get<List<Item>>("cartItems");

                if (mlstItem != null && mlstItem.Count > 0)
                {
                    var item = mlstItem.SingleOrDefault(x => x.ItemId == id);
                    if (item != null)
                        mlstItem.Remove(item);
                }

                HttpContext.Session.Set<List<Item>>("cartItems", mlstItem);
                return RedirectToAction("Index", new { pageNumber = pageNumber });

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        public IActionResult PlaceOrder()
        {
            Item? objItem;
            string? id;

            try
            {
                //if (Global.GetSession(HttpContext.Session.GetString(Global.SessionKeyName)) == false)
                //{
                //    return RedirectToAction(nameof(EmployeeModel), "Home");
                //}
                HttpContext.Session.Remove("SearchString");

                mlstItem = HttpContext.Session.Get<List<Item>>("cartItems");



                if (mlstItem != null && mlstItem.Count > 0)
                {
                    objItem = new Item();
                    objItem.AddItemTran(Convert.ToInt16(ViewBag.userId), mlstItem, configuration.GetConnectionString("DefaultConnection"));
                }

                HttpContext.Session.Remove("cartItems");
                return RedirectToAction("Index", new { pageNumber = 0, Msg = "Order Placed Successfully" });
                //return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        public IActionResult EditOrder(int? pageNumber)
        {
            int pageSize = 8;
            List<Item>? listItem = null;
            Item? objItem = null;
            string? id;

            try
            {
                //if (Global.GetSession(HttpContext.Session.GetString(Global.SessionKeyName)) == false)
                //{
                //    return RedirectToAction(nameof(EmployeeModel), "Home");
                //}
                HttpContext.Session.Remove("SearchString");

                objItem = new Item();
                listItem = new List<Item>();

                listItem = objItem.getPlacedOrder(Convert.ToInt16(ViewBag.userId), Convert.ToDateTime(DateTime.Now).ToString("yyyy/MM/dd"), configuration.GetConnectionString("DefaultConnection"));

                objItem = null;

                return View(PaginatedList<Item>.Create(listItem, pageNumber ?? 1, pageSize));

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public IActionResult ReportEmpData()
        {
            //if (Global.GetSession(HttpContext.Session.GetString(Global.SessionKeyName)) == false)
            //{
            //    return RedirectToAction(nameof(EmployeeModel), "Home");
            //}

            HttpContext.Session.Remove("SearchString");

            ReportFilter? objReport = null;
            List<ReportFilter>? lstReport = new List<ReportFilter>();
            DataTable? dtYear = new DataTable();
            Item? item = new Item();

            for (int i = 0; i <= 11; i++)
            {
                objReport = new ReportFilter();
                objReport.Month = CultureInfo.CurrentCulture.DateTimeFormat.MonthNames[i];
                objReport.MonthId = i;
                lstReport.Add(objReport);
            }
            ViewBag.MonthListItem = item.ToSelectMonthList(lstReport);

            lstReport = new List<ReportFilter>();

            dtYear = item.getYear(configuration.GetConnectionString("DefaultConnection"));

            if (dtYear != null)
            {
                foreach (DataRow dr in dtYear.Rows)
                {
                    objReport = new ReportFilter();
                    objReport.Month = dr["Year"].ToString();
                    lstReport.Add(objReport);
                }
            }

            ViewBag.YearListItem = item.ToSelectList(lstReport);

            item = null;
            lstReport = null;

            return View(objReport);
        }
        public IActionResult ReportMonthWiseData()
        {
            //if (Global.GetSession(HttpContext.Session.GetString(Global.SessionKeyName)) == false)            //{
            //    return RedirectToAction(nameof(EmployeeModel), "Home");
            //}

            HttpContext.Session.Remove("SearchString");
            ReportFilter? objReport = null;
            List<ReportFilter>? lstReport = new List<ReportFilter>();
            DataTable? dtYear = new DataTable();
            Item? item = new Item();

            for (int i = 1; i <= 31; i++)
            {
                objReport = new ReportFilter();
                objReport.Month = i.ToString();
                lstReport.Add(objReport);
            }
            ViewBag.DayListItem = item.ToSelectList(lstReport);

            lstReport = new List<ReportFilter>();

            for (int i = 0; i <= 11; i++)
            {
                objReport = new ReportFilter();
                objReport.Month = CultureInfo.CurrentCulture.DateTimeFormat.MonthNames[i];
                objReport.MonthId = i;
                lstReport.Add(objReport);
            }
            ViewBag.MonthListItem = item.ToSelectMonthList(lstReport);

            lstReport = new List<ReportFilter>();

            dtYear = item.getYear(configuration.GetConnectionString("DefaultConnection"));

            if (dtYear != null)
            {
                foreach (DataRow dr in dtYear.Rows)
                {
                    objReport = new ReportFilter();
                    objReport.Month = dr["Year"].ToString();
                    lstReport.Add(objReport);
                }
            }

            ViewBag.YearListItem = item.ToSelectList(lstReport);

            dtYear = null;
            item = null;
            lstReport = null;

            return View(objReport);
        }
        public IActionResult ReportEmpDay()
        {
            try
            {
                //if (Global.GetSession(HttpContext.Session.GetString(Global.SessionKeyName)) == false)
                //{
                //    return RedirectToAction(nameof(EmployeeModel), "Home");
                //}

                HttpContext.Session.Remove("SearchString");

                ViewBag.Date = DateTime.Now.ToString("dd-MM-yyyy");

                return View();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        //[HttpPost]
        //public IActionResult ReportEmpDay(string date)
        //{
        //    try
        //    {
        //        if (Global.GetSession(HttpContext.Session.GetString(Global.SessionKeyName)) == false)
        //        {
        //            return RedirectToAction(nameof(Login), "Home");
        //        }

        //HttpContext.Session.Remove("SearchString");

        //if (string.IsNullOrEmpty(date))
        //{
        //    ViewBag.Message = "Please select date!";
        //    return View();
        //}

        //Print(date);

        //ViewBag.Date = DateTime.Now.ToString("dd-MM-yyyy");

        //        return View();

        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}
        public IActionResult ReportEmp_Day(int? pageNumber)
        {
            int pageSize = 8;
            List<Item>? listItem = null;
            Item? objItem = null;
            string? id;
            try
            {
                //if (Global.GetSession(HttpContext.Session.GetString(Global.SessionKeyName)) == false)
                //{
                //    return RedirectToAction(nameof(EmployeeModel), "Home");
                //}

                if (TempData["msg"] != null)
                    ViewBag.Message = TempData["msg"].ToString();

                objItem = new Item();
                listItem = new List<Item>();
                listItem = objItem.getDayWiseOrder(Convert.ToInt16(ViewBag.userId), DateTime.Now, configuration.GetConnectionString("DefaultConnection"));

                ViewBag.Date = DateTime.Now.ToString("dd-MM-yyyy");

                objItem = null;

                return View(PaginatedList<Item>.Create(listItem, pageNumber ?? 1, pageSize));

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        [HttpPost]
        public IActionResult ReportEmp_Day(int? pageNumber, string date)
        {
            int pageSize = 8;
            List<Item>? listItem = null;
            Item? objItem = null;
            string? id;
            try
            {
                //if (Global.GetSession(HttpContext.Session.GetString(Global.SessionKeyName)) == false)
                //{
                //    return RedirectToAction(nameof(EmployeeModel), "Home");
                //}



                HttpContext.Session.Remove("SearchString");

                if (string.IsNullOrEmpty(date))
                {
                    ViewBag.Message = "Please select date!";
                    ViewBag.Date = Convert.ToDateTime(DateTime.Now).ToString("dd-MM-yyyy");
                    date = Convert.ToDateTime(DateTime.Now).ToString("dd-MM-yyyy");
                }
                else
                {
                    ViewBag.Date = Convert.ToDateTime(date).ToString("dd-MM-yyyy");
                }

                objItem = new Item();
                listItem = new List<Item>();
                listItem = objItem.getDayWiseOrder(Convert.ToInt16(ViewBag.userId), Convert.ToDateTime(date), configuration.GetConnectionString("DefaultConnection"));
                objItem = null;

                return View(PaginatedList<Item>.Create(listItem, pageNumber ?? 1, pageSize));

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }


        }
        [HttpPost]
        public IActionResult EditOrder(string str, int? pageNumber)
        {
            int pageSize = 8;
            List<Item>? listItem = null;
            Item? objItem = null;
            try
            {
                //if (Global.GetSession(HttpContext.Session.GetString(Global.SessionKeyName)) == false)
                //{
                //    return RedirectToAction(nameof(EmployeeModel), "Home");
                //}
                HttpContext.Session.Remove("SearchString");

                objItem = new Item();
                listItem = new List<Item>();
                listItem = objItem.getAllItems("", configuration.GetConnectionString("DefaultConnection"));

                objItem = null;
                TempData["msg"] = "Order Deleted Successfully";
                return View(PaginatedList<Item>.Create(listItem, pageNumber ?? 1, pageSize));

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public IActionResult DeleteOrder(int id, int? pageNumber)
        {
            Item? objItem;
            try
            {
                //if (Global.GetSession(HttpContext.Session.GetString(Global.SessionKeyName)) == false)
                //{
                //    return RedirectToAction(nameof(EmployeeModel), "Home");
                //}

                HttpContext.Session.Remove("SearchString");

                objItem = new Item();
                objItem.DeletePlacedItem(id, Convert.ToInt16(ViewBag.userId), Convert.ToDateTime(DateTime.Now).ToString("yyyy/MM/dd"), configuration.GetConnectionString("DefaultConnection"));

                objItem = null;
                TempData["msg"] = "Order Deleted Successfully";
                return RedirectToAction("ReportEmp_Day", new { pageNumber = pageNumber });

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        public async Task<IActionResult> Print(string date)
        {
            SqlConnection? connection;
            SqlCommand? command;
            SqlDataAdapter? adapter = new SqlDataAdapter();
            DataSet? ds = new DataSet();
            int i = 0;
            string sql = "";
            int yPoint = 0;
            string todayDate = "";
            string Name = "";
            string ItemName = "";
            Int32 Quantiy;
            decimal Amount;
            string StrName = "";
            Double GrandTotalAmount = 0;
            Double TotalQuantity = 0;
            DateTime dtTodaydate = DateTime.Now;
            int totalLine = 0;
            int pageNo = 1;

            try
            {
                //if (Global.GetSession(HttpContext.Session.GetString(Global.SessionKeyName)) == false)
                //{
                //    return RedirectToAction(nameof(EmployeeModel), "Home");
                //}

                HttpContext.Session.Remove("SearchString");

                XFont xFontRegular = new XFont("Verdana", 10, XFontStyle.Regular);
                XFont xFontName = new XFont("Verdana", 10, XFontStyle.Bold);
                XFont xFontBold = new XFont("Verdana", 10, XFontStyle.Bold);
                XFont xCompBold = new XFont("Verdana", 12, XFontStyle.Bold);

                sql = "SELECT IT.TodayDate,E.FirstName+' '+E.LastName AS employeeName, " +
                            "IM.ItemName,IT.Quantiy,IT.Amount " +
                        "FROM tbl_ItemTran IT,tbl_ItemMaster IM,tbl_Employee E " +
                        "WHERE IT.ItemID = IM.ItemId " +
                        "AND IT.EmpID = E.employeeId " +
                        "AND  CONVERT(CHAR(8),IT.TodayDate,112) = '" + Convert.ToDateTime(date).ToString("yyyyMMdd") + "'" +
                        "ORDER BY employeeName ";

                connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
                connection.Open();
                command = new SqlCommand(sql, connection);
                adapter.SelectCommand = command;
                adapter.Fill(ds);
                connection.Close();

                command = null;
                connection = null;
                adapter = null;

                PdfDocument pdfDocument = new PdfDocument();
                //Create an empty page
                PdfPage pdfPage = pdfDocument.AddPage();
                // Get an XGraphics object for drawing
                XGraphics xGraphics = XGraphics.FromPdfPage(pdfPage);
                //  Create a font
                XPen lineRed = new XPen(XColors.Black, 55);

                yPoint = yPoint + 100;

                // Draw Line
                double x = 80;
                XPen pen = XPens.Black;
                xGraphics.DrawLine(pen, x - 65, 60, 580, 60);
                xGraphics.DrawLine(pen, x - 65, yPoint - 10, 580, yPoint - 10);
                xGraphics.DrawLine(pen, x - 65, yPoint + 12, 580, yPoint + 12);

                CheckDuplicateRecords(ds.Tables[0]);
                todayDate = Convert.ToDateTime(date).ToString("dd/M/yyyy");

                //Header
                xGraphics.DrawString("Alita Infotech Pvt.Ltd", xCompBold, XBrushes.Black, new XRect(0, 30, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopCenter);
                xGraphics.DrawString("Report Date: " + todayDate, xFontBold, XBrushes.Black, new XRect(25, 70, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                xGraphics.DrawString("Particular", xFontBold, XBrushes.Black, new XRect(-180, yPoint - 5, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopCenter);
                xGraphics.DrawString("Qty", xFontBold, XBrushes.Black, new XRect(70, yPoint - 5, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopCenter);
                xGraphics.DrawString("Rate", xFontBold, XBrushes.Black, new XRect(150, yPoint - 5, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopCenter);
                xGraphics.DrawString("Amount", xFontBold, XBrushes.Black, new XRect(230, yPoint - 5, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopCenter);

                for (i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
                {
                    totalLine = totalLine + 1;

                    if (totalLine == 30 && pageNo == 1 || (totalLine == 32 && pageNo > 1))
                    {
                        pdfPage = pdfDocument.AddPage();
                        xGraphics = XGraphics.FromPdfPage(pdfPage);
                        totalLine = 0;
                        yPoint = 10;
                        pageNo++;
                    }

                    todayDate = Convert.ToDateTime(ds.Tables[0].Rows[i].ItemArray[0]).ToString("dd/M/yyyy");
                    Name = ds.Tables[0].Rows[i].ItemArray[1].ToString();
                    ItemName = ds.Tables[0].Rows[i].ItemArray[2].ToString();
                    Quantiy = Convert.ToInt32(ds.Tables[0].Rows[i].ItemArray[3]);
                    Amount = Convert.ToDecimal(ds.Tables[0].Rows[i].ItemArray[4]);
                    Double TotalAmount = Convert.ToDouble(Quantiy * Amount);
                    ds.Tables[0].AcceptChanges();

                    TotalQuantity += Quantiy;
                    GrandTotalAmount += TotalAmount;

                    if (StrName != Name)
                    {
                        yPoint = yPoint + 30;
                        StrName = Name;
                        xGraphics.DrawString(Name, xFontName, XBrushes.Black, new XRect(30, yPoint, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    }

                    xGraphics.DrawString(ItemName, xFontRegular, XBrushes.Black, new XRect(30, yPoint + 20, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);

                    xGraphics.DrawString(Quantiy.ToString(), xFontRegular, XBrushes.Black, new XRect(-220, yPoint + 20, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopRight);

                    xGraphics.DrawString(Amount.ToString(), xFontRegular, XBrushes.Black, new XRect(-130, yPoint + 20, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopRight);

                    xGraphics.DrawString(TotalAmount.ToString("0.00"), xFontRegular, XBrushes.Black, new XRect(-50, yPoint + 20, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopRight);

                    yPoint = yPoint + 20;

                }

                xGraphics.DrawString("Total", xFontBold, XBrushes.Black, new XRect(30, yPoint + 40, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);

                xGraphics.DrawString(TotalQuantity.ToString(), xFontBold, XBrushes.Black, new XRect(-220, yPoint + 40, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopRight);

                xGraphics.DrawString(GrandTotalAmount.ToString("0.00"), xFontBold, XBrushes.Black, new XRect(-50, yPoint + 40, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopRight);

                string fileName = "";
                var extenstion = ".pdf";

                fileName = "Food_Report_Of_Employees_" + DateTime.Now.ToString("yyyyMMddHHmmss") + extenstion;

                var pathBuild = Path.Combine(Directory.GetCurrentDirectory(), "upload\\files");

                if (!Directory.Exists(pathBuild))
                {
                    Directory.CreateDirectory(pathBuild);
                }

                pdfDocument.Save(pathBuild + "\\" + fileName);

                var path = Path.Combine(Directory.GetCurrentDirectory(), "upload\\files", pathBuild + "\\" + fileName);
                var provider = new FileExtensionContentTypeProvider();

                if (!provider.TryGetContentType(path, out var ContentType))
                {
                    ContentType = "application/octet-stream";

                }
                var bytes = await System.IO.File.ReadAllBytesAsync(path);

                System.IO.File.Delete(path);

                return File(bytes, ContentType, Path.GetFileName(path));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        public async Task<IActionResult> Print1()
        {
            SqlConnection? connection;
            SqlCommand? command;
            SqlDataAdapter? adapter = new SqlDataAdapter();
            DataSet? ds = new DataSet();
            int i = 0;
            string sql = "";
            int yPoint = 0;
            string todayDate = "";
            string Name = "";
            string ItemCategory = "";
            Int32 Quantiy;
            decimal Amount;
            Double GrandTotalAmount = 0;
            Double TotalQuantity = 0;
            DateTime dtTodaydate = DateTime.Now;
            int totalLine = 0;
            int pageNo = 1;

            try
            {


                //if (Global.GetSession(HttpContext.Session.GetString(Global.SessionKeyName)) == false)
                //{
                //    return RedirectToAction(nameof(EmployeeModel), "Home");
                //}

                HttpContext.Session.Remove("SearchString");

                XFont xFontRegular = new XFont("Verdana", 10, XFontStyle.Regular);
                XFont xFontName = new XFont("Verdana", 10, XFontStyle.Bold);
                XFont xFontBold = new XFont("Verdana", 10, XFontStyle.Bold);
                XFont xCompBold = new XFont("Verdana", 12, XFontStyle.Bold);

                sql = "SELECT IC.ItemCatName,IM.ItemName,SUM(IT.Quantiy),IT.Amount " +
                         "FROM tbl_ItemTran IT,tbl_ItemMaster IM, tbl_ItemCategory IC " +
                         "WHERE IT.ItemID = IM.ItemId " +
                         "AND IM.ItemCatID = IC.ItemCatID " +
                         "AND  CONVERT(CHAR(8),IT.TodayDate,112) = '" + dtTodaydate.ToString("yyyyMMdd") + "'" +
                         "GROUP BY  ItemName,IT.Amount,IC.ItemCatName " +
                         "ORDER BY ItemName ";

                connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
                connection.Open();
                command = new SqlCommand(sql, connection);
                adapter.SelectCommand = command;
                adapter.Fill(ds);
                connection.Close();

                command = null;
                adapter = null;
                connection = null;

                PdfDocument pdfDocument = new PdfDocument();
                // Create an empty page
                PdfPage pdfPage = pdfDocument.AddPage();
                // Get an XGraphics object for drawing
                XGraphics xGraphics = XGraphics.FromPdfPage(pdfPage);
                // Create a font                      
                XPen lineRed = new XPen(XColors.Black, 55);

                yPoint = yPoint + 100;

                todayDate = Convert.ToDateTime(DateTime.Now).ToString("dd/M/yyyy");

                //Draw Line
                double x = 80;
                XPen pen = XPens.Black;
                xGraphics.DrawLine(pen, x - 65, 60, 580, 60);
                xGraphics.DrawLine(pen, x - 65, yPoint - 10, 580, yPoint - 10);
                xGraphics.DrawLine(pen, x - 65, yPoint + 12, 580, yPoint + 12);

                //Header
                xGraphics.DrawString("Alita Infotech Pvt.Ltd", xCompBold, XBrushes.Black, new XRect(0, 30, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopCenter);
                xGraphics.DrawString("Report Date: " + todayDate, xFontBold, XBrushes.Black, new XRect(25, 70, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                xGraphics.DrawString("Category", xFontBold, XBrushes.Black, new XRect(-230, yPoint - 5, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopCenter);
                xGraphics.DrawString("Name", xFontBold, XBrushes.Black, new XRect(-90, yPoint - 5, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopCenter);
                xGraphics.DrawString("Qty", xFontBold, XBrushes.Black, new XRect(70, yPoint - 5, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopCenter);
                xGraphics.DrawString("Rate", xFontBold, XBrushes.Black, new XRect(150, yPoint - 5, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopCenter);
                xGraphics.DrawString("Amount", xFontBold, XBrushes.Black, new XRect(230, yPoint - 5, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopCenter);

                for (i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
                {
                    totalLine = totalLine + 1;

                    if (totalLine == 30 && pageNo == 1 || (totalLine == 35 && pageNo > 1))
                    {
                        pdfPage = pdfDocument.AddPage();
                        xGraphics = XGraphics.FromPdfPage(pdfPage);
                        totalLine = 0;
                        yPoint = 10;
                        pageNo++;
                    }

                    ItemCategory = ds.Tables[0].Rows[i].ItemArray[0].ToString();
                    Name = ds.Tables[0].Rows[i].ItemArray[1].ToString();
                    Quantiy = Convert.ToInt32(ds.Tables[0].Rows[i].ItemArray[2]);
                    Amount = Convert.ToDecimal(ds.Tables[0].Rows[i].ItemArray[3]);
                    Double TotalAmount = Convert.ToDouble(Quantiy * Amount);
                    ds.Tables[0].AcceptChanges();

                    TotalQuantity += Quantiy;
                    GrandTotalAmount += TotalAmount;

                    xGraphics.DrawString(ItemCategory, xFontRegular, XBrushes.Black, new XRect(30, yPoint + 20, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);

                    xGraphics.DrawString(Name, xFontRegular, XBrushes.Black, new XRect(180, yPoint + 20, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);

                    xGraphics.DrawString(Quantiy.ToString(), xFontRegular, XBrushes.Black, new XRect(-220, yPoint + 20, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopRight);

                    xGraphics.DrawString(Amount.ToString(), xFontRegular, XBrushes.Black, new XRect(-130, yPoint + 20, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopRight);

                    xGraphics.DrawString(TotalAmount.ToString("0.00"), xFontRegular, XBrushes.Black, new XRect(-50, yPoint + 20, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopRight);

                    yPoint = yPoint + 20;

                }

                xGraphics.DrawString("Total", xFontBold, XBrushes.Black, new XRect(30, yPoint + 40, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);

                xGraphics.DrawString(TotalQuantity.ToString(), xFontBold, XBrushes.Black, new XRect(-220, yPoint + 40, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopRight);

                xGraphics.DrawString(GrandTotalAmount.ToString("0.00"), xFontBold, XBrushes.Black, new XRect(-50, yPoint + 40, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopRight);

                string fileName = "";
                var extenstion = ".pdf";

                fileName = "Today's_Order_Report_" + DateTime.Now.ToString("yyyyMMddHHmmss") + extenstion;

                var pathBuild = Path.Combine(Directory.GetCurrentDirectory(), "upload\\files");

                if (!Directory.Exists(pathBuild))
                {
                    Directory.CreateDirectory(pathBuild);
                }

                pdfDocument.Save(pathBuild + "\\" + fileName);

                var path = Path.Combine(Directory.GetCurrentDirectory(), "upload\\files", pathBuild + "\\" + fileName);
                var provider = new FileExtensionContentTypeProvider();

                if (!provider.TryGetContentType(path, out var ContentType))
                {
                    ContentType = "application/octet-stream";

                }
                var bytes = await System.IO.File.ReadAllBytesAsync(path);

                System.IO.File.Delete(path);

                return File(bytes, ContentType, Path.GetFileName(path));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        //public async Task<IActionResult> Print2(ReportFilter report)
        //{
        //    SqlConnection connection;
        //    SqlCommand command;
        //    SqlDataAdapter adapter = new SqlDataAdapter();
        //    DataSet ds = new DataSet();
        //    int i = 0;
        //    string sql = "";
        //    int yPoint = 0;
        //    string todayDate = "";
        //    string Name = "";
        //    string ItemName = "";
        //    Int32 Quantiy;
        //    decimal Amount;
        //    string StrName = "";
        //    Double GrandTotalAmount = 0;
        //    Double TotalQuantity = 0;
        //    DateTime dtTodaydate = DateTime.Now;
        //    int totalLine = 0;
        //    int pageNo = 1;
        //    string? id;
        //    string? Admin;

        //    try
        //    {
        //        if (Global.GetSession(HttpContext.Session.GetString(Global.SessionKeyName)) == false)
        //        {
        //            return RedirectToAction(nameof(Login), "Home");
        //        }
        //        HttpContext.Session.Remove("SearchString");

        //        dtTodaydate = Convert.ToDateTime(report.ReportDate);
        //        XFont xFontRegular = new XFont("Verdana", 10, XFontStyle.Regular);
        //        XFont xFontName = new XFont("Verdana", 10, XFontStyle.Bold);
        //        XFont xFontBold = new XFont("Verdana", 10, XFontStyle.Bold);
        //        XFont xCompBold = new XFont("Verdana", 12, XFontStyle.Bold);

        //       

        //        sql = "SELECT IT.TodayDate,E.FirstName+' '+E.LastName AS employeeName, " +
        //                    "IM.ItemName,IT.Quantiy,IT.Amount " +
        //                "FROM ItemTran IT,ItemMaster IM,Employee E " +
        //                "WHERE IT.ItemID = IM.ItemId " +
        //                "AND IT.EmpID = E.EmpID " +
        //                "AND  E.EmpID = '" + Convert.ToInt16(id) + "'" +
        //                "AND  CONVERT(CHAR(8),IT.TodayDate,112) = '" + dtTodaydate.ToString("yyyyMMdd") + "'" +
        //                "ORDER BY employeeName ";

        //        connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
        //        connection.Open();
        //        command = new SqlCommand(sql, connection);
        //        adapter.SelectCommand = command;
        //        adapter.Fill(ds);
        //        connection.Close();

        //        PdfDocument pdfDocument = new PdfDocument();
        //        // Create an empty page
        //        PdfPage pdfPage = pdfDocument.AddPage();
        //        // Get an XGraphics object for drawing
        //        XGraphics xGraphics = XGraphics.FromPdfPage(pdfPage);
        //        // Create a font                      
        //        XPen lineRed = new XPen(XColors.Black, 55);

        //        yPoint = yPoint + 100;

        //        //Draw Line
        //        double x = 80;
        //        XPen pen = XPens.Black;
        //        xGraphics.DrawLine(pen, x - 65, 60, 580, 60);
        //        xGraphics.DrawLine(pen, x - 65, yPoint - 10, 580, yPoint - 10);
        //        xGraphics.DrawLine(pen, x - 65, yPoint + 12, 580, yPoint + 12);

        //        CheckDuplicateRecords(ds.Tables[0]);
        //        todayDate = Convert.ToDateTime(DateTime.Now).ToString("dd/M/yyyy");

        //        //Header
        //        xGraphics.DrawString("Alita Infotech Pvt.Ltd", xCompBold, XBrushes.Black, new XRect(0, 30, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopCenter);
        //        xGraphics.DrawString("Particular", xFontBold, XBrushes.Black, new XRect(-180, yPoint - 5, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopCenter);
        //        xGraphics.DrawString("Qty", xFontBold, XBrushes.Black, new XRect(70, yPoint - 5, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopCenter);
        //        xGraphics.DrawString("Rate", xFontBold, XBrushes.Black, new XRect(150, yPoint - 5, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopCenter);
        //        xGraphics.DrawString("Amount", xFontBold, XBrushes.Black, new XRect(230, yPoint - 5, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopCenter);

        //        for (i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
        //        {
        //            totalLine = totalLine + 1;

        //            if (totalLine == 30 && pageNo == 1 || (totalLine == 32 && pageNo > 1))
        //            {
        //                pdfPage = pdfDocument.AddPage();
        //                xGraphics = XGraphics.FromPdfPage(pdfPage);
        //                totalLine = 0;
        //                yPoint = 10;
        //                pageNo++;
        //            }

        //            todayDate = Convert.ToDateTime(ds.Tables[0].Rows[i].ItemArray[0]).ToString("dd/M/yyyy");
        //            Name = ds.Tables[0].Rows[i].ItemArray[1].ToString();
        //            ItemName = ds.Tables[0].Rows[i].ItemArray[2].ToString();
        //            Quantiy = Convert.ToInt32(ds.Tables[0].Rows[i].ItemArray[3]);
        //            Amount = Convert.ToDecimal(ds.Tables[0].Rows[i].ItemArray[4]);
        //            Double TotalAmount = Convert.ToDouble(Quantiy * Amount);
        //            ds.Tables[0].AcceptChanges();

        //            TotalQuantity += Quantiy;
        //            GrandTotalAmount += TotalAmount;

        //            if (i == 0)
        //            {
        //                xGraphics.DrawString("Order Date: " + todayDate, xFontBold, XBrushes.Black, new XRect(25, 70, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
        //            }

        //            xGraphics.DrawString(ItemName, xFontRegular, XBrushes.Black, new XRect(30, yPoint + 20, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);

        //            xGraphics.DrawString(Quantiy.ToString(), xFontRegular, XBrushes.Black, new XRect(-220, yPoint + 20, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopRight);

        //            xGraphics.DrawString(Amount.ToString(), xFontRegular, XBrushes.Black, new XRect(-130, yPoint + 20, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopRight);

        //            xGraphics.DrawString(TotalAmount.ToString("0.00"), xFontRegular, XBrushes.Black, new XRect(-50, yPoint + 20, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopRight);

        //            yPoint = yPoint + 20;

        //        }

        //        xGraphics.DrawString("Total", xFontBold, XBrushes.Black, new XRect(30, yPoint + 40, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);

        //        xGraphics.DrawString(TotalQuantity.ToString(), xFontBold, XBrushes.Black, new XRect(-220, yPoint + 40, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopRight);

        //        xGraphics.DrawString(GrandTotalAmount.ToString("0.00"), xFontBold, XBrushes.Black, new XRect(-50, yPoint + 40, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopRight);

        //        string fileName = "";
        //        var extenstion = ".pdf";

        //        fileName = "Report_Employee_" + DateTime.Now.ToString("yyyyMMddHHmmss") + extenstion;

        //        var pathBuild = Path.Combine(Directory.GetCurrentDirectory(), "upload\\files");

        //        if (!Directory.Exists(pathBuild))
        //        {
        //            Directory.CreateDirectory(pathBuild);
        //        }

        //        pdfDocument.Save(pathBuild + "\\" + fileName);

        //        var path = Path.Combine(Directory.GetCurrentDirectory(), "upload\\files", pathBuild + "\\" + fileName);
        //        var provider = new FileExtensionContentTypeProvider();

        //        if (!provider.TryGetContentType(path, out var ContentType))
        //        {
        //            ContentType = "application/octet-stream";

        //        }
        //        var bytes = await System.IO.File.ReadAllBytesAsync(path);

        //        System.IO.File.Delete(path);

        //        return File(bytes, ContentType, Path.GetFileName(path));
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }

        //}
        public async Task<IActionResult> PrintEmployeeData(ReportFilter report)
        {
            SqlConnection? connection;
            SqlCommand? command;
            SqlDataAdapter? adapter = new SqlDataAdapter();
            DataSet? ds = new DataSet();
            int i = 0;
            string sql = "";
            int yPoint = 0;
            string Date = "";
            string todayDate = "";
            double rate;
            Double ExpectedAmount;
            Double GrandTotalAmount = 0;
            Double TotalExpectedAmount = 0;
            Double TotalOrderRate = 0;
            DateTime dtTodaydate = DateTime.Now;
            int totalLine = 0;
            int pageNo = 1;
            string? id;

            try
            {

                //if (Global.GetSession(HttpContext.Session.GetString(Global.SessionKeyName)) == false)
                //{
                //    return RedirectToAction(nameof(EmployeeModel), "Home");
                //}

                HttpContext.Session.Remove("SearchString");

                XFont xFontRegular = new XFont("Verdana", 10, XFontStyle.Regular);
                XFont xFontName = new XFont("Verdana", 10, XFontStyle.Bold);
                XFont xFontBold = new XFont("Verdana", 10, XFontStyle.Bold);
                XFont xCompBold = new XFont("Verdana", 12, XFontStyle.Bold);



                sql = "SELECT EmpID, TodayDate, SUM(Amount * Quantiy) AS Amount " +
                            "FROM tbl_ItemTran " +
                       "WHERE MONTH(TodayDate) = '" + (Convert.ToInt16(report.Month) + 1) + "' AND YEAR(TodayDate) = '" + report.Year + "' " +
                       "AND EmpID = " + ViewBag.userId + " " +
                       "GROUP BY EmpID,TodayDate ";

                connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
                connection.Open();
                command = new SqlCommand(sql, connection);
                adapter.SelectCommand = command;
                adapter.Fill(ds);
                connection.Close();

                command = null;
                adapter = null;
                connection = null;

                PdfDocument pdfDocument = new PdfDocument();
                // Create an empty page
                PdfPage pdfPage = pdfDocument.AddPage();
                // Get an XGraphics object for drawing
                XGraphics xGraphics = XGraphics.FromPdfPage(pdfPage);
                // Create a font                      
                XPen lineRed = new XPen(XColors.Black, 55);

                yPoint = yPoint + 100;

                todayDate = Convert.ToDateTime(DateTime.Now).ToString("dd/M/yyyy");

                //Draw Line
                double x = 80;
                XPen pen = XPens.Black;
                xGraphics.DrawLine(pen, x - 65, 60, 580, 60);
                xGraphics.DrawLine(pen, x - 65, yPoint - 10, 580, yPoint - 10);
                xGraphics.DrawLine(pen, x - 65, yPoint + 12, 580, yPoint + 12);

                //Header
                xGraphics.DrawString("Alita Infotech Pvt.Ltd", xCompBold, XBrushes.Black, new XRect(0, 30, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopCenter);
                xGraphics.DrawString(HttpContext.Session.GetString("loginUserName"), xFontBold, XBrushes.Black, new XRect(25, 70, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                xGraphics.DrawString("Report Date: " + todayDate, xFontBold, XBrushes.Black, new XRect(-25, 70, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopRight);
                xGraphics.DrawString("Date", xFontBold, XBrushes.Black, new XRect(-230, yPoint - 5, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopCenter);
                xGraphics.DrawString("Order", xFontBold, XBrushes.Black, new XRect(70, yPoint - 5, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopCenter);
                xGraphics.DrawString("Accepted ", xFontBold, XBrushes.Black, new XRect(150, yPoint - 5, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopCenter);
                xGraphics.DrawString("Amount", xFontBold, XBrushes.Black, new XRect(230, yPoint - 5, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopCenter);

                for (i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
                {
                    totalLine = totalLine + 1;

                    if (totalLine == 30 && pageNo == 1 || (totalLine == 35 && pageNo > 1))
                    {
                        pdfPage = pdfDocument.AddPage();
                        xGraphics = XGraphics.FromPdfPage(pdfPage);
                        totalLine = 0;
                        yPoint = 10;
                        pageNo++;
                    }

                    Date = Convert.ToDateTime(ds.Tables[0].Rows[i].ItemArray[1]).ToString("dd/MM/yyyy");
                    rate = Convert.ToInt32(ds.Tables[0].Rows[i].ItemArray[2]);
                    ExpectedAmount = 20;
                    Double TotalAmount = Convert.ToDouble(rate - ExpectedAmount);

                    if (TotalAmount < 0)
                    {
                        TotalAmount = 0;
                    }

                    ds.Tables[0].AcceptChanges();

                    TotalExpectedAmount += ExpectedAmount;
                    GrandTotalAmount += TotalAmount;
                    TotalOrderRate += rate;

                    xGraphics.DrawString(Date, xFontRegular, XBrushes.Black, new XRect(30, yPoint + 20, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);

                    xGraphics.DrawString(rate.ToString("0.00"), xFontRegular, XBrushes.Black, new XRect(-210, yPoint + 20, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopRight);

                    xGraphics.DrawString(ExpectedAmount.ToString("0.00"), xFontRegular, XBrushes.Black, new XRect(-130, yPoint + 20, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopRight);

                    xGraphics.DrawString(TotalAmount.ToString("0.00"), xFontRegular, XBrushes.Black, new XRect(-50, yPoint + 20, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopRight);

                    yPoint = yPoint + 20;

                }

                xGraphics.DrawString("Total", xFontBold, XBrushes.Black, new XRect(30, yPoint + 40, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);

                xGraphics.DrawString(TotalOrderRate.ToString("0.00"), xFontBold, XBrushes.Black, new XRect(-210, yPoint + 40, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopRight);

                xGraphics.DrawString(TotalExpectedAmount.ToString("0.00"), xFontBold, XBrushes.Black, new XRect(-130, yPoint + 40, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopRight);

                xGraphics.DrawString(GrandTotalAmount.ToString("0.00"), xFontBold, XBrushes.Black, new XRect(-50, yPoint + 40, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopRight);

                string fileName = "";
                var extenstion = ".pdf";

                fileName = "Order_Details_" + DateTime.Now.ToString("yyyyMMddHHmmss") + extenstion;

                var pathBuild = Path.Combine(Directory.GetCurrentDirectory(), "upload\\files");

                if (!Directory.Exists(pathBuild))
                {
                    Directory.CreateDirectory(pathBuild);
                }

                pdfDocument.Save(pathBuild + "\\" + fileName);

                var path = Path.Combine(Directory.GetCurrentDirectory(), "upload\\files", pathBuild + "\\" + fileName);
                var provider = new FileExtensionContentTypeProvider();

                if (!provider.TryGetContentType(path, out var ContentType))
                {
                    ContentType = "application/octet-stream";

                }
                var bytes = await System.IO.File.ReadAllBytesAsync(path);

                System.IO.File.Delete(path);

                return File(bytes, ContentType, Path.GetFileName(path));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        public async Task<IActionResult> MonthWiseData(ReportFilter report)
        {
            SqlConnection? connection;
            SqlCommand? command;
            SqlDataAdapter? adapter = new SqlDataAdapter();
            DataSet? ds = new DataSet();
            int i = 0;
            string sql = "";
            int yPoint = 0;
            string Name = "";
            string todayDate = "";
            Double AcceptedAmount = 0;
            Double TotalAmount = 0;
            Double FinalAmount = 0;
            Double Amount = 0;
            //Double GrandTotalAmount = 0;
            //Double TotalAccptedAmount = 0;
            //Double TotalOrderRate = 0;
            DateTime dtTodaydate = DateTime.Now;
            int totalLine = 0;
            int pageNo = 1;
            string? id;

            try
            {

                //if (Global.GetSession(HttpContext.Session.GetString(Global.SessionKeyName)) == false)
                //{
                //    return RedirectToAction(nameof(EmployeeModel), "Home");
                //}

                HttpContext.Session.Remove("SearchString");

                string MonthName = CultureInfo.CurrentCulture.DateTimeFormat.MonthNames[Convert.ToInt16(report.Month)] + "-" + report.Year;

                XFont xFontRegular = new XFont("Verdana", 10, XFontStyle.Regular);
                XFont xFontName = new XFont("Verdana", 10, XFontStyle.Bold);
                XFont xFontBold = new XFont("Verdana", 10, XFontStyle.Bold);
                XFont xCompBold = new XFont("Verdana", 12, XFontStyle.Bold);



                sql = "SELECT tbl_Employee.employeeId,tbl_Employee.FirstName +' ' + tbl_Employee.LastName AS EName, TodayDate, SUM(Amount) AS Amount " +
                        "FROM tbl_ItemTran, tbl_Employee " +
                        "WHERE MONTH(TodayDate) = '" + (Convert.ToInt16(report.Month) + 1) + "' AND YEAR(TodayDate) = '" + report.Year + "' " +
                        "AND tbl_Employee.employeeId = tbl_ItemTran.EmpID " +
                        "GROUP BY tbl_Employee.employeeId,tbl_Employee.FirstName, tbl_Employee.LastName,TodayDate " +
                        "ORDER BY tbl_Employee.FirstName, tbl_Employee.LastName ";


                connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
                connection.Open();
                command = new SqlCommand(sql, connection);
                adapter.SelectCommand = command;
                adapter.Fill(ds);
                connection.Close();

                command = null;
                adapter = null;
                connection = null;

                PdfDocument pdfDocument = new PdfDocument();
                // Create an empty page
                PdfPage pdfPage = pdfDocument.AddPage();
                // Get an XGraphics object for drawing
                XGraphics xGraphics = XGraphics.FromPdfPage(pdfPage);
                // Create a font                      
                XPen lineRed = new XPen(XColors.Black, 55);

                yPoint = yPoint + 100;

                todayDate = Convert.ToDateTime(DateTime.Now).ToString("dd/M/yyyy");

                //Draw Line
                double x = 80;
                XPen pen = XPens.Black;
                xGraphics.DrawLine(pen, x - 65, 60, 580, 60);
                xGraphics.DrawLine(pen, x - 65, yPoint - 10, 580, yPoint - 10);
                xGraphics.DrawLine(pen, x - 65, yPoint + 12, 580, yPoint + 12);

                //Header
                xGraphics.DrawString("Alita Infotech Pvt.Ltd", xCompBold, XBrushes.Black, new XRect(0, 30, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopCenter);
                xGraphics.DrawString("Report Date: " + todayDate, xFontBold, XBrushes.Black, new XRect(25, 70, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                xGraphics.DrawString(MonthName, xFontBold, XBrushes.Black, new XRect(-25, 70, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopRight);
                xGraphics.DrawString("Particular", xFontBold, XBrushes.Black, new XRect(-230, yPoint - 5, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopCenter);
                xGraphics.DrawString("Order", xFontBold, XBrushes.Black, new XRect(70, yPoint - 5, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopCenter);
                xGraphics.DrawString("Accepted ", xFontBold, XBrushes.Black, new XRect(150, yPoint - 5, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopCenter);
                xGraphics.DrawString("Amount", xFontBold, XBrushes.Black, new XRect(230, yPoint - 5, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopCenter);

                DataTable dtTable = new DataTable();
                dtTable.Columns.Add("Name", typeof(String));
                dtTable.Columns.Add("Amount", typeof(Double));
                dtTable.Columns.Add("Accepted", typeof(Double));
                dtTable.Columns.Add("TotalAmount", typeof(Double));

                DataView dtView = ds.Tables[0].DefaultView;

                AcceptedAmount = 20 * Convert.ToDouble(report.Day);

                for (i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
                {
                    if (Name != ds.Tables[0].Rows[i].ItemArray[1].ToString())
                    {
                        Name = ds.Tables[0].Rows[i].ItemArray[1].ToString();

                        dtView.RowFilter = "EName LIKE '%" + Name + "%'";

                        foreach (DataRowView drForm in dtView)
                        {
                            Amount = Amount + Convert.ToDouble(drForm["Amount"]);
                            TotalAmount = Convert.ToDouble(Amount - AcceptedAmount);

                            if (TotalAmount < 0)
                            {
                                TotalAmount = 0;
                            }

                            FinalAmount += TotalAmount;
                        }

                        DataRow row = dtTable.NewRow();
                        row["Name"] = Name;
                        row["Amount"] = Amount;
                        row["Accepted"] = AcceptedAmount;
                        row["TotalAmount"] = FinalAmount;
                        dtTable.Rows.Add(row);

                        //GrandTotalAmount += Amount;
                        //TotalAccptedAmount += AcceptedAmount;
                        //TotalOrderRate += FinalAmount;

                        Amount = 0;
                        TotalAmount = 0;
                        FinalAmount = 0;

                    }
                }

                for (i = 0; i <= dtTable.Rows.Count - 1; i++)
                {
                    totalLine = totalLine + 1;
                    AcceptedAmount = 0;
                    FinalAmount = 0;

                    if (totalLine == 30 && pageNo == 1 || (totalLine == 35 && pageNo > 1))
                    {
                        pdfPage = pdfDocument.AddPage();
                        xGraphics = XGraphics.FromPdfPage(pdfPage);
                        totalLine = 0;
                        yPoint = 10;
                        pageNo++;
                    }

                    Name = dtTable.Rows[i].ItemArray[0].ToString();
                    Amount = Convert.ToDouble(dtTable.Rows[i].ItemArray[1]);
                    AcceptedAmount = Convert.ToDouble(dtTable.Rows[i].ItemArray[2]);
                    FinalAmount = Convert.ToDouble(dtTable.Rows[i].ItemArray[3]);

                    xGraphics.DrawString(Name, xFontRegular, XBrushes.Black, new XRect(30, yPoint + 20, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    xGraphics.DrawString(Amount.ToString("0.00"), xFontRegular, XBrushes.Black, new XRect(-210, yPoint + 20, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopRight);
                    xGraphics.DrawString(AcceptedAmount.ToString("0.00"), xFontRegular, XBrushes.Black, new XRect(-130, yPoint + 20, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopRight);
                    xGraphics.DrawString(FinalAmount.ToString("0.00"), xFontRegular, XBrushes.Black, new XRect(-50, yPoint + 20, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopRight);

                    yPoint = yPoint + 20;

                }

                //xGraphics.DrawString("Total", xFontBold, XBrushes.Black, new XRect(30, yPoint + 40, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);

                //xGraphics.DrawString(GrandTotalAmount.ToString("0.00"), xFontBold, XBrushes.Black, new XRect(-210, yPoint + 40, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopRight);

                //xGraphics.DrawString(TotalAccptedAmount.ToString("0.00"), xFontBold, XBrushes.Black, new XRect(-130, yPoint + 40, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopRight);

                //xGraphics.DrawString(TotalOrderRate.ToString("0.00"), xFontBold, XBrushes.Black, new XRect(-50, yPoint + 40, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopRight);

                string fileName = "";
                var extenstion = ".pdf";

                fileName = "Month_Order_Details_" + DateTime.Now.ToString("yyyyMMddHHmmss") + extenstion;

                var pathBuild = Path.Combine(Directory.GetCurrentDirectory(), "upload\\files");

                if (!Directory.Exists(pathBuild))
                {
                    Directory.CreateDirectory(pathBuild);
                }

                pdfDocument.Save(pathBuild + "\\" + fileName);

                var path = Path.Combine(Directory.GetCurrentDirectory(), "upload\\files", pathBuild + "\\" + fileName);
                var provider = new FileExtensionContentTypeProvider();

                if (!provider.TryGetContentType(path, out var ContentType))
                {
                    ContentType = "application/octet-stream";

                }
                var bytes = await System.IO.File.ReadAllBytesAsync(path);

                System.IO.File.Delete(path);

                return File(bytes, ContentType, Path.GetFileName(path));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        static void CheckDuplicateRecords(DataTable dataTable)
        {
            Hashtable hTable = new Hashtable();
            ArrayList duplicateList = new ArrayList();
            foreach (DataRow drow in dataTable.Rows)
            {
                if (hTable.Contains(drow["employeeName"] + "" + drow["ItemName"]))
                    duplicateList.Add(drow);
                else
                    hTable.Add(drow["employeeName"] + "" + drow["ItemName"], String.Empty);
            }

            foreach (DataRow dRow in duplicateList)
            {
                string find = "ItemName = '" + dRow.ItemArray[2].ToString() + "'";

                DataRow[] resultupdate = dataTable.Select(find);
                resultupdate[0]["Quantiy"] = Convert.ToInt32(resultupdate[0]["Quantiy"].ToString()) + Convert.ToInt32(dRow.ItemArray[3].ToString());
                dataTable.AcceptChanges();
                dataTable.Rows.Remove(dRow);
            }
        }
    }
}
