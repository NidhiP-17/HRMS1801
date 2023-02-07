using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;
using HRMS.Models;
using Newtonsoft.Json;

namespace HRMS.Controllers
{
    public class ReportController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        private readonly IConfiguration configuration;
        List<Item>? mlstItem;
        string strSource = "ItemOrder";
        public ReportController(IConfiguration config)
        {
            configuration = config;
        }


        public JsonResult TodayReport()
        {
            SqlConnection? connection;
            SqlCommand? command;
            SqlDataAdapter? adapter = new SqlDataAdapter();
            DataSet? ds = new DataSet();
            DataTable dt = new DataTable();
            string sql = "";
            DateTime dtTodaydate = DateTime.Now;

            try
            {
                HttpContext.Session.Remove("SearchString");


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
                dt = ds.Tables[0];
                return new JsonResult(JsonConvert.SerializeObject(dt));

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        public async Task<IActionResult> PrintTodayReport()
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

    }
}
