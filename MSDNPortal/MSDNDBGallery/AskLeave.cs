using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ahead.Data
{
   public  class AskLeave
    {
        public int ID { get; set; }

        public string Alias { get; set; }
        
        public string OperationType { get; set; }

        public string Comment { get; set; }

        public double UsedTime { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public DateTime LogTime { get; set; }
    }
}
