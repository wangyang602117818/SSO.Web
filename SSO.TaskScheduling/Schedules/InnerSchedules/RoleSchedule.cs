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
    public class RoleSchedule : TableScheduleBase
    {
        public override string TableName => "Role";
        public override string Name => "RoleSchedule";
        public override string Description => "监视角色数据变化";
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
            string title = data["Name"]?.ToString();
            string desc = data["Description"]?.ToString();
            DateTime createtime = DateTime.Parse(data["CreateTime"]?.ToString());
            return AddData(DataBaseType.sqlserver, TableName, id, title, desc, createtime);
        }
    }
}
