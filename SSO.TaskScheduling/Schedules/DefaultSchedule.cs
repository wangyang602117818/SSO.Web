using Quartz;
using SSO.Util.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SSO.TaskScheduling.Schedules
{
    public class DefaultSchedule : BaseSchedule
    {
        private string name = "DefaultSchedule";
        private string description = "DefaultScheduleDescription";

        public override string Name => name;
        public override string Description => description;

        public override string ExecuteJob(Data.Models.TaskScheduling data)
        {
            DateTimeOffset now = DateTimeOffset.Now.LocalDateTime;
            Log4Net.InfoLog(now.ToString("yyyy-MM-dd HH:mm:ss"));
            return "success";
        }
    }
}
