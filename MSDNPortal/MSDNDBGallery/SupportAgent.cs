using System;

namespace Ahead.Data
{
    public class SupportAgent
    {
        public int ID { get; set; }

        public string Alias { get; set; }

        public string ChineseName { get; set; }

        public DateTime OnBoardTime { get; set; }

        public DateTime? GoLiveTime { get; set; }

        public string ActualLevel { get; set; }

        public string ExpectLevel { get; set; }

        public string AliasIdentity { get; set; }

        public double InitOverTime { get; set; }

        public double InitAnnual { get; set; }

        public int IsUsed { get; set; }
    }
}