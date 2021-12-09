using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSO.Business
{
    public class TaskSchedulingHistory : ModelBase<Data.Models.TaskSchedulingHistory>
    {
        public TaskSchedulingHistory() : base(new Data.Models.TaskSchedulingHistory()) { }

        public int InsertHistoryAndUpdateScheduling(int schedulingId, string schedulingName, DateTimeOffset runTime, DateTimeOffset endTime, DateTimeOffset? nextRunTime, string runResult)
        {
            return instance.InsertHistoryAndUpdateScheduling(schedulingId, schedulingName, runTime, endTime, nextRunTime, runResult);
        }
        public int InsertHistoryAndUpdateScheduling(int schedulingId, string schedulingName, DateTimeOffset runTime, DateTimeOffset endTime, DateTimeOffset? nextRunTime, IEnumerable<string> runResults)
        {
            return instance.InsertHistoryAndUpdateScheduling(schedulingId, schedulingName, runTime, endTime, nextRunTime, runResults);
        }
    }
}
