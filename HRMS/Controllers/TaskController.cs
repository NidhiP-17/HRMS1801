using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Models;
using Repositories;
using WebSite.Common;

namespace HRMS.Controllers
{
    [SessionTimeoutAttribute]
    public class TaskController : Controller
    {
        public IActionResult Index(int id)
        {
            TaskRepository repository = new TaskRepository();
            string msg = "";

            ProjectRepository project = new ProjectRepository();
            var response1 = project.ListProjects(ViewBag.userId, out msg);
            ViewBag.Projects = new SelectList(response1.Response, "projectId", "projectName");

            if (TempData["msg"] != null)
                ViewBag.msg = TempData["msg"].ToString();
            var response = repository.GetList(id,ViewBag.userId, out msg);

            if (id.Equals("0") == false && id.Equals("ALL") == false)
                ViewBag.Projects = new SelectList(response1.Response, "projectId", "projectName", id);

            return View(response.Response);
        }
        public ViewResult Create()
        {
            ProjectRepository project = new ProjectRepository();
            string msg = "";
            var response1 = project.GetProjectList(ViewBag.userId, out msg);
            ViewBag.Projects = new SelectList(response1.Response, "projectId", "projectName");

            EmployeeRepository employee = new EmployeeRepository();
            var response2 = employee.GetList(ViewBag.userId, out msg);
            ViewBag.Employees = new SelectList(response2.Response, "employeeId", "firstName");
            //var response3 = employee.GetStatus();
            //ViewBag.Status = new SelectList(response3.Response, "statusId", "status");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TaskModel task)
        {
            if (ModelState.IsValid)
            {
                TaskRepository repository = new TaskRepository();

                var response = repository.Create(task, ViewBag.userId);
                if (response.Message == "Success")
                {
                    TempData["msg"] = "Task Saved Successfully";
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("TaskName", response.Message);
                    return View("Create", task);
                }
            }
            return View(task);
        }
        public IActionResult Edit(int id)
        {
            TaskRepository repository = new TaskRepository();
            var result = repository.GetDetail(id);

            ProjectRepository project = new ProjectRepository();
            string msg = "";
            var response1 = project.GetList(ViewBag.userId, out msg);
            ViewBag.Projects = new SelectList(response1.Response, "projectId", "projectName", result.Response.projectId);

            EmployeeRepository employee = new EmployeeRepository();
            var response2 = employee.GetList(ViewBag.userId, out msg);
            ViewBag.Employees = new SelectList(response2.Response, "employeeId", "firstName", result.Response.employeeId);

            //var response3 = employee.GetStatus();
            //ViewBag.Status = new SelectList(response3.Response, "status", "status", result.Response.status);

            return View(result.Response);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(TaskModel task)
        {
            if (ModelState.IsValid)
            {
                TaskRepository repository = new TaskRepository();

                var response = repository.Update(task, ViewBag.userId);
                if (response.Message == "Success")
                {
                    TempData["msg"] = "Task Saved Successfully";
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("TaskName", response.Message);
                    return View("Edit", task);
                }
            }
            return View("Edit", task);
        }     
        public IActionResult Delete(int id)
        {
            TaskRepository repository = new TaskRepository();
            repository.Delete(id, ViewBag.userId);
            TempData["msg"] = "Task Deleted Successfully";
            return RedirectToAction("Index");
        }
        public ViewResult Details(int id)
        {
            TaskRepository repository = new TaskRepository();
            var response = repository.GetDetail(id);
            return View(response.Response);
        }
    }
}
