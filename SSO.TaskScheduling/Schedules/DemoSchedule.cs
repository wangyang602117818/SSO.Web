using SSO.Util.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSO.TaskScheduling.Schedules
{
    public class DemoSchedule : ISchedule
    {
        private string name = "DemoSchedule";
        private string description = "DemoScheduleDescription";

        public string Name => name;
        public string Description => description;

        public string Execute()
        {
            Log4Net.InfoLog("xx");
            return "ok";
        }
    }
}
