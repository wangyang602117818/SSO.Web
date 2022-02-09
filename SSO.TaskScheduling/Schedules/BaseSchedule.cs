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
        protected SearchService searchService = new SearchService(AppSettings.GetValue("messageBaseUrl"));
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
        /// <param name="dataBase"></param>
        /// <param name="table"></param>
        /// <param name="key"></param>
        /// <param name="title"></param>
        /// <param name="desc"></param>
        /// <param name="docCreateTime"></param>
        /// <returns></returns>
        public string AddData(DataBaseType database, string table, string key, string title, string desc, DateTime docCreateTime)
        {
            ServiceModel<string> result = searchService.InsertSearchData(database, table, key, title, desc, docCreateTime, "");
            Log4Net.InfoLog("add message center(" + table + "):" + JsonSerializerHelper.Serialize(result));
            return JsonSerializerHelper.Serialize(result);
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="dataBase"></param>
        /// <param name="table"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public string DeleteData(DataBaseType database, string table, string key)
        {
            ServiceModel<string> result = searchService.DeleteSearchData(database, table, key);
            Log4Net.InfoLog("delete message center(" + table + "):" + JsonSerializerHelper.Serialize(result));
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
