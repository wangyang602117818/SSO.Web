﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSO.Data.Models
{
    public class TaskSchedulingHistory : ModelBase
    {
        public int SchedulingId { get; set; }
        public string SchedulingName { get; set; }
        public DateTime? RunTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string RunResult { get; set; }

        public int InsertHistoryAndUpdateScheduling(int schedulingId, string schedulingName, DateTimeOffset runTime, DateTimeOffset endTime, DateTimeOffset? nextRunTime, string runResult)
        {
            List<object> datas = new List<object>()
            {
                new { schedulingId,schedulingName,runTime,endTime,runResult},
                new { Id=schedulingId, NextRunTime=nextRunTime,LastRunTime=runTime,LastRunResult=runResult}
            };
            List<string> nodes = new List<string>() { "insert", "update-scheduling" };
            return base.ExecuteTransaction(nodes, datas, null);
        }
        public int InsertHistoryAndUpdateScheduling(int schedulingId, string schedulingName, DateTimeOffset runTime, DateTimeOffset endTime, DateTimeOffset? nextRunTime, IEnumerable<string> runResults)
        {
            List<object> datas = new List<object>()
            {
                new { schedulingId,schedulingName,runTime,endTime,runResults},
                new { Id=schedulingId, NextRunTime=nextRunTime,LastRunTime=runTime,LastRunResult=runResults.LastOrDefault()}
            };
            List<string> nodes = new List<string>() { "insert-many", "update-scheduling" };
            return base.ExecuteTransaction(nodes, datas, null);
        }
    }
}
