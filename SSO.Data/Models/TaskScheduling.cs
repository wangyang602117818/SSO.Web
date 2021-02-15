using SSO.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSO.Data.Models
{
    public class TaskScheduling : ModelBase
    {
        public TaskScheduling() { }
        public string Name { get; set; }
        public string Description { get; set; }
        public SchedulingTaskEnum Status { get; set; }
        public string Api { get; set; }
        public DateTime? NextRunTime { get; set; }
        public DateTime? LastRunTime { get; set; }
        public string LastRunResult { get; set; }

        public IEnumerable<TaskScheduling> GetTaskSchedulings(int? status, ref int count)
        {
            return base.QueryList<TaskScheduling>("get-task-scheduling-trigger", new { status }, null, ref count);
        }
    }
}
