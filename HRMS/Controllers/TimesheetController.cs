using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Build.Evaluation;
using Microsoft.CodeAnalysis;
using Models;
using Newtonsoft.Json;
using NuGet.Protocol.Core.Types;
using NuGet.Protocol.Plugins;
using Repositories;
using System.Threading.Tasks;
using WebSite.Common;

namespace HRMS.Controllers
{
    [SessionTimeoutAttribute]
    public class TimesheetController : Controller
    {

        long loginedempId = 0;
        [HttpGet]
        public IActionResult Index(string fd, string td, string pId, string eId)
        {
            string mesg = "";
            ViewBag.StateEnabled = false;
            ProjectRepository project = new ProjectRepository();
            var response1 = project.ListProjects(ViewBag.userId, out mesg);
            ViewBag.Projects = new SelectList(response1.Response, "projectId", "projectName");

            ViewBag.fromdaterange = fd;
            ViewBag.todaterange = td;

            EmployeeRepository employee = new EmployeeRepository();
            var response2 = employee.ListEmployees(ViewBag.userId, out mesg);
            if (ViewBag.userTypeId != 3)
            {

                ViewBag.Employees = new SelectList(response2.Response, "employeeId", "firstName");
            }
            else if (eId == null || ViewBag.userTypeId == 3)
            {
                ViewBag.Employees = new SelectList(response2.Response, "employeeId", "firstName", ViewBag.userId);
                loginedempId = ViewBag.userId;
                eId = ViewBag.userId.ToString();
            }

            TaskRepository task = new TaskRepository();
            var responsetask = task.ListTasks(0, ViewBag.userId, out mesg);
            ViewBag.Tasks = new SelectList(responsetask.Response, "taskId", "taskName");

            TimesheetRepository timesheet = new TimesheetRepository();
            var response3 = timesheet.ListReasons(ViewBag.userId, out mesg);
            ViewBag.Reasons = new SelectList(response3.Response, "reasonId", "reasonCode");

            TimesheetRepository repository = new TimesheetRepository();
            if (TempData["msg"] != null)
                ViewBag.msg = TempData["msg"].ToString();

            var now = DateTime.Now; // get the current DateTime 

            //Get the number of days in the current month
            //int daysInMonth = DateTime.DaysInMonth(now.Year, now.Month);

            //First day of the month is always 1
            var firstDay = now;
            //var firstDay = new DateTime(now.Year, now.Month, 1);

            //Last day will be similar to the number of days calculated above
            var lastDay = now;
            //var lastDay = new DateTime(now.Year, now.Month, daysInMonth);
            string startDate;
            string endDate;
            if (fd != null && td != null)
            {
                startDate = fd;
                endDate = td;
            }
            else
            {
                startDate = firstDay.ToShortDateString();
                endDate = lastDay.ToShortDateString();
            }
            if (eId == null)
                eId = "ALL";

            if (pId == null)
                pId = "ALL";
            var response = repository.GetTimesheet(pId, eId, startDate, endDate, ViewBag.userId, out mesg);

            //ViewBag.TimesheetList = response.Response;
            if (pId.Equals("0") == false || pId.Equals("ALL") == false)
                ViewBag.Projects = new SelectList(response1.Response, "projectId", "projectName", pId);
            if (eId.Equals("0") == false && eId.Equals("ALL") == false)
            {
                ViewBag.Employees = new SelectList(response2.Response, "employeeId", "firstName", eId);
            }

            TimesheetModel t = new TimesheetModel();
            if (eId != "ALL")
                t.employeeId = Convert.ToInt32(eId);
            else
                t.employeeId = 0;
            if (pId != "ALL")
                t.projectId = Convert.ToInt32(pId);
            else
                t.projectId = 0;
            t.taskId = 0;
            t.TimeSheetList = response.Response;
            //t.daterange = ViewBag.daterange;
          
            return View(t);
            //return View(response.Response);
        }


        public IActionResult Create()
        {
            string msg = "";

            ProjectRepository project = new ProjectRepository();
            var response1 = project.ListProjects(ViewBag.userId, out msg);
            ViewBag.Projects = new SelectList(response1.Response, "projectId", "projectName");

            EmployeeRepository employee = new EmployeeRepository();
            var response4 = employee.ListEmployees(ViewBag.userId, out msg);
            ViewBag.Employees = new SelectList(response4.Response, "employeeId", "firstName");

            TaskRepository task = new TaskRepository();
            var response2 = task.ListTasks(0, ViewBag.userId, out msg);
            ViewBag.Tasks = new SelectList(response2.Response, "taskId", "taskName");

            TimesheetRepository timesheet = new TimesheetRepository();
            var response3 = timesheet.ListReasons(ViewBag.userId, out msg);
            ViewBag.Reasons = new SelectList(response3.Response, "reasonId", "reasonCode");

            return View("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TimesheetModel timesheet)
        {
            String mesg = "";
            string startDate;
            string endDate;
            var now = DateTime.Now;
            var firstDay = now;
            var lastDay = now;
            string employeeId = "";
            string projectId = "";
            if (timesheet.daterange != null)
            {
                startDate = timesheet.daterange.Split(" - ")[0];
                endDate = timesheet.daterange.Split(" - ")[1];
            }
            else
            {
                startDate = firstDay.ToShortDateString();
                endDate = lastDay.ToShortDateString();
            }
            if (timesheet.employeeId == null)
                employeeId = "ALL";

            if (timesheet.projectId == null)
                projectId = "ALL";

            TimesheetRepository repositorys = new TimesheetRepository();
            var responses = repositorys.GetTimesheet(projectId, employeeId, startDate, endDate, ViewBag.userId, out mesg);

            timesheet.TimeSheetList = responses.Response;

            if (ModelState.IsValid)
            {
                if (timesheet.employeeId != 0 && timesheet.projectId == 0 && timesheet.reasonCode == "0")
                {
                    string msg = "";
                    ProjectRepository project = new ProjectRepository();
                    var response1 = project.ListProjects(ViewBag.userId, out msg);
                    ViewBag.Projects = new SelectList(response1.Response, "projectId", "projectName", timesheet.projectId);

                    EmployeeRepository employee = new EmployeeRepository();
                    var response4 = employee.ListEmployees(ViewBag.userId, out msg);
                    ViewBag.Employees = new SelectList(response4.Response, "employeeId", "firstName", timesheet.employeeId);

                    TaskRepository task = new TaskRepository();
                    var response2 = task.ListTasks(0, ViewBag.userId, out msg);
                    ViewBag.Tasks = new SelectList(response2.Response, "taskId", "taskName", timesheet.taskId);

                    TimesheetRepository timesheets = new TimesheetRepository();
                    var response3 = timesheets.ListReasons(ViewBag.userId, out msg);
                    ViewBag.Reasons = new SelectList(response3.Response, "reasonId", "reasonCode", timesheet.reasonCode);

                    ModelState.AddModelError("projectId", "Select Project OR Leave Reason");
                    return View("Index", timesheet);
                }
                else
                {
                    TimesheetRepository repository = new TimesheetRepository();
                    if (timesheet.isWorkingDay == "true")
                    {
                        var res = repository.GetWorkingHours(timesheet.date.ToString());
                        if (res.Message != "Not Exists")
                        {
                            decimal r = Convert.ToDecimal(res.Message);
                            if (timesheet.hours > r)
                            {
                                string msg = "";
                                ProjectRepository project = new ProjectRepository();
                                var response1 = project.ListProjects(ViewBag.userId, out msg);
                                ViewBag.Projects = new SelectList(response1.Response, "projectId", "projectName");

                                EmployeeRepository employee = new EmployeeRepository();
                                var response4 = employee.ListEmployees(ViewBag.userId, out msg);
                                ViewBag.Employees = new SelectList(response4.Response, "employeeId", "firstName");

                                TaskRepository task = new TaskRepository();
                                var response2 = task.ListTasks(0, ViewBag.userId, out msg);
                                ViewBag.Tasks = new SelectList(response2.Response, "taskId", "taskName");

                                TimesheetRepository timesheets = new TimesheetRepository();
                                var response3 = timesheets.ListReasons(ViewBag.userId, out msg);
                                ViewBag.Reasons = new SelectList(response3.Response, "reasonId", "reasonCode", timesheet.reasonCode);

                                ModelState.AddModelError("hours", "Time Limit Exceed. You can add till " + r + " hours");
                                return View("Index", timesheet);
                            }
                            else
                            {
                                string? t = string.Empty;
                                if (timesheet.reasonCode != "0")
                                    t = "Leave";
                                else
                                    t = "WorkingHours";
                                decimal? h = null;
                                if (t == "WorkingHours")
                                    h = timesheet.hours;
                                else if (t == "Leave")
                                    h = timesheet.leavehours;
                                var rest = repository.GetTotalHoursOfDay((int)timesheet.employeeId, (decimal)h, timesheet.date.ToString());
                                if (rest.Message == "Success")
                                {
                                    var response = repository.CreateTimesheet(timesheet, ViewBag.userId);
                                    if (response.Message == "Success")
                                    {
                                        TempData["msg"] = "Timesheet Saved Successfully";
                                        return RedirectToAction("Index");
                                    }
                                    else
                                    {
                                        string msg = "";
                                        ProjectRepository project = new ProjectRepository();
                                        var response1 = project.ListProjects(ViewBag.userId, out msg);
                                        ViewBag.Projects = new SelectList(response1.Response, "projectId", "projectName");

                                        EmployeeRepository employee = new EmployeeRepository();
                                        var response4 = employee.ListEmployees(ViewBag.userId, out msg);
                                        ViewBag.Employees = new SelectList(response4.Response, "employeeId", "firstName");

                                        TaskRepository task = new TaskRepository();
                                        var response2 = task.ListTasks(0, ViewBag.userId, out msg);
                                        ViewBag.Tasks = new SelectList(response2.Response, "taskId", "taskName");

                                        TimesheetRepository timesheets = new TimesheetRepository();
                                        var response3 = timesheets.ListReasons(ViewBag.userId, out msg);
                                        ViewBag.Reasons = new SelectList(response3.Response, "reasonId", "reasonCode", timesheet.reasonCode);

                                        ModelState.AddModelError("TaskName", response.Message);
                                        return View("Index", timesheet);
                                    }
                                }
                                else
                                {
                                    string msg = "";
                                    ProjectRepository project = new ProjectRepository();
                                    var response1 = project.ListProjects(ViewBag.userId, out msg);
                                    ViewBag.Projects = new SelectList(response1.Response, "projectId", "projectName", timesheet.projectId);

                                    EmployeeRepository employee = new EmployeeRepository();
                                    var response4 = employee.ListEmployees(ViewBag.userId, out msg);
                                    ViewBag.Employees = new SelectList(response4.Response, "employeeId", "firstName", timesheet.employeeId);

                                    TaskRepository task = new TaskRepository();
                                    var response2 = task.ListTasks(0, ViewBag.userId, out msg);
                                    ViewBag.Tasks = new SelectList(response2.Response, "taskId", "taskName", timesheet.taskId);

                                    TimesheetRepository timesheets = new TimesheetRepository();
                                    var response3 = timesheets.ListReasons(ViewBag.userId, out msg);
                                    ViewBag.Reasons = new SelectList(response3.Response, "reasonId", "reasonCode", timesheet.reasonCode);

                                    if (t == "WorkingHours")
                                        ModelState.AddModelError("hours", rest.Message);
                                    else if (t == "Leave")
                                        ModelState.AddModelError("leavehours", rest.Message);
                                    return View("Index", timesheet);
                                }

                            }
                        }
                    }
                    else
                    {
                        string? t = string.Empty;
                        if (timesheet.reasonCode != "0")
                            t = "Leave";
                        else
                            t = "WorkingHours";
                        decimal? h = null;
                        if (t == "WorkingHours")
                            h = timesheet.hours;
                        else if (t == "Leave")
                            h = timesheet.leavehours;
                        var rest = repository.GetTotalHoursOfDay((int)timesheet.employeeId, (decimal)h, timesheet.date.ToString());
                        if (rest.Message == "Success")
                        {
                            var response = repository.CreateTimesheet(timesheet, ViewBag.userId);
                            if (response.Message == "Success")
                            {
                                TempData["msg"] = "Timesheet Saved Successfully";
                                return RedirectToAction("Index");
                            }
                            else
                            {
                                string msg = "";
                                ProjectRepository project = new ProjectRepository();
                                var response1 = project.ListProjects(ViewBag.userId, out msg);
                                ViewBag.Projects = new SelectList(response1.Response, "projectId", "projectName");

                                EmployeeRepository employee = new EmployeeRepository();
                                var response4 = employee.ListEmployees(ViewBag.userId, out msg);
                                ViewBag.Employees = new SelectList(response4.Response, "employeeId", "firstName");

                                TaskRepository task = new TaskRepository();
                                var response2 = task.ListTasks(0, ViewBag.userId, out msg);
                                ViewBag.Tasks = new SelectList(response2.Response, "taskId", "taskName");

                                TimesheetRepository timesheets = new TimesheetRepository();
                                var response3 = timesheets.ListReasons(ViewBag.userId, out msg);
                                ViewBag.Reasons = new SelectList(response3.Response, "reasonId", "reasonCode", timesheet.reasonCode);

                                ModelState.AddModelError("TaskName", response.Message);
                                return View("Index", timesheet);
                            }
                        }
                        else
                        {
                            string msg = "";
                            ProjectRepository project = new ProjectRepository();
                            var response1 = project.ListProjects(ViewBag.userId, out msg);
                            ViewBag.Projects = new SelectList(response1.Response, "projectId", "projectName", timesheet.projectId);

                            EmployeeRepository employee = new EmployeeRepository();
                            var response4 = employee.ListEmployees(ViewBag.userId, out msg);
                            ViewBag.Employees = new SelectList(response4.Response, "employeeId", "firstName", timesheet.employeeId);

                            TaskRepository task = new TaskRepository();
                            var response2 = task.ListTasks(0, ViewBag.userId, out msg);
                            ViewBag.Tasks = new SelectList(response2.Response, "taskId", "taskName", timesheet.taskId);

                            TimesheetRepository timesheets = new TimesheetRepository();
                            var response3 = timesheets.ListReasons(ViewBag.userId, out msg);
                            ViewBag.Reasons = new SelectList(response3.Response, "reasonId", "reasonCode", timesheet.reasonCode);
                            if (t == "WorkingHours")
                                ModelState.AddModelError("hours", rest.Message);
                            else if (t == "Leave")
                                ModelState.AddModelError("leavehours", rest.Message);
                            return View("Index", timesheet);
                        }
                    }

                }
            }

            string msg1 = "";
            ProjectRepository project1 = new ProjectRepository();
            var response11 = project1.ListProjects(ViewBag.userId, out msg1);
            ViewBag.Projects = new SelectList(response11.Response, "projectId", "projectName", timesheet.projectId);

            EmployeeRepository employee1 = new EmployeeRepository();
            var response41 = employee1.ListEmployees(ViewBag.userId, out msg1);
            ViewBag.Employees = new SelectList(response41.Response, "employeeId", "firstName", timesheet.employeeId);

            TaskRepository task1 = new TaskRepository();
            var response21 = task1.ListTasks(0, ViewBag.userId, out msg1);
            ViewBag.Tasks = new SelectList(response21.Response, "taskId", "taskName", timesheet.taskId);

            TimesheetRepository timesheet1 = new TimesheetRepository();
            var response31 = timesheet1.ListReasons(ViewBag.userId, out msg1);
            ViewBag.Reasons = new SelectList(response31.Response, "reasonId", "reasonCode", timesheet.reasonCode);

            return View("Index",timesheet);
        }
        public IActionResult Delete(int id)
        {
            TimesheetRepository repository = new TimesheetRepository();
            repository.DeleteTimesheet(id, ViewBag.userId);
            TempData["msg"] = "Task Deleted Successfully";
            return RedirectToAction("Index");
        }
        public ViewResult Details(int id)
        {
            TimesheetRepository repository = new TimesheetRepository();
            var response = repository.GetTimesheetById(id);
            return View(response.Response);
        }
        [HttpGet]
        public JsonResult GetTask(int projectId)
        {
            TaskRepository material = new TaskRepository();
            var response = material.GetTaskByProjectId(projectId);
            return new JsonResult(JsonConvert.SerializeObject(response));
        }

        public JsonResult HolidayList()
        {
            string msg = "";
            HolidayRepository holiday = new HolidayRepository();
            var response = holiday.ListHolidays(ViewBag.userId, out msg);
            return new JsonResult(JsonConvert.SerializeObject(response.Response));
        }
        public JsonResult ShowTotalhours(long employeeId, DateTime date)
        {
            string msg = "";
            TimesheetRepository timesheet = new TimesheetRepository();
            long empid = 0;
            if (employeeId == 0)
                empid = ViewBag.userId;
            else
                empid = employeeId;
            var response = timesheet.GetTotalHours(empid, date);
            ViewBag.totalhours = response.Response;
            return new JsonResult(JsonConvert.SerializeObject(response.Response));
        }
        public JsonResult GetRemainingDays(int employeeId)
        {
            TimesheetRepository timesheet = new TimesheetRepository();
            var response = timesheet.GetRemainingDays(employeeId);
            return new JsonResult(JsonConvert.SerializeObject(response.Response));
        }

        public JsonResult GetJoiningDate(int employeeId)
        {
            TimesheetRepository timesheet = new TimesheetRepository();
            var response = timesheet.GetJoiningDate(employeeId);
            return new JsonResult(JsonConvert.SerializeObject(response.Response));
        }

        public JsonResult GetLastDayOftimeSheet(int employeeId)
        {
            TimesheetRepository timesheet = new TimesheetRepository();
            var response = timesheet.GetLastDayOftimeSheet(employeeId);
            return new JsonResult(JsonConvert.SerializeObject(response.Response));
        }
        public JsonResult CheckTimesheet(string date,int id)
        {
            TimesheetRepository timesheet = new TimesheetRepository();
            var response = timesheet.CheckTimesheet(id,date);
            return new JsonResult(JsonConvert.SerializeObject(response.Response));
        }
        public JsonResult GetLaunchDate()
        {
            LaunchDateRepository timesheet = new LaunchDateRepository();
            var response = timesheet.GetLaunchDate();
            return new JsonResult(JsonConvert.SerializeObject(response.Response));
        }
    }
}
