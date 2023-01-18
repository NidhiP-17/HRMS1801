using DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Models;
using Repositories;
using System.Data;
using WebSite.Common;

namespace HRMS.Controllers
{
    [SessionTimeoutAttribute]
    public class ItemCategoryController : Controller
    {
        public IActionResult Index()
        {
            ItemCategoryRepository repository = new ItemCategoryRepository();
            string msg = "";
            if (TempData["msg"] != null)
                ViewBag.msg = TempData["msg"].ToString();
            var response = repository.GetList(ViewBag.userId, out msg);
            return View(response.Response);
        }
        public ViewResult Create()
        {
           return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ItemCategoryModel item)
        {
            if (ModelState.IsValid)
            {
                ItemCategoryRepository repository = new ItemCategoryRepository();
                var response = repository.Create(item, ViewBag.userId);
                if (response.Message == "Success")
                {
                    TempData["msg"] = "Item category Saved Successfully";
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("ItemCatName", response.Message);
                    return View("Create", item);
                }
            }
            return View(item);
        }
        public IActionResult Edit(int id)
        {
            ItemCategoryRepository repository = new ItemCategoryRepository();
            var result = repository.GetCategoryById(id);
            return View(result.Response);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ItemCategoryModel item)
        {
            if (ModelState.IsValid)
            {
                ItemCategoryRepository repository = new ItemCategoryRepository();

                var response = repository.Update(item, ViewBag.userId);
                if (response.Message == "Success")
                {
                    TempData["msg"] = "Item Category Saved Successfully";
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
        public IActionResult Delete(int id)
        {
            ItemCategoryRepository repository = new ItemCategoryRepository();
            repository.Delete(id, ViewBag.userId);
            TempData["msg"] = "Item Deleted Successfully";
            return RedirectToAction("Index");
        }
        public ViewResult Details(int id)
        {
            ItemCategoryRepository repository = new ItemCategoryRepository();
            var response = repository.GetCategoryById(id);
            return View(response.Response);
        }

        [HttpPost]
        public IActionResult EditStatus(int id, bool status)
        {
            if (ModelState.IsValid)
            {
                ItemCategoryRepository repository = new ItemCategoryRepository();

                var response = repository.UpdateStatus(id, status, ViewBag.userId);
                if (response.Message == "Success")
                {
                    TempData["msg"] = "Item Category Changed Successfully";
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
    }
}
