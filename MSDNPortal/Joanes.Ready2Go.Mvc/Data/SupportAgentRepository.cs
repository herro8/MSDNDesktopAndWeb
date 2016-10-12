using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;

namespace Ahead.Data
{
    public class SupportAgentRepository : IRepository<SupportAgent>
    {
        private DB1 db;
        private TeamRepository _team_repository;
        public SupportAgentRepository()
        {
            db = new DB1();
            _team_repository = new TeamRepository();
        }


        public void Add(SupportAgent entity)
        {
            throw new NotImplementedException();
        }

        public void UpdateOverTimeAndAnnual(string alias, string opertaiontype, double relatedvalue, string comment, DateTime starttime, DateTime endtime)
        {
            var supportagent = db.SupportAgents.Where(w => w.Alias == alias).FirstOrDefault();
            if (supportagent == null)
            {
                throw new Exception("cannot load corresponding support agent from database");
            }

           
        }

        public IList<SupportAgentScenario> LoadRecentActivity(SupportAgent agent)
        {
            return db.SupportAgentScenarios.Where(w => w.AgentID == agent.ID).ToList();
        }

        public void Delete(SupportAgent entity)
        {
            throw new NotImplementedException();
        }

        public SupportAgent Find(int id)
        {
            throw new NotImplementedException();
        }

        //all those engineer who deliver in MSDN or Asp.net forum
        public IList<TeamMember> FindALLMemberInTeam()
        {
            var query = from agent in db.SupportAgents
                        where agent.DeliverMSDN != "0" || agent.DeliverASPNET != "0"
                        select new TeamMember()
                        {
                            Alias = agent.Alias,
                            ChineseName = agent.Chinese,
                            Team = agent.Team,
                            Annual = 0,
                            OverTime = 0
                        };

            var data = query.Where(w=> w.Team== "Installation & Configuration").ToList();

            return query.ToList();
        }

        public IList<SupportAgent> FindAll()
        {
            return ListSupportAgent;
        }

        #region support agent cache 
        private List<SupportAgent> ListSupportAgent
        {
            get
            {
                if (! ServerCacheManager.Cache.Contains("ListSupportAgent"))
                    RefreshListSupportAgent();
                return ServerCacheManager.Cache.Get("ListSupportAgent") as List<SupportAgent>;
            }
        }

        private void RefreshListSupportAgent()
        {
            var listsupportagent = db.SupportAgents.ToList();
            CacheItemPolicy cacheItemPolicy = new CacheItemPolicy();
            cacheItemPolicy.AbsoluteExpiration = DateTime.Now.AddHours(1);
            ServerCacheManager.Cache.Add("ListSupportAgent", listsupportagent, cacheItemPolicy);
        }

        public void RemoveListSupportAgent()
        {
            if (ServerCacheManager.Cache.Contains("ListSupportAgent"))
                ServerCacheManager.Cache.Remove("ListSupportAgent");
        }
        #endregion

      
    }
}