using Ahead.Common;
using Ahead.Data;
using System.Linq;
using System.Web.Mvc;

namespace Ahead.Controllers
{
    public class ForumController : CustomController
    {
        private ForumRepository _forum_activity_repository;

        public ForumController()
        {
            _forum_activity_repository = new Data.ForumRepository();
        }
        

       //[EntryPermissionFilter]
        public ActionResult Index()
        {
            var temp = _forum_activity_repository.PrepareViewModeForPage2();
            var replycommnet = _forum_activity_repository.RetrieveDailyReplyComment();
            ViewBag.Cloud = temp.Where(w => w.Team == "Cloud").ToArray();
            ViewBag.Mobile = temp.Where(w => w.Team == "Mobile Dev").ToArray();
            ViewBag.IC = temp.Where(w => w.Team == "Installation & Configuration").ToArray();
            ViewBag.Dev = temp.Where(w => w.Team == "Fundamental Developing").ToArray();
            ViewBag.SQL = temp.Where(w => w.Team == "SQL").ToArray();

            ViewBag.Activities = replycommnet;

            return View();
        }

        public ActionResult Detail(string ID)
        {
            string msg = ID;
            var temp = _forum_activity_repository.FindRecentTwoMonthCase();
            var data = temp.Where(w => w.Alias == ID).Select(s => new ViewModeForumActivityDetail() { ActivityID = s.ID, Alias = s.Alias, Comment = s.Comment, SubmitTime = s.PublishTime, IsReply = s.OperationType.StartsWith("Replied to") }).OrderByDescending(o => o.SubmitTime);
            ViewBag.Activity = data.Where(w => w.IsReply == true).ToArray();
            ViewBag.ProposeAndMark = data.Where(w => w.IsReply == false).ToArray();
            ViewBag.Count = data.Where(w => w.IsReply == true).Count();

            return View();
        }

        public ActionResult RemoveForumActivityCache()
        {
            _forum_activity_repository.RemoveListSupportAgent();
            return Content("Done");
        }
    }
}