using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Models;
using NuGet.Protocol.Core.Types;
using Repositories;
using System.Threading.Tasks;
using WebSite.Common;

namespace HRMS.Controllers
{
    [SessionTimeoutAttribute]
    public class ItemController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly IWebHostEnvironment _hostEnvironment;
        public ItemController(IConfiguration config, IWebHostEnvironment hostEnvironment)
        {
            configuration = config;
            this._hostEnvironment = hostEnvironment;
        }
        public IActionResult Index()
        {
            ItemRepository repository = new ItemRepository();
            string msg = "";
            if (TempData["msg"] != null)
                ViewBag.msg = TempData["msg"].ToString();
            var response = repository.GetList(ViewBag.userId, out msg);
            return View(response.Response);
        }
        public ViewResult Create()
        {
            ItemCategoryRepository project = new ItemCategoryRepository();
            string msg = "";
            var response1 = project.GetCategories(ViewBag.userId, out msg);
            ViewBag.Categories = new SelectList(response1.Response, "ItemCatID", "ItemCatName");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ItemModel item)
        {
            if (ModelState.IsValid)
            {
                ItemRepository repository = new ItemRepository();
                if (item.ImageFile != null)
                {
                    string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/itemImage");

                    //create folder if not exist
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);

                    //get file extension
                    FileInfo fileInfo = new FileInfo(item.ImageFile.FileName);
                    string fileName = item.ImageFile.FileName;

                    string fileNameWithPath = Path.Combine(path, fileName);

                    using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                    {
                        item.ImageFile.CopyTo(stream);
                    }
                    item.ImageName = item.ImageFile.FileName;
                }
                var response = repository.Create(item, ViewBag.userId);
                if (response.Message == "Success")
                {
                    TempData["msg"] = "Item Saved Successfully";
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("ItemName", response.Message);
                    return View("Create", item);
                }
            }
            return View(item);
        }
        public IActionResult Edit(int id)
        {
            ItemRepository repository = new ItemRepository();
            var result = repository.GetItemById(id);

            ItemCategoryRepository project = new ItemCategoryRepository();
            string msg = "";
            var response1 = project.GetCategories(ViewBag.userId, out msg);
            ViewBag.Categories = new SelectList(response1.Response, "ItemCatID", "ItemCatName", result.Response.ItemCatID);

            return View(result.Response);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ItemModel item)
        {
            if (item.ImageFile != null)
            {
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/itemImage");

                //create folder if not exist
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                //get file extension
                FileInfo fileInfo = new FileInfo(item.ImageFile.FileName);
                string fileName = item.ImageFile.FileName;

                string fileNameWithPath = Path.Combine(path, fileName);

                using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                {
                    item.ImageFile.CopyTo(stream);
                }
                if (item.ImageName != null)
                {
                    var filePath = Path.Combine(path, item.ImageName.ToString());
                    if (System.IO.File.Exists(Path.Combine(path, item.ImageName.ToString())))
                        System.IO.File.Delete(filePath);
                }
                else
                    item.ImageName = item.ImageFile.FileName;
            }
            else
            {
                item.ImageName = item.ImageName;
            }
            if (ModelState.IsValid)
            {
                ItemRepository repository = new ItemRepository();

                var response = repository.Update(item, ViewBag.userId);
                if (response.Message == "Success")
                {
                    TempData["msg"] = "Item Saved Successfully";
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("ItemName", response.Message);
                    return View("Edit", item);
                }
            }
            return View("Edit", item);
        }
        [HttpPost]
        public IActionResult EditStatus(int id, bool status)
        {
            if (ModelState.IsValid)
            {
                ItemRepository repository = new ItemRepository();

                var response = repository.UpdateStatus(id, status, ViewBag.userId);
                if (response.Message == "Success")
                {
                    TempData["msg"] = "Item Changed Successfully";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["msg"] = response.Message;
                    return View("Index");
                }
            }
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            ItemRepository repository = new ItemRepository();
            repository.Delete(id, ViewBag.userId);
            TempData["msg"] = "Item Deleted Successfully";
            return RedirectToAction("Index");
        }
        public ViewResult Details(int id)
        {
            ItemRepository repository = new ItemRepository();
            var response = repository.GetItemById(id);
            return View(response.Response);
        }
    }
}
