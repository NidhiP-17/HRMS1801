using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Models;
using Newtonsoft.Json;
using NuGet.Protocol.Core.Types;
using Repositories;
using WebSite.Common;

namespace HRMS.Controllers
{
    [SessionTimeoutAttribute]
    public class HolidayController : Controller
    {
        public IActionResult Index()
        {
            string mesg = string.Empty;
            HolidayRepository repository = new HolidayRepository();
            var response = repository.ListHolidaysandWorkingDays(ViewBag.userId, out mesg);
            return View(response.Response);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(HolidayModel holiday)
        {
            if (ModelState.IsValid)
            {
                HolidayRepository repository = new HolidayRepository();
                var response = repository.Create(holiday, ViewBag.userId);
                if (response.Status == Constants.WebApiStatusOk)
                {
                    TempData["msg"] = "Holiday Saved Successfully";
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("name", response.Message);
                    return View("Create", holiday);
                }
            }

            return View(holiday);
        }
        public IActionResult Edit(int id)
        {
            HolidayRepository repository = new HolidayRepository();
            var result = repository.GetHolidayById(id);
            var response = repository.Listtypes();
            ViewBag.type = new SelectList(response.Response, "type", "type", result.Response.type);

            return View(result.Response);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(HolidayModel holiday)
        {
            if (ModelState.IsValid)
            {
                HolidayRepository repository = new HolidayRepository();

                var response = repository.Update(holiday, ViewBag.userId);
                if (response.Message == "Success")
                {
                    TempData["msg"] = "Holiday Saved Successfully";
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("hours", response.Message);
                    return View("Edit", holiday);
                }
            }
            return View("Edit", holiday);
        }
        public IActionResult Delete(int id)
        {
            HolidayRepository repository = new HolidayRepository();
            repository.Delete(id, ViewBag.userId);
            TempData["msg"] = "Holiday Deleted Successfully";
            return RedirectToAction("Index");
        }
        public JsonResult HolidayList()
        {
            string msg = "";
            HolidayRepository holiday = new HolidayRepository();
            var response = holiday.ListHolidays(ViewBag.userId, out msg);
            return new JsonResult(JsonConvert.SerializeObject(response.Response));
        }

        public JsonResult SaturdaysList()
        {
            string msg = "";
            HolidayRepository holiday = new HolidayRepository();
            var response = holiday.ListSaturdays(ViewBag.userId, out msg);
            return new JsonResult(JsonConvert.SerializeObject(response.Response));
        }

        [HttpGet]
        public JsonResult HolidayType(string date)
        {
            HolidayRepository holiday = new HolidayRepository();
            var response = holiday.GetholidayType(date);
            return new JsonResult(JsonConvert.SerializeObject(response.Response));
        }

    }
}
