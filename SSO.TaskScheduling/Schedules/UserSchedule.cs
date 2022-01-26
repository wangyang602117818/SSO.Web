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
    public class UserSchedule : BaseSchedule
    {
        private string tableName = "User";
        private string name = "UserSchedule";
        private string description = "监视用户数据变化";
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
            string title = data["UserName"].ToString();
            string desc = "";
            DateTime createtime = DateTime.Parse(data["CreateTime"].ToString());
            return AddData(DataBaseType.sqlserver, tableName, id, title, desc, createtime);
        }
    }
}
