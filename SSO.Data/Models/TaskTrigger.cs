using SSO.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSO.Data.Models
{
    public class TaskTrigger : ModelBase
    {
        public TaskTrigger() { }
        public string Crons { get; set; }
        public string Description { get; set; }
        public string Description1 { get; set; }
        public DateTime? Activate { get; set; }
        public DateTime? Expire { get; set; }
        public IEnumerable<TaskTrigger> GetTaskTriggers(int schedulingId)
        {
            int count = 0;
            return QueryList<TaskTrigger>("get-task-triggers", new { schedulingId }, null, ref count);
        }
        public IEnumerable<object> GetTypes()
        {
            int count = 0;
            return QueryList<object>("get-types", null, null, ref count);
        }
        public IEnumerable<TaskTrigger> GetByIds(IEnumerable<int> ids)
        {
            int count = 0;
            return QueryList<TaskTrigger>("get-by-ids", new { ids }, null, ref count);
        }
        public IEnumerable<TaskTrigger> GetBySchedulingId(int schedulingId)
        {
            int count = 0;
            return QueryList<TaskTrigger>("get-by-scheduling-id", new { schedulingId }, null, ref count);
        }
    }
}
