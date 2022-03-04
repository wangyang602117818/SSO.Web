using Newtonsoft.Json.Linq;
using SSO.Data.Models;
using SSO.Util.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSO.TaskScheduling.Schedules.InnerSchedules
{
    public class UserSchedule : TableScheduleBase
    {
        public override string TableName => "User";
        public override string Name => "UserSchedule";
        public override string Description => "监视用户数据变化";
        public override string ExecuteJob()
        {
            MonitorTableData monitorTableData = new MonitorTableData();
            return monitorTableData.Monitor(TableName, ProcessTableData);
        }
        private string ProcessTableData(object arg)
        {
            JObject data = (JObject)arg;
            string type = data["opertion"].ToString(); //U I D
            string id = data["realId"].ToString();
            if (type.ToLower() == "d") return DeleteData(DataBaseType.sqlserver, TableName, id);
            string title = data["UserName"].ToString();
            string desc = "";
            DateTime createtime = DateTime.Parse(data["CreateTime"].ToString());
            return AddData(DataBaseType.sqlserver, TableName, id, title, desc, createtime);
        }
    }
}
