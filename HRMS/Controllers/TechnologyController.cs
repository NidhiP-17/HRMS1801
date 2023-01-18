using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Models;
using Repositories;
using WebSite.Common;

namespace HRMS.Controllers
{
    [SessionTimeoutAttribute]
    public class TechnologyController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult CreateTechnology(string item)
        {
            string msg = "";
            TechnologyRepository repository = new TechnologyRepository();
            var response = repository.CreateTechnology(item, ViewBag.userId);
            if (response.Message == "Success")
            {
                TempData["msg"] = "Technology Saved Successfully";
                var response2 = repository.GetTechnologyList(ViewBag.userId, out msg);
                ViewBag.Technology = new SelectList(response2.Response, "technologyId", "Name");
                return new JsonResult(response.Message);
            }
            else
            {
                ModelState.AddModelError("Name", response.Message);
                return new JsonResult(response.Response);
            }
        }

    }
}
