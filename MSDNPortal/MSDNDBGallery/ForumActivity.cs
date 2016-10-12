using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ahead.Data
{
   public   class ForumActivity
    {
        public int ID { get; set; }

        public string Alias { get; set; }

        public string OperationType { get; set; }

        public string Comment { get; set; }

        public DateTime PublishTime { get; set; }

        public string LogTime { get; set; }

        public DateTime LastUpdateTime { get; set; }
    }
}
