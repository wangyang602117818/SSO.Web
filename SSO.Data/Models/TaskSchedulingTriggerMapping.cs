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

        public int InsertMany(int taskId, IEnumerable<int> triggerIds)
        {
            return base.ExecuteNonQuery("insert-mapping", new { taskId, triggerIds }, null);
        }
        public bool CheckTriggerExists(IEnumerable<int> triggerIds)
        {
            return (int)ExecuteScalar("check-trigger-exists", new { TriggerIds = triggerIds }, null) > 0;
        }
    }
}
