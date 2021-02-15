using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSO.Business
{
    public class TaskScheduling : ModelBase<Data.Models.TaskScheduling>
    {
        public TaskScheduling() : base(new Data.Models.TaskScheduling()) { }
        public IEnumerable<Data.Models.TaskScheduling> GetTaskSchedulings(int? status, ref int count)
        {
            return instance.GetTaskSchedulings(status, ref count);
        }
    }
}
