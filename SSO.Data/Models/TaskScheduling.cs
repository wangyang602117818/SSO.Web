using SSO.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
        public int Status { get; set; }
        public DateTime? NextRunTime { get; set; }
        public DateTime? LastRunTime { get; set; }
        public string LastRunResult { get; set; }
        public bool Enable { get; set; }
        public IEnumerable<TaskScheduling> GetTaskSchedulings(int? status)
        {
            return base.QueryList<TaskScheduling>("get-task-scheduling-trigger", new { status }, null);
        }
        public TaskScheduling GetByName(string name)
        {
            return base.QueryObject<TaskScheduling>("get-by-name", new { Name = name }, null);
        }
        public int UpdateScheduling(object obj, int schedulingId, IEnumerable<int> triggerIds)
        {
            var nodes = new List<string>() { "delete-mapping", "insert-mapping", "update" };
            var datas = new List<object>() { new { SchedulingIds = new List<int> { schedulingId } }, new { schedulingId = schedulingId, triggerIds }, obj };
            return base.ExecuteTransaction(nodes, datas, null);
        }
        public int DeleteScheduling(IEnumerable<int> ids)
        {
            var nodes = new List<string>() { "delete", "delete-mapping" };
            var datas = new List<object>() { new { Ids = ids }, new { SchedulingIds = ids } };
            return base.ExecuteTransaction(nodes, datas, null);
        }
        public object GetSchedulingById(int id)
        {
            return base.QueryObject<object>("get-by-id", new { id }, null);
        }
        public int EnableScheduling(int id, bool enable)
        {
            return base.ExecuteNonQuery("enable-scheduling", new { id, enable }, null);
        }
        public int CheckSchedulingRunning(IEnumerable<int> ids)
        {
            return (int)ExecuteScalar("check-scheduling-running", new { ids }, null);
        }
        public IEnumerable<object> GetDistinctNames()
        {
            return QueryList<object>("get-distinct-names", null, null);
        }
    }
}
