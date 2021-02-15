using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSO.Data.Models
{
    public class TaskSchedulingTriggerMapping : ModelBase
    {
        public TaskSchedulingTriggerMapping() { }
        public int TaskId { get; set; }
        public int TriggerId { get; set; }
    }
}
