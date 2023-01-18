using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Models;
using Models.ResponseModel;
using Repositories;
using WebSite.Common;

namespace HRMS.Controllers
{
    [SessionTimeoutAttribute]
    public class EmployeeController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        public EmployeeController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            EmployeeRepository repository = new EmployeeRepository();
            string msg = "";
            if (TempData["msg"] != null)
                ViewBag.msg = TempData["msg"].ToString();
            var response = repository.GetList(ViewBag.userId, out msg);
            return View(response.Response);
        }
        public ViewResult Create()
        {
            EmployeeRepository usertype = new EmployeeRepository();
            var response1 = usertype.ListUserTypes();
            ViewBag.userTypes = new SelectList(response1.Response, "userTypeId", "userType");

            EmployeeRepository employeeType = new EmployeeRepository();
            var response2 = employeeType.ListEmployeeTypes();
            ViewBag.employeeType = new SelectList(response2.Response, "employeeTypeId", "employeeType");

            EmployeeRepository status = new EmployeeRepository();
            var response3 = status.ListStatus();
            ViewBag.status = new SelectList(response3.Response, "status", "status");

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(EmployeeModel task)
        {
            if (ModelState.IsValid)
            {
                EmployeeRepository repository = new EmployeeRepository();
                if (task.employeeImage != null)
                {
                    string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/EmployeeDocs");

                    //create folder if not exist
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);

                    //get file extension
                    FileInfo fileInfo = new FileInfo(task.employeeImage.FileName);
                    string fileName = task.employeeImage.FileName;

                    string fileNameWithPath = Path.Combine(path, fileName);

                    using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                    {
                        task.employeeImage.CopyTo(stream);
                    }
                    task.employeeImageName = task.employeeImage.FileName;
                }
                if (task.aadharImage != null)
                {
                    string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/EmployeeDocs");

                    //create folder if not exist
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);

                    //get file extension
                    FileInfo fileInfo = new FileInfo(task.aadharImage.FileName);
                    string fileName = task.aadharImage.FileName;

                    string fileNameWithPath = Path.Combine(path, fileName);

                    using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                    {
                        task.aadharImage.CopyTo(stream);
                    }
                    task.aadharImageName = task.aadharImage.FileName;
                }
                if (task.panImage != null)
                {
                    string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/EmployeeDocs");

                    //create folder if not exist
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);

                    //get file extension
                    FileInfo fileInfo = new FileInfo(task.panImage.FileName);
                    string fileName = task.panImage.FileName;

                    string fileNameWithPath = Path.Combine(path, fileName);

                    using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                    {
                        task.panImage.CopyTo(stream);
                    }
                    task.panImageName = task.panImage.FileName;
                }
                
                var response = repository.CreateEmployee(task, ViewBag.userId);
                if (response.Status == Constants.WebApiStatusOk)
                {
                    TempData["msg"] = "Employee Saved Successfully";
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("EmployeeName", response.Message);
                    return View("Create", task);
                }
            }
            EmployeeRepository usertype = new EmployeeRepository();
            var response1 = usertype.ListUserTypes();
            ViewBag.userTypes = new SelectList(response1.Response, "userTypeId", "userType");

            EmployeeRepository employeeType = new EmployeeRepository();
            var response2 = employeeType.ListEmployeeTypes();
            ViewBag.employeeType = new SelectList(response2.Response, "employeeTypeId", "employeeType");

            EmployeeRepository status = new EmployeeRepository();
            var response3 = status.ListStatus();
            ViewBag.status = new SelectList(response3.Response, "status", "status");

            return View(task);
        }
        public IActionResult Edit(int id)
        {
            EmployeeRepository repository = new EmployeeRepository();
            var result = repository.GetDetail(id);

            EmployeeRepository usertype = new EmployeeRepository();
            var response1 = usertype.GetUserTypes();
            ViewBag.userTypes = new SelectList(response1.Response, "userTypeId", "userType",result.Response.userTypeId);

            EmployeeRepository employeeType = new EmployeeRepository();
            var response2 = employeeType.GetEmployeeTypes();
            ViewBag.employeeType = new SelectList(response2.Response, "employeeTypeId", "employeeType", result.Response.employeeTypeId);

            EmployeeRepository status = new EmployeeRepository();
            var response3 = status.GetStatus();
            ViewBag.status = new SelectList(response3.Response, "status", "status", result.Response.status);

            return View(result.Response);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(EmployeeModel task)
        {
            if (task.employeeImage != null)
            {
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/EmployeeDocs");

                //create folder if not exist
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                //get file extension
                FileInfo fileInfo = new FileInfo(task.employeeImage.FileName);
                string fileName = task.employeeImage.FileName;

                string fileNameWithPath = Path.Combine(path, fileName);

                using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                {
                    task.employeeImage.CopyTo(stream);
                }
                if (System.IO.File.Exists(Path.Combine(path, task.employeeImage.ToString())))
                    System.IO.File.Delete(task.employeeImageName);
                task.employeeImageName = task.employeeImage.FileName;
            }
            else
            {
                task.employeeImageName = task.employeeImageName;
                task.employeeImage = task.employeeImage;
            }
            if (task.aadharImage != null)
            {
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/EmployeeDocs");

                //create folder if not exist
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                //get file extension
                FileInfo fileInfo = new FileInfo(task.aadharImage.FileName);
                string fileName = task.aadharImage.FileName;

                string fileNameWithPath = Path.Combine(path, fileName);

                using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                {
                    task.aadharImage.CopyTo(stream);
                }
                if (System.IO.File.Exists(Path.Combine(path, task.aadharImageName.ToString())))
                    System.IO.File.Delete(task.aadharImageName);
                task.aadharImageName = task.aadharImage.FileName;
            }
            else
            {
                task.aadharImageName = task.aadharImageName;
                task.aadharImage = task.aadharImage;
            }
            if (task.panImage != null)
            {
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/EmployeeDocs");

                //create folder if not exist
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                //get file extension
                FileInfo fileInfo = new FileInfo(task.panImage.FileName);
                string fileName = task.panImage.FileName;

                string fileNameWithPath = Path.Combine(path, fileName);

                using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                {
                    task.panImage.CopyTo(stream);
                }
                if (System.IO.File.Exists(Path.Combine(path, task.panImageName.ToString())))
                    System.IO.File.Delete(task.panImageName);
                task.panImageName = task.panImage.FileName;
            }
            else
            {
                task.panImageName = task.panImageName;
                task.panImage = task.panImage;
            }
            if (ModelState.IsValid)
            {
                EmployeeRepository repository = new EmployeeRepository();
                var response = repository.UpdateEmployee(task, ViewBag.userId);
                if (response.Status == Constants.WebApiStatusOk)
                {
                    TempData["msg"] = "Employee Saved Successfully";
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("EmployeeName", response.Message);
                    return View("Edit", task);
                }
            }
            return View("Edit", task);
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            EmployeeRepository repository = new EmployeeRepository();
            repository.DeleteEmployee(ViewBag.userId);
            return Json(new { result = "Success", msg = "Employee deleted" });
            //return RedirectToAction("Index");
        }
        public ViewResult Details(int id)
        {
            EmployeeRepository  repository = new EmployeeRepository();
            var response = repository.GetDetail(id);
            return View(response.Response);
        }
    }
}
