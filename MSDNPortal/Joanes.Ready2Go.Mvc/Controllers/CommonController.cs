using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ahead.Controllers
{
    public class CommonController : Controller
    {
       

        public ActionResult Display()
        {
            ViewBag.Result = Request.QueryString["Result"].ToString();
            ViewBag.Message = Request.QueryString["Message"].ToString();
            ViewBag.ReturnValue = Request.QueryString["ReturnValue"].ToString();
            return View();
        }
    }
}