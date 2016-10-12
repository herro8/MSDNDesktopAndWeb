using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ahead.Data;
using System.IO;

namespace Joanes.Ready2Go.Mvc.Controllers
{
    public class AgentsController : Controller
    {

        private Ahead.Data.SupportAgentRepository repository = null;

        public AgentsController()
        {
            repository = new Ahead.Data.SupportAgentRepository();
        }

        // GET: Agents
        public ActionResult Index(string id)
        {
          



            return View();
        }


        public ActionResult ImportAndUpdateSupportAgents()
        {
            return View();
        }


        [HttpPost]
        public ActionResult ImportAndUpdateSupportAgents(HttpPostedFileBase[] files)
        {
            
            
           return View();
        }
    }
}