using SSO.Util.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSO.TaskScheduling.Schedules
{
    public class DateLogger : BaseSchedule
    {
        public override string Name => "DataLogger";
        public override string Description => "时间戳打印判断";
        public override string ExecuteJob()
        {
            long ts = DateTime.Now.TimeStamp();
            Log4Net.InfoLog(ts.ToString());
            if (ts % 2 == 0) return "success";
            return "false";
        }
    }
}
