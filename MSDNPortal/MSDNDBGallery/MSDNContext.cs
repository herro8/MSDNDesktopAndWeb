using System.Data.Entity;

namespace Ahead.Data
{
    public class MSDNContext: DbContext
    {

        public MSDNContext() : base("MSDNDataBase")
        {
                
        }

        public DbSet<Product> Products { get; set; }

        public DbSet<ReviewItem> ReviewItems { get; set; }

        public DbSet<SupportAgent> SupportAgents { get; set; }
        
        public DbSet<SupportAgentInProduct> SupportAgentInProducts { get; set; }

        public DbSet<SupportAgentInTeam> SupportAgentInTeams { get; set; }

        public DbSet<SupportAgentScenario> SupportAgentScenarios { get; set; }
        
        public DbSet<Team> Teams { get; set; }

        public DbSet<AskLeave> AskLeaves { get; set; }

        public DbSet<ForumActivity> ForumActivitys { get; set; }
    }
}