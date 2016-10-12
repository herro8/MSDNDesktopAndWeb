using System;
using System.Collections.Generic;

namespace Ahead.Support
{
    public delegate void LogMessageEventHandler(string msg);


   public class SupportAgentProfile
    {
        public string Alias { get; set; }
        public string ProfileAddress { get; set; }
    }


    public class JSONResult
    {
        public List<UIFXJSON> Results { get; set; }
    }

    public  class UIFXJSON
    {
        public string Title { get; set; }

        public string TitleShort { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime CreatedDateLong { get; set; }

        public string CreatedDateFormatted { get; set; }

        public string Url { get; set; }

        public string Description { get; set; }

        public string DescriptionShort { get; set; }

        public int ForumId { get; set; }

        public int PostId { get; set; }

        public string AuthorName { get; set; }
    }



}
