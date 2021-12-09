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
        protected MessageCenterService messageCenterService = new MessageCenterService(AppSettings.GetValue("messageBaseUrl"));
        public Task Execute(IJobExecutionContext context)
        {
            var data = JsonSerializerHelper.Deserialize<Data.Models.TaskScheduling>(context.JobDetail.JobDataMap.GetString("data"));
            Log4Net.InfoLog("run job:" + data.Name);
            DateTime runTime = context.FireTimeUtc.LocalDateTime;
            DateTime? nextRunTime = null;
            if (context.NextFireTimeUtc != null) nextRunTime = context.NextFireTimeUtc.Value.LocalDateTime;
            try
            {
                IEnumerable<string> result = ExecuteJob(data);
                DateTime endTime = DateTimeOffset.Now.LocalDateTime;
                if (result.Count() > 0)
                {
                    taskSchedulingHistory.InsertHistoryAndUpdateScheduling(data.Id, data.Name, runTime, endTime, nextRunTime, result);
                }
            }
            catch (Exception ex)
            {
                Log4Net.ErrorLog(ex);
            }
            return Task.CompletedTask;
        }
        /// <summary>
        /// 处理消息
        /// </summary>
        /// <param name="type">d:删除,i:修改,u:修改</param>
        /// <param name="id"></param>
        /// <param name="title"></param>
        /// <param name="desc"></param>
        /// <param name="docCreateTime"></param>
        /// <returns></returns>
        public string AddData(string id, string title, string desc, DateTime docCreateTime)
        {
            ServiceModel<string> result = messageCenterService.InsertSearchData(id, title, desc, docCreateTime);
            return JsonSerializerHelper.Serialize(result);
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string DeleteData(string id)
        {
            ServiceModel<string> result = messageCenterService.DeleteSearchData(id);
            return JsonSerializerHelper.Serialize(result);
        }
        /// <summary>
        /// 执行任务的基本处理方法
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public abstract IEnumerable<string> ExecuteJob(Data.Models.TaskScheduling data);
    }
}
