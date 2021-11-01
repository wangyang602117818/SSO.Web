using Quartz;
using SSO.Util.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSO.TaskScheduling
{
    public abstract class SchedulingJob : IJob
    {
        Business.TaskSchedulingHistory schedulingHistory = new Business.TaskSchedulingHistory();
        HttpRequestHelper httpRequest = new HttpRequestHelper();
        public virtual string ExecuteJob(Data.Models.TaskScheduling data)
        {
            string result = "";
            try
            {
                result = httpRequest.Get(data.Api, null);
            }
            catch (Exception ex)
            {
                Log4Net.ErrorLog(ex);
                result = ex.Message;
            }
            return result;
        }
        public Task Execute(IJobExecutionContext context)
        {
            var data = JsonSerializerHelper.Deserialize<Data.Models.TaskScheduling>(context.JobDetail.JobDataMap.GetString("data"));
            Log4Net.InfoLog("run job:" + data.Id);
            DateTime runTime = context.FireTimeUtc.LocalDateTime;
            DateTime? nextRunTime = null;
            if (context.NextFireTimeUtc != null) nextRunTime = context.NextFireTimeUtc.Value.LocalDateTime;
            string result = ExecuteJob(data);
            try
            {
                schedulingHistory.InsertHistoryAndUpdateScheduling(data.Id, data.Name, runTime, nextRunTime, result);
            }
            catch (Exception ex)
            {
                Log4Net.ErrorLog(ex);
            }
            return Task.Delay(0);
        }
    }
 
}
