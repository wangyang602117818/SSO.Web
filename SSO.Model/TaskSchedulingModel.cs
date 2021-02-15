using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSO.Model
{
    public class TaskSchedulingModel
    {
        public int TaskId { get; set; }
        public int TriggerId { get; set; }
        public SchedulingTaskEnum SchedulingTaskEnum { get; set; }
    }

    public class SchedulingExample
    {
        public List<DateTimeOffset> Examples { get; set; }
        public List<string> CronsDescriptions { get; set; }
    }
}
