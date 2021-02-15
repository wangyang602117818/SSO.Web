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
        public IEnumerable<TaskTrigger> GetTaskTriggers(int taskId)
        {
            int count = 0;
            return QueryList<TaskTrigger>("get-task-triggers", new { taskId }, null, ref count);
        }
        public IEnumerable<object> GetTypes()
        {
            int count = 0;
            return QueryList<object>("get-types", null, null, ref count);
        }
    }
}
