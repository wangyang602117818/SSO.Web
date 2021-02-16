using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSO.Business
{
    public class TaskSchedulingTriggerMapping : ModelBase<Data.Models.TaskSchedulingTriggerMapping>
    {
        public TaskSchedulingTriggerMapping() : base(new Data.Models.TaskSchedulingTriggerMapping()) { }
        public int InsertMany(int taskId, IEnumerable<int> triggerIds)
        {
            return instance.InsertMany(taskId, triggerIds);
        }
        public bool CheckTriggerExists(IEnumerable<int> triggerIds)
        {
            return instance.CheckTriggerExists(triggerIds);
        }
    }
}
