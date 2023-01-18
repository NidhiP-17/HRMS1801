using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Models;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSite.Common
{
    public class SessionTimeoutAttribute : ActionFilterAttribute, IResultFilter
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            if (filterContext.HttpContext.Session.GetString("UserSession") == null)
            {
                if (filterContext.Controller is Controller controller)
                {
                    controller.ViewBag.loginDate = DateTime.Now.ToString("dd/MM/yyyy");
                    controller.ViewBag.loginTime = DateTime.Now.ToString("HH:mm");
                    controller.ViewBag.loginUserName = "";
                    controller.ViewBag.logoutTime = 0;
                    controller.ViewBag.userId = 0;
                }
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                {
                    controller = "Login",
                    action = "Index"
                }));
                //filterContext.Result = new RedirectResult("~/Login/Index");
                return;
            }
            else
            {

                UserSession user = (UserSession)Constants.ByteArrayToObject(filterContext.HttpContext.Session.Get("UserSession"));
                string conString;

                if (filterContext.Controller is Controller controller)
                {
                    var controllerName = controller.RouteData.Values["controller"].ToString().ToLower();
                    var actionName = controller.RouteData.Values["action"].ToString().ToLower();

                    controller.ViewBag.loginUserName = user.LoginUserName;
                    controller.ViewBag.userId = user.UserId;
                    controller.ViewBag.userTypeId = user.userTypeId;
                    controller.ViewBag.controller = controllerName;
                    controller.ViewBag.joiningDate = user.JoiningDate;
                    //check for accesslevel
                    //controller.ViewBag.isAdmin = user.AccessLevel == "Administrator" ? true : false;
                    //bool isAdmin = user.AccessLevel == "Administrator" ? true : false;


                    if (controller.ViewBag.userTypeId == 3)
                    {
                        if (controllerName == "employee" && actionName == "index")
                        {
                            filterContext.Result = new RedirectResult("~/Opps/Accessdenied");
                            return;
                        }
                        if (controllerName == "employee" && actionName == "create")
                        {
                            filterContext.Result = new RedirectResult("~/Opps/Accessdenied");
                            return;
                        }
                        if (controllerName == "project" && actionName == "index")
                        {
                            filterContext.Result = new RedirectResult("~/Opps/Accessdenied");
                            return;
                        }
                        if (controllerName == "project" && actionName == "create")
                        {
                            filterContext.Result = new RedirectResult("~/Opps/Accessdenied");
                            return;
                        }
                        if (controllerName == "task" && actionName == "create")
                        {
                            filterContext.Result = new RedirectResult("~/Opps/Accessdenied");
                            return;
                        }
                    }

                }

                base.OnActionExecuting(filterContext);

            }
        }






    }
}
