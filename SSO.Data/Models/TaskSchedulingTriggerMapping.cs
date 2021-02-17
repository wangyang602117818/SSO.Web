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
        public int SchedulingId { get; set; }
        public int TriggerId { get; set; }

        public bool CheckTriggerExists(IEnumerable<int> triggerIds)
        {
            return (int)ExecuteScalar("check-trigger-exists", new { TriggerIds = triggerIds }, null) > 0;
        }
        public IEnumerable<TaskSchedulingTriggerMapping> GetByTriggerId(int triggerId)
        {
            int count = 0;
            return base.QueryList<TaskSchedulingTriggerMapping>("get-by-triggerid", new { triggerId }, null, ref count);
        }
    }
}
