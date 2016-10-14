using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ahead.Common
{
    public class ViewModeForumActivity
    {
        public string Alias { get; set; }

        public string Team { get; set; }

        public string DisplayName { get; set; }

        public int MarkedAProposedAnswer { get; set; }

        public int MarkedAnAnswer { get; set; }
        
        public int RepliedToAForumsThread { get; set; }

        public DateTime LastUpdate { get; set; }
        
    }


    public class ViewModeForumActivityDetail
    {

        public int ActivityID { get; set; }

        public string Alias { get; set; }

        public string Comment { get; set; }

        public DateTime SubmitTime { get; set; }

        public bool IsReply { get; set; }
    }
}