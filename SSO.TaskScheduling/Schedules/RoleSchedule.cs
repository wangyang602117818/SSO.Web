using Newtonsoft.Json.Linq;
using SSO.Data.Models;
using SSO.Util.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSO.TaskScheduling.Schedules
{
    public class RoleSchedule : BaseSchedule
    {
        private string tableName = "Role";
        private string name = "RoleSchedule";
        private string description = "监视角色数据变化";
        public override string Name => name;
        public override string Description => description;
        public override IEnumerable<string> ExecuteJob(Data.Models.TaskScheduling data)
        {
            MonitorTableData monitorTableData = new MonitorTableData();
            return monitorTableData.Monitor(tableName, ProcessTableData);
        }
        private string ProcessTableData(object arg)
        {
            JObject data = (JObject)arg;
            string type = data["opertion"].ToString(); //U I D
            string id = data["realId"].ToString();
            if (type.ToLower() == "d") return DeleteData(DataBaseType.sqlserver, tableName, id);
            string title = data["Name"]?.ToString();
            string desc = data["Description"]?.ToString();
            DateTime createtime = DateTime.Parse(data["CreateTime"]?.ToString());
            return AddData(DataBaseType.sqlserver, tableName, id, title, desc, createtime);
        }
    }
}
