using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Web;
using Ahead.Data;
using System.Data.Entity;

namespace Ahead.Data
{
    public class ForumRepository : IRepository<Ahead.Data.ForumActivity>
    {

        private Ahead.Data.DB1 db;
        private SupportAgentRepository _supportagent_repository;

        public ForumRepository()
        {
            db = new DB1();
            _supportagent_repository = new SupportAgentRepository();
        }


        public void Add(ForumActivity entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(ForumActivity entity)
        {
            throw new NotImplementedException();
        }

        public ForumActivity Find(int id)
        {
            throw new NotImplementedException();
        }

        public IList<ForumActivity> FindAll()
        {
            throw new NotImplementedException();
        }

        public IList<ForumActivity> FindTodayAll()
        {
            return ListTodayForumActivity;
        }

        public IList<ForumActivity> FindRecentTwoMonthCase()
        {
            return ListRecentTwoMonthForumActivity;

        }

        public Ahead.Common.ViewModeForumActivity[] PrepareViewModeForPage()
        {
            List<Ahead.Common.ViewModeForumActivity> viewmodeforumactivity = new List<Common.ViewModeForumActivity>();
            var support_agent_in_team = _supportagent_repository.FindALLMemberInTeam();


            foreach (var supportagent in _supportagent_repository.FindAll().ToList())
            {
                //there is no need to track people deliver not in MSDN & Asp.net
                if (support_agent_in_team.Count(c=> c.Alias==supportagent.Alias)==0)
                {
                    continue;
                }
                var tempdate = FindTodayAll().Where(w => w.Alias == supportagent.Alias);
                Ahead.Common.ViewModeForumActivity viewmode = new Common.ViewModeForumActivity();
                viewmode.Alias = supportagent.Alias;
                var agentprofile = support_agent_in_team.Where(w => w.Alias == viewmode.Alias).FirstOrDefault();
               
                viewmode.Team =support_agent_in_team.Where(w => w.Alias == viewmode.Alias).FirstOrDefault().Team;
                viewmode.DisplayName = supportagent.Chinese;
                viewmode.MarkedAnAnswer = tempdate.Count(c => c.OperationType == "Marked an answer") +
                    tempdate.Count(c => c.OperationType == "Answered the question") +
                    tempdate.Count(c => c.OperationType == "Quickly answered the question");
                viewmode.MarkedAProposedAnswer = tempdate.Count(c => c.OperationType == "Marked a proposed answer");
                viewmode.RepliedToAForumsThread = tempdate.Count(c => c.OperationType == "Replied to a forums thread");
                var data = tempdate.OrderByDescending(o => o.LastUpdateTime).FirstOrDefault();
                if (data == null)
                {
                    viewmode.LastUpdate = DateTime.Now;
                }
                else {
                    viewmode.LastUpdate = data.LastUpdateTime;
                }
                
                viewmodeforumactivity.Add(viewmode);
            }
            return viewmodeforumactivity.ToArray();
        }

        public Ahead.Common.ViewModeForumActivity[] PrepareViewModeForPage2()
        {
            List<Ahead.Common.ViewModeForumActivity> viewmodeforumactivity = new List<Common.ViewModeForumActivity>();
            var support_agent_in_team = _supportagent_repository.FindALLMemberInTeam();


            foreach (var supportagent in support_agent_in_team)
            {
                var tempdate = FindTodayAll().Where(w => w.Alias == supportagent.Alias);
                Ahead.Common.ViewModeForumActivity viewmode = new Common.ViewModeForumActivity();
                viewmode.Alias = supportagent.Alias;
                var agentprofile = support_agent_in_team.Where(w => w.Alias == viewmode.Alias).FirstOrDefault();

                viewmode.Team = support_agent_in_team.Where(w => w.Alias == viewmode.Alias).FirstOrDefault().Team;
                viewmode.DisplayName = supportagent.ChineseName;
                viewmode.MarkedAnAnswer = tempdate.Count(c => c.OperationType == "Marked an answer") +
                    tempdate.Count(c => c.OperationType == "Answered the question") +
                    tempdate.Count(c => c.OperationType == "Quickly answered the question");
                viewmode.MarkedAProposedAnswer = tempdate.Count(c => c.OperationType == "Marked a proposed answer");
                viewmode.RepliedToAForumsThread = tempdate.Count(c => c.OperationType == "Replied to a forums thread");
                var data = tempdate.OrderByDescending(o => o.LastUpdateTime).FirstOrDefault();
                if (data == null)
                {
                    viewmode.LastUpdate = DateTime.Now;
                }
                else
                {
                    viewmode.LastUpdate = data.LastUpdateTime;
                }

                viewmodeforumactivity.Add(viewmode);
            }
            return viewmodeforumactivity.ToArray();
        }


        public Ahead.Data.ForumActivity[] RetrieveDailyReplyComment()
        {
            return db.ForumActivities.Where(w => DbFunctions.DiffDays(w.PublishTime, DateTime.Now) == 0 && w.Comment.StartsWith("Replied to")).OrderByDescending(o => o.PublishTime).ToArray();
        }

        public void RemoveListSupportAgent()
        {
            if (ServerCacheManager.Cache.Contains("ListForumActivity"))
                ServerCacheManager.Cache.Remove("ListForumActivity");

            if (ServerCacheManager.Cache.Contains("ListRecentTwoMonthForumActivity"))
                ServerCacheManager.Cache.Remove("ListRecentTwoMonthForumActivity");
        }


        #region fourm activity cache 
        private List<ForumActivity> ListTodayForumActivity
        {
            get
            {
                if (!ServerCacheManager.Cache.Contains("ListForumActivity"))
                    RefreshListForumActivity();
                return ServerCacheManager.Cache.Get("ListForumActivity") as List<ForumActivity>;
            }
        }

        private void RefreshListForumActivity()
        {
            string today = DateTime.Now.ToString("yyyy-MM-dd");
            var listforumactivity = db.ForumActivities.Where(w => w.LogTime == today).ToList();
            CacheItemPolicy cacheItemPolicy = new CacheItemPolicy();
            cacheItemPolicy.AbsoluteExpiration = DateTime.Now.AddHours(1);
            ServerCacheManager.Cache.Add("ListForumActivity", listforumactivity, cacheItemPolicy);
        }

      
        #endregion

        #region Recent Two Month Activity 
        private List<ForumActivity> ListRecentTwoMonthForumActivity
        {
            get
            {
                if (!ServerCacheManager.Cache.Contains("ListRecentTwoMonthForumActivity"))
                    RefreshListRecentTwoMonthForumActivity();
                return ServerCacheManager.Cache.Get("ListRecentTwoMonthForumActivity") as List<ForumActivity>;
            }
        }

        private void RefreshListRecentTwoMonthForumActivity()
        {
            DateTime Search_Start_Date = DateTime.Now.AddMonths(-1);
            DateTime Search_End_Date = DateTime.Now.AddDays(1);

            var listforumactivity = db.ForumActivities.Where(w => w.PublishTime >= Search_Start_Date && w.PublishTime <= Search_End_Date).ToList();
            CacheItemPolicy cacheItemPolicy = new CacheItemPolicy();
            cacheItemPolicy.AbsoluteExpiration = DateTime.Now.AddHours(1);
            ServerCacheManager.Cache.Add("ListRecentTwoMonthForumActivity", listforumactivity, cacheItemPolicy);
        }
        #endregion
    }
}