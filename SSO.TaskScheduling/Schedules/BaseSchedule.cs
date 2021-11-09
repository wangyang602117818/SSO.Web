using Quartz;
using SSO.Util.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSO.TaskScheduling.Schedules
{
    public abstract class BaseSchedule : IJob
    {
        public abstract string Name { get; }
        public abstract string Description { get; }
        public Task Execute(IJobExecutionContext context)
        {
            var data = JsonSerializerHelper.Deserialize<Data.Models.TaskScheduling>(context.JobDetail.JobDataMap.GetString("data"));
            Log4Net.InfoLog("run job:" + data.Id);
            DateTime runTime = context.FireTimeUtc.LocalDateTime;
            DateTime? nextRunTime = null;
            if (context.NextFireTimeUtc != null) nextRunTime = context.NextFireTimeUtc.Value.LocalDateTime;
            try
            {
                string result = ExecuteJob(data);
                DateTime endTime = DateTimeOffset.Now.LocalDateTime;
                new Business.TaskSchedulingHistory().InsertHistoryAndUpdateScheduling(data.Id, data.Name, runTime, endTime, nextRunTime, result);
            }
            catch (Exception ex)
            {
                Log4Net.ErrorLog(ex);
            }
            return Task.CompletedTask;
        }
        public abstract string ExecuteJob(Data.Models.TaskScheduling data);
    }
}
