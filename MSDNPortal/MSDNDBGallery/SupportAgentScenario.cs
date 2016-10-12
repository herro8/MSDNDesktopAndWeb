using System;

namespace Ahead.Data
{
    public class SupportAgentScenario
    {
        public int ID { get; set; }

        public int AgentID { get; set; }

        public string ScenarioType { get; set; }

        //small type
        public string OperationType { get; set; }

        public string Comment { get; set; }

        public double RelatedValue { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public DateTime LogTime { get; set; }
    }
}