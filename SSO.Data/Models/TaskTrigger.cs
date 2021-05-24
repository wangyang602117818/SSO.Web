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
            return QueryList<TaskTrigger>("get-task-triggers", new { schedulingId }, null);
        }
        public IEnumerable<object> GetTypes()
        {
            return QueryList<object>("get-types", null, null);
        }
        public IEnumerable<TaskTrigger> GetByIds(IEnumerable<int> ids)
        {
            return QueryList<TaskTrigger>("get-by-ids", new { ids }, null);
        }
        public IEnumerable<TaskTrigger> GetBySchedulingId(int schedulingId)
        {
            return QueryList<TaskTrigger>("get-by-scheduling-id", new { schedulingId }, null);
        }
    }
}
