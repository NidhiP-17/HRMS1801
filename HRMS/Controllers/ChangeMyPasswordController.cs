using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Repositories;
using WebSite.Common;

namespace HRMS.Controllers
{
    [SessionTimeoutAttribute]
    public class ChangeMyPasswordController : Controller
    {
        public IActionResult Index()
        {
            var st = ViewBag.passwordExpited;
            ChangeMyPasswordModel resetPasswordModel = new ChangeMyPasswordModel();
            return View(resetPasswordModel);
        }


        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        [HttpPost]
        public ActionResult update(ChangeMyPasswordModel resetPasswordModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    byte[] bt;
                    if (HttpContext.Session.TryGetValue("userId", out bt))
                    {
                        string msg = "";
                        EmployeeRepository userRepository = new EmployeeRepository();
                        var response = userRepository.ChangePassword((long)Constants.ByteArrayToObject(bt), resetPasswordModel.OldPassword, resetPasswordModel.NewPassword, out msg);
                        if (response.Status == Constants.WebApiStatusOk)
                        {
                            return RedirectToAction("ResetPasswordSuccess", "Login");// ("~/Login/ResetPasswordSuccess");
                        }
                        else
                        {
                            ModelState.AddModelError("OldPassword", response.Message);
                            return View("Index", resetPasswordModel);
                        }
                    }
                    else
                    {
                        return RedirectToAction("Index", "Opps");
                    }

                }
                catch (Exception exp)
                {
                    ModelState.AddModelError(string.Empty, exp.Message + "Internal server error, Please try after some time.");
                    return View("Index", resetPasswordModel);
                }
            }
            else
            {
                return View("Index", resetPasswordModel);
            }




        }
    }
}
