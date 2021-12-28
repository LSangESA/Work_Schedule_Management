using QL_VANBAN.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace QL_VANBAN.Controllers
{
    public class BaseController : Controller
    {
        // GET: Base
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var sesssion = (UserLogin)Session[CommonConstants.USER_SESSION];
            if (sesssion == null)
            {
                filterContext.Result = new RedirectToRouteResult(new
                    RouteValueDictionary(new { controller = "Login", action = "Index" }));
            }
            base.OnActionExecuting(filterContext);

        }

        protected void SetAlert(string message, string type)
        {
            TempData["AlertMessage"] = message;
            if(type == "success")
            {
                TempData["AlertType"] = "alert-success";
            }
            else if(type == "warning")
            {
                TempData["AlertType"] = "alert-warning";
            }
            else if(type == "danger")
            {
                TempData["AlertType"] = "alert-danger";
            }
        }
    }
}