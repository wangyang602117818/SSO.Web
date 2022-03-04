using SSO.Util.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSO.TaskScheduling.Schedules.InnerSchedules
{
    public abstract class TableScheduleBase : BaseSchedule
    {
        public abstract string TableName { get; }
        public abstract override string Name { get; }
        public abstract override string Description { get; }
        protected SearchService searchService = new SearchService(AppSettings.GetValue("messageBaseUrl"));
        public abstract override string ExecuteJob();

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
        protected string AddData(DataBaseType database, string table, string key, string title, string desc, DateTime docCreateTime)
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
        protected string DeleteData(DataBaseType database, string table, string key)
        {
            ServiceModel<string> result = searchService.DeleteSearchData(database, table, key);
            Log4Net.InfoLog("delete message center(" + table + "):" + JsonSerializerHelper.Serialize(result));
            return JsonSerializerHelper.Serialize(result);
        }
    }
}
