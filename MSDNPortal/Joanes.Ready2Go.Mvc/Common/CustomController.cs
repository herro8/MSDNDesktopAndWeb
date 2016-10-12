using Ahead.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ahead.Common
{
    public class CustomController: Controller
    {

        protected override IActionInvoker CreateActionInvoker()
        {
            string str = Request.CurrentExecutionFilePath;

            switch (str)
            {
                case "/Forum/Index":// see who are interested in this page
                    LogInterface.LogInfo(string.Format(SystemCustomDefine.LogFormatString_Forum_Index, CustomWindowsPrincipal.Name));
                    break;
                default:
                    break;
            }




            return base.CreateActionInvoker();
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {






            base.OnActionExecuting(filterContext);
        }
    }
}