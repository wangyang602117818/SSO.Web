using Quartz;
using SSO.Util.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SSO.TaskScheduling.Test
{
    public class HelloJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {

            JobDataMap dataMap = context.JobDetail.JobDataMap;
            var result = JsonSerializerHelper.Deserialize<Data.Models.TaskScheduling>(dataMap.GetString("data"));


            await Console.Out.WriteLineAsync(DateTime.Now.ToString(AppSettings.DateTimeFormat) + ",next:" + context.NextFireTimeUtc.Value.ToString(AppSettings.DateTimeFormat) + ",id:" + result.Id);
        }
    }
}
