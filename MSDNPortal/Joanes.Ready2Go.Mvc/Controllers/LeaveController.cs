using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ahead.Data;
using System.Globalization;

namespace Ahead.Controllers
{
    public class LeaveController : Controller
    {
        private SupportAgentRepository _supportagent_repository;
        private TeamRepository _team_repository;

        public LeaveController()
        {
            _supportagent_repository = new SupportAgentRepository();
            _team_repository = new TeamRepository();
        }

        public ActionResult Index()
        {

            ViewBag.Cloud = _supportagent_repository.FindALLMemberInTeam().Where(w => w.Team == "Cloud").ToArray();

            ViewBag.IC = _supportagent_repository.FindALLMemberInTeam().Where(w => w.Team == "Installation & Configuration").ToArray();

            ViewBag.Mobile = _supportagent_repository.FindALLMemberInTeam().Where(w => w.Team == "Mobile Dev").ToArray();

            ViewBag.Dev = _supportagent_repository.FindALLMemberInTeam().Where(w => w.Team == "Fundamental Developing").ToArray();

            ViewBag.Other = _supportagent_repository.FindALLMemberInTeam().Where(w => w.Team == "Other").ToArray();


            return View();
        }

        public ActionResult New(string ID)
        {
            if (string.IsNullOrEmpty(ID))
            {
                throw new Exception("there is no Alias parameter.");
            }

            var agent = _supportagent_repository.FindAll().Where(w => w.Alias == ID).FirstOrDefault();
            if (agent == null)
            {

                throw new Exception("There is no corresponding support agent information in database.");
            }

            ViewBag.SupportAgent = agent;
            //load ask leave type activity
            ViewBag.Activity = _supportagent_repository.LoadRecentActivity(agent).OrderByDescending(o => o.LogTime).Take(10).ToArray();


            ViewBag.StartDate = DateTime.Now.ToString("yyyy-MM-dd");
            
            return View();
        }

        [HttpPost]
        public ActionResult New(FormCollection form)
        {

            string alias = form["supportagentalias"].ToString();
            string type = form["txttype"].ToString();
            string startdate = form["startdate"].ToString();
            string starthour = form["starthour"].ToString();
            string startminutes = form["startminutes"].ToString();
            string enddate = form["enddate"].ToString();
            string endhour = form["endhour"].ToString();
            string endminutes = form["endminutes"].ToString();
            string reason = form["txtreason"].ToString();
            string intervaltime = form["txtinterval"].ToString();

            DateTime starttime = DateTime.Parse(startdate);
            starttime= starttime.AddHours(double.Parse(starthour));
            starttime= starttime.AddMinutes(double.Parse(startminutes));

            DateTime endtime = DateTime.Parse(enddate);
            endtime= endtime.AddHours(double.Parse(endhour));
            endtime= endtime.AddMinutes(double.Parse(endminutes));

            if (starttime.CompareTo(endtime) > 0)
            {
                throw new Exception("Are you sure you type Start Date later than End Date?");
            }

            double interval = 0;
            if (!double.TryParse(intervaltime, out interval))
            {
                throw new Exception("Cannot cast interval time to double numberic type.");
            }


            _supportagent_repository.UpdateOverTimeAndAnnual(alias, type, interval, reason, starttime, endtime);

            return RedirectToAction("Display", "Common", new { Result= "Success!", Message = "Save Done", returnvalue = "/Leave/Index" });

        }
    }
}