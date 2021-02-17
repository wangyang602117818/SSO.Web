using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSO.Model
{
    public class SchedulingQueueModel
    {
        public int SchedulingId { get; set; }
        public int TriggerId { get; set; }
        public SchedulingStateEnum SchedulingState { get; set; }
    }

    public class SchedulingExample
    {
        public List<DateTimeOffset> Examples { get; set; }
        public List<string> CronsDescriptions { get; set; }
    }
}
