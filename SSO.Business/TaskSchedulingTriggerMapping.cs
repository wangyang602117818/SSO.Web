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
        public bool CheckTriggerExists(IEnumerable<int> triggerIds)
        {
            return instance.CheckTriggerExists(triggerIds);
        }
        public IEnumerable<Data.Models.TaskSchedulingTriggerMapping> GetByTriggerId(int triggerId)
        {
            return instance.GetByTriggerId(triggerId);
        }
    }
}
