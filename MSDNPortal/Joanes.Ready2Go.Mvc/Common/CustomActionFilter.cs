using System;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Mvc;


namespace Ahead.Common
{
    public class EntryPermissionFilter : System.Web.Mvc.ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            bool flag = false;
            if (flag)
            {
               
            }
            else
            {
                var httpContext = HttpContext.Current;
                httpContext.Server.TransferRequest("/Common/Display?Result=Error&Message=have no permission&ReturnValue=/Home/Index", true);


            }
        }
    }
}
