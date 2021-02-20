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
        public Data.Models.TaskScheduling GetByName(string name)
        {
            return instance.GetByName(name);
        }
        public int InsertScheduling(IEnumerable<object> objs)
        {
            return instance.InsertScheduling(objs);
        }
        public int UpdateScheduling(object obj, int schedulingId, IEnumerable<int> triggerIds)
        {
            return instance.UpdateScheduling(obj, schedulingId, triggerIds);
        }
        public int DeleteScheduling(IEnumerable<int> ids)
        {
            return instance.DeleteScheduling(ids);
        }
        public object GetSchedulingById(int id)
        {
            return instance.GetSchedulingById(id);
        }
        public int UpdateStatus(int id, int status)
        {
            return instance.UpdateStatus(id, status);
        }
        public int EnableScheduling(int id, bool enable)
        {
            return instance.EnableScheduling(id, enable);
        }
        public int CheckSchedulingRunning(IEnumerable<int> ids)
        {
            return instance.CheckSchedulingRunning(ids);
        }
        public IEnumerable<object> GetDistinctNames()
        {
            return instance.GetDistinctNames();
        }
    }
}
