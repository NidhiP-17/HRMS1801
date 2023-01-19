using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Models;
using Repositories;
using WebSite.Common;

namespace HRMS.Controllers
{
    [SessionTimeoutAttribute]
    public class LaunchDateController : Controller
    {
        public IActionResult Index()
        {
            LaunchDateRepository repository = new LaunchDateRepository();
            if (TempData["msg"] != null)
                ViewBag.msg = TempData["msg"].ToString();
            var response = repository.GetLaunchDate();
            return View(response.Response);
        }
        public ViewResult Create()
        {
           return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(LaunchDateModel item)
        {
            if (ModelState.IsValid)
            {
                LaunchDateRepository repository = new LaunchDateRepository();
            
                var response = repository.Create(item, ViewBag.userId);
                if (response.Message == "Success")
                {
                    TempData["msg"] = "Launch Date Successfully";
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("launchdate", response.Message);
                    return View("Create", item);
                }
            }
            return View(item);
        }
    }
}
