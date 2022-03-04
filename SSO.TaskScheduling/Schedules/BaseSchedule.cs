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
        Business.TaskSchedulingHistory taskSchedulingHistory = new Business.TaskSchedulingHistory();
        public Task Execute(IJobExecutionContext context)
        {
            var data = JsonSerializerHelper.Deserialize<Data.Models.TaskScheduling>(context.JobDetail.JobDataMap.GetString("data"));
            Log4Net.InfoLog("run job:" + data.Name);
            DateTime runTime = context.FireTimeUtc.LocalDateTime;
            DateTime? nextRunTime = null;
            if (context.NextFireTimeUtc != null) nextRunTime = context.NextFireTimeUtc.Value.LocalDateTime;
            try
            {
                string result = ExecuteJob();
                DateTime endTime = DateTimeOffset.Now.LocalDateTime;
                if (!result.IsNullOrEmpty())
                    taskSchedulingHistory.InsertHistoryAndUpdateScheduling(data.Id, data.Name, runTime, endTime, nextRunTime, result);
            }
            catch (Exception ex)
            {
                Log4Net.ErrorLog(ex);
            }
            return Task.CompletedTask;
        }
        /// <summary>
        /// 执行任务的基本处理方法
        /// </summary>
        /// <returns></returns>
        public abstract string ExecuteJob();
    }
}
