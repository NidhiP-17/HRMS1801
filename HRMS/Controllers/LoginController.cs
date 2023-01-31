using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using NuGet.Protocol.Plugins;
using Repositories;
using System.Security.Cryptography;
using System.Text;
using WebSite.Common;

namespace HRMS.Controllers
{


    public class LoginController : Controller
    {
        #region "Login"
        // GET: Login
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Index()
        {
            HttpContext.Session.Clear();
            UserLoginWithId userLogin = new UserLoginWithId();
            if (Request.Cookies["loginId"] != null)
            {
                userLogin.Email = Request.Cookies["loginId"];
                userLogin.remember = true;
            }
            else
            {
                userLogin.remember = false;
            }

            return View(userLogin);
        }

        //Post login detail (Login click)
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> Index(UserLoginWithId login)
        {
            //ViewBag.ReturnUrl = returnUrl;
            try
            {
                if (ModelState.IsValid)
                {
                    //var result =  await repository.LoginUsingId(login.Email, login.Password);
                    var message = string.Empty;
                    EmployeeRepository repository = new EmployeeRepository();
                    var user = repository.LoginUsingId(login.Email, login.Password);
                    if (user.Response != null)
                    {
                        if (user.Response.status == "Active")
                        {
                            var session = new UserSession();
                            session.SetValue(user.Response);
                            HttpContext.Session.Set("UserSession", Constants.ObjectToByteArray(session));
                            HttpContext.Session.Set("userId", Constants.ObjectToByteArray(session.UserId));
                            HttpContext.Session.Set("usertypeId", Constants.ObjectToByteArray(session.userTypeId));
                            HttpContext.Session.Set("loginUserName", Constants.ObjectToByteArray(session.LoginUserName));
                            HttpContext.Session.SetString("usertypeId", session.userTypeId.ToString());
                            HttpContext.Session.SetString("joiningDate", session.userTypeId.ToString());

                            if (session.userTypeId == 3)
                                return RedirectToAction(nameof(Index), "Timesheet");
                            else
                                return RedirectToAction("Index", "Timesheet");// ("~/User/Index");

                        }
                        else
                        {
                            //ModelState.AddModelError(string.Empty, "Userid is inactive.");
                            ModelState.AddModelError(string.Empty, user.Message);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid credentials.");
                        //ModelState.AddModelError(string.Empty, user.Message);
                    }
                }
            }
            catch (Exception exp)
            {
                ModelState.AddModelError(string.Empty, exp.Message + "Internal server error, Please try after some time.");
            }

            return View();
        }

        //public ActionResult Index(UserLogin login)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            var message = string.Empty;
        //            UserRepository repository = new UserRepository();
        //            var user = repository.Login(login.emailaddress, login.Password);
        //            if (user.Response != null)
        //            {
        //                UserLogin userLogin = new UserLogin();
        //                if (login.remember)
        //                    Response.Cookies.Append("UserName", login.emailaddress, new CookieOptions() { Expires = DateTime.Now.AddDays(30) });
        //                else
        //                    Response.Cookies.Append("UserName", login.emailaddress, new CookieOptions() { Expires = DateTime.Now.AddDays(-1) });

        //                var session = new UserSession();
        //                session.SetValue(user.Response);
        //                HttpContext.Session.Set("UserSession", Constants.ObjectToByteArray(session));
        //                HttpContext.Session.Set("userId", Constants.ObjectToByteArray(session.UserId));
        //                //this.sessionStorage.SetValue("UserSession", session);
        //                //FormsAuthentication.SetAuthCookie(session.LoginUserName, false);
        //                return RedirectToAction("PumpStationList", "Dashboard");// ("~/User/Index");
        //            }
        //            else
        //            {
        //                ModelState.AddModelError(string.Empty, "Invalid credentials.");
        //            }
        //        }
        //    }
        //    catch (Exception exp)
        //    {
        //        ModelState.AddModelError(string.Empty, exp.Message + "Internal server error, Please try after some time.");
        //    }

        //    return View();
        //}

        #endregion
        #region "Passwrod"
        //[AllowAnonymous]
        //[HttpGet]
        //public ActionResult ForgotPassword()
        //{
        //    HttpContext.Session.Clear();
        //    Forgotpassword userLogin = new Forgotpassword();
        //    return View(userLogin);
        //}

        //[ValidateAntiForgeryToken]
        //[AllowAnonymous]
        //[HttpPost]
        //public ActionResult ForgotPassword(Forgotpassword login)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            var message = string.Empty;

        //            var key = GenerateToken(login.emailaddress);
        //            var length = key.Length;
        //            UserRepository repository = new UserRepository();
        //            var user = repository.ForgotPassword(login.emailaddress, key);
        //            if (user.Response != null)
        //            {
        //                //send email
        //                if (sendEmail(key, login.emailaddress))
        //                    return RedirectToAction("ResetPasswordNotification", "Login");// ("~/Login/ResetPasswordNotification");
        //                else
        //                    ModelState.AddModelError(string.Empty, "Internal server error, Please try after some time.");
        //            }
        //            else
        //            {
        //                ModelState.AddModelError(string.Empty, "Email address is not registered.");
        //            }
        //        }
        //    }
        //    catch (Exception exp)
        //    {
        //        ModelState.AddModelError(string.Empty, exp.Message + "Internal server error, Please try after some time.");
        //    }

        //    return View();
        //}
        [AllowAnonymous]
        [HttpGet]
        public ActionResult ResetPassword(int id)
        {
            ViewBag.userId = id;
            HttpContext.Session.Clear();
            return View();
        }
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        [HttpPost]
        public ActionResult ResetPassword(ResetPasswordModel resetPasswordModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var message = string.Empty;
                    EmployeeRepository repository = new EmployeeRepository();
                    var user = repository.ResetPassword(Convert.ToInt32(resetPasswordModel.userId), resetPasswordModel.NewPassword, out message);
                    if (user.Message == "Success")
                    {
                        return RedirectToAction("ResetPasswordSuccess", "Login");// ("~/Login/ResetPasswordSuccess");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Opps");// ("~/Opps/Index");
                    }
                }
            }
            catch (Exception exp)
            {
                ModelState.AddModelError(string.Empty, exp.Message + "Internal server error, Please try after some time.");
            }
            return View(resetPasswordModel);

        }
        [AllowAnonymous]
        [HttpGet]
        public ActionResult ResetPasswordSuccess()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult ResetPasswordNotification()
        {
            return View();
        }
        #endregion
        #region 
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Setting(int Id)
        {
            return View();
        }

        #endregion

        #region "Generate Key"

        private const string _alg = "HmacSHA256";
        private const string _salt = "rz8LuOtFBXphj9WQfvFh";
        public static string GenerateToken(string emailaddress)
        {
            string hash = string.Join(":", new string[] { emailaddress, _salt, DateTime.Now.ToString() });
            string hashLeft = "";
            string hashRight = "";
            using (HMAC hmac = HMACSHA256.Create(_alg))
            {
                hmac.Key = Encoding.UTF8.GetBytes(GetHashedPassword(emailaddress));
                hmac.ComputeHash(Encoding.UTF8.GetBytes(hash));
                hashLeft = Convert.ToBase64String(hmac.Hash);
                hashRight = string.Join(":", new string[] { emailaddress, DateTime.Now.ToString() });
            }
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Join(":", hashLeft, hashRight)));
        }

        public static string GetHashedPassword(string password)
        {
            string key = string.Join(":", new string[] { password, _salt });
            using (HMAC hmac = HMACSHA256.Create(_alg))
            {
                // Hash the key.
                hmac.Key = Encoding.UTF8.GetBytes(_salt);
                hmac.ComputeHash(Encoding.UTF8.GetBytes(key));
                return Convert.ToBase64String(hmac.Hash);
            }
        }

        #endregion

        //#region "Send email"
        //private bool sendEmail(string key, string toemailaddress)
        //{

        //    try
        //    {
        //        string reply = "";
        //        MailSenderRepository mailSenderRepository = new MailSenderRepository();
        //        var mailsender = mailSenderRepository.GetMailSender(out reply);

        //        if (mailsender.Status == Constants.WebApiStatusFail)
        //            return false;

        //        MailMessage mail = new MailMessage();
        //        SmtpClient SmtpServer = new SmtpClient(mailsender.Response.smtpClient);

        //        mail.From = new MailAddress(mailsender.Response.mailAddress);
        //        mail.To.Add(toemailaddress);
        //        mail.Subject = "Reset your Password";
        //        StringBuilder sb = new StringBuilder("Forgot your password?");

        //        sb.AppendLine("");
        //        sb.AppendLine("We've received a request to reset the password for this email address.");
        //        sb.AppendLine("");
        //        sb.AppendLine("To reset your password please click on this link or cut and paste this");
        //        sb.AppendLine("URL into your browser(link expires in 1 hour):");
        //        var request = string.Format("{0}://{1}{2}", Request.Scheme, Request.Host, Url.Content("~"));
        //        sb.AppendLine(request + "/Login/ResetPassword?key=" + key);
        //        sb.AppendLine("");
        //        sb.AppendLine("This link takes you to a page where you can change your password.");
        //        sb.AppendLine("If you don't want to reset your password, please ignore this message.");
        //        sb.AppendLine("Your password will not be reset.");

        //        mail.Body = sb.ToString();

        //        SmtpServer.UseDefaultCredentials = false;
        //        SmtpServer.Port = mailsender.Response.smptPort;
        //        SmtpServer.EnableSsl = mailsender.Response.EnableSsl;// == 1 ? true : false;
        //        SmtpServer.Credentials = new System.Net.NetworkCredential(mailsender.Response.userName, mailsender.Response.password);

        //        SmtpServer.Send(mail);
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        LogRepository logRepository = new LogRepository(0);
        //        logRepository.insetLog("sendEmail", ex.Message);
        //        return false;
        //    }
        //}
        //#endregion

    }
}
