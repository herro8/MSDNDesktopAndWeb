//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Ahead.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class SupportAgentScenario
    {
        public int ID { get; set; }
        public int AgentID { get; set; }
        public string ScenarioType { get; set; }
        public string OperationType { get; set; }
        public string Comment { get; set; }
        public double RelatedValue { get; set; }
        public System.DateTime StartTime { get; set; }
        public Nullable<System.DateTime> EndTime { get; set; }
        public System.DateTime LogTime { get; set; }
    }
}