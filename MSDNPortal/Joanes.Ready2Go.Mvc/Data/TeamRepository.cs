using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Web;

namespace Ahead.Data
{
    public class TeamRepository : IRepository<Team>
    {

        private DB1 db;
        public TeamRepository()
        {
            db = new DB1();
        }

        public void Add(Team entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Team entity)
        {
            throw new NotImplementedException();
        }

        public Team Find(int id)
        {
            throw new NotImplementedException();
        }

        public IList<Team> FindAll()
        {
            return ListTeam;
        }

        #region team cache 
        private List<Team> ListTeam
        {
            get
            {
                if (!ServerCacheManager.Cache.Contains("ListTeam"))
                    RefreshListTeam();
                return ServerCacheManager.Cache.Get("ListTeam") as List<Team>;
            }
        }

        private void RefreshListTeam()
        {

            var listteam = db.Teams.ToList();

            CacheItemPolicy cacheItemPolicy = new CacheItemPolicy();
            cacheItemPolicy.AbsoluteExpiration = DateTime.Now.AddHours(1);

            ServerCacheManager.Cache.Add("ListTeam", listteam, cacheItemPolicy);
        }

        public void RemoveListTeam()
        {
            if (ServerCacheManager.Cache.Contains("ListTeam"))
                ServerCacheManager.Cache.Remove("ListTeam");
        }
        #endregion
    }
}