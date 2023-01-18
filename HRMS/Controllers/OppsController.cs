using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebSite.Common;

namespace HRMS.Controllers
{
    public class OppsController : Controller
    {
        // GET: Opps
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Index()
        {
            HttpContext.Session.Clear();
            return View();
        }
        [SessionTimeoutAttribute]
        [HttpGet]
        public ActionResult Accessdenied()
        {
            return View();
        }
    }

}
