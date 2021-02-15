using Newtonsoft.Json.Linq;
using SSO.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSO.Business
{
    public class TaskTrigger : ModelBase<Data.Models.TaskTrigger>
    {
        public TaskTrigger() : base(new Data.Models.TaskTrigger()) { }
        public IEnumerable<Data.Models.TaskTrigger> GetTaskTriggers(int taskId)
        {
            return instance.GetTaskTriggers(taskId);
        }
        public IEnumerable<string> GetTypes()
        {
            return instance.GetTypes().Select(s =>
            {
                return ((JObject)s).GetValue("type").ToString();
            });
        }
    }
}
