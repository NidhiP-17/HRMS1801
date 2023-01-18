using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Models;
using Newtonsoft.Json;
using NuGet.Protocol.Core.Types;
using Repositories;
using WebSite.Common;

namespace HRMS.Controllers
{
    [SessionTimeoutAttribute]
    public class ProjectController : Controller
    {
        public IActionResult Index()
        {
            ProjectRepository repository = new ProjectRepository();
            string msg = "";
            if (TempData["msg"] != null)
                ViewBag.msg = TempData["msg"].ToString();
            var response = repository.GetList(ViewBag.userId, out msg);
            return View(response.Response);
        }
        public ViewResult Create()
        {
            string msg = "";
            TechnologyRepository repository = new TechnologyRepository();
            var response2 = repository.GetTechnologyList(ViewBag.userId, out msg);
            ViewBag.Technology = new SelectList(response2.Response, "technologyId", "Name");

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ProjectModel project)
        {
            //string[] tech_str_comma = project.technologies;
            //tech_str_comma = tech_str_comma[0].Split(",");
            //for (var j = 0; j < tech_str_comma.Length; j++)
            //{
                //project.technology = tech_str_comma[j];

                if (ModelState.IsValid)
                {
                    ProjectRepository repository = new ProjectRepository();
                    var response = repository.Create(project, ViewBag.userId);
                    if (response.Message == "Success")
                    {
                        TempData["msg"] = "Project Saved Successfully";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("ProjectName", response.Message);
                        return View("Create", project);
                    }
                }
            //}
            return View(project);
        }
        public IActionResult Edit(int id)
        {
            string msg = "";
            TechnologyRepository repository = new TechnologyRepository();
            var response2 = repository.GetTechnologyList(ViewBag.userId, out msg);
            
            ProjectRepository repository1 = new ProjectRepository();
            var result = repository1.GetDetail(id);
            ViewBag.Technology = new SelectList(response2.Response, "technologyId", "Name",result.Response.technology);

            EmployeeRepository status = new EmployeeRepository();
            var response3 = status.ListStatus();
            ViewBag.status = new SelectList(response3.Response, "status", "status", result.Response.status);
            return View(result.Response);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ProjectModel detail)
        {
            if (ModelState.IsValid)
            {
                ProjectRepository repository = new ProjectRepository();

                var response = repository.Update(detail, ViewBag.userId);
                if (response.Message == "Success")
                {
                    TempData["msg"] = "Project Saved Successfully";
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("ProjectName", response.Message);
                    return View("Edit", detail);
                }
            }
            return View("Edit", detail);
        }      
        public IActionResult Delete(int id)
        {
            ProjectRepository repository = new ProjectRepository();
            repository.Delete(id, ViewBag.userId);
            TempData["msg"] = "Project Deleted Successfully";
            //return Json(new { result = "Success", msg = "Project deleted" });
            return RedirectToAction("Index");
        }
        public ViewResult Details(int id)
        {
            ProjectRepository repository = new ProjectRepository();
            var response = repository.GetDetail(id);
            return View(response.Response);
        }

        public JsonResult GetTechnology()
        {
            string msg = "";
            TechnologyRepository repository = new TechnologyRepository();
            var response= repository.GetTechnologyList(ViewBag.userId, out msg);
            return new JsonResult(JsonConvert.SerializeObject(response.Response));
        }

    }
}
