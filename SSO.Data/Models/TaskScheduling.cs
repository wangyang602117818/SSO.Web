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
        public SchedulingStateEnum Status { get; set; }
        public string Api { get; set; }
        public DateTime? NextRunTime { get; set; }
        public DateTime? LastRunTime { get; set; }
        public string LastRunResult { get; set; }

        public IEnumerable<TaskScheduling> GetTaskSchedulings(int? status, ref int count)
        {
            return base.QueryList<TaskScheduling>("get-task-scheduling-trigger", new { status }, null, ref count);
        }
        public TaskScheduling GetByName(string name)
        {
            return base.QueryObject<TaskScheduling>("get-by-name", new { Name = name }, null);
        }
        public int InsertScheduling(IEnumerable<object> objs)
        {
            List<string> sqls = new List<string>();
            List<SqlParameter[]> parameters = new List<SqlParameter[]>();
            List<string> eles = new List<string>() { "insert", "insert-mapping" };
            for (var i = 0; i < eles.Count(); i++)
            {
                sqls.Add(GetSql(eles.ElementAt(i), objs.ElementAt(i)));
                parameters.Add(GetParameters(objs.ElementAt(i)));
            }
            using (SqlConnection conn = new SqlConnection(session.connstring))
            {
                conn.Open();
                using (SqlTransaction transaction = conn.BeginTransaction())
                {
                    using (SqlCommand command = conn.CreateCommand())
                    {
                        command.Transaction = transaction;
                        int count = 0;
                        try
                        {
                            //第一条插入语句
                            command.CommandText = sqls.ElementAt(0) + " select SCOPE_IDENTITY()";
                            if (parameters.ElementAt(0).Length > 0)
                                command.Parameters.AddRange(parameters.ElementAt(0));
                            var id = Convert.ToInt32(command.ExecuteScalar());
                            command.Parameters.Clear();
                            //第二条
                            command.CommandText = sqls.ElementAt(1);
                            if (parameters.ElementAt(1).Length > 0)
                            {
                                command.Parameters.AddRange(parameters.ElementAt(1));
                                command.Parameters.Add(new SqlParameter("@schedulingId", id));
                            }
                            count = command.ExecuteNonQuery();
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw ex;
                        }
                        return count;
                    }
                }
            }
        }
        public int UpdateScheduling(object obj, int schedulingIds, IEnumerable<int> triggerIds)
        {
            var nodes = new List<string>() { "delete-mapping", "insert-mapping","update" };
            var datas = new List<object>() { new { SchedulingIds = new List<int> { schedulingIds } }, new { schedulingId = schedulingIds, triggerIds }, obj };
            return base.ExecuteTransaction(nodes, datas, null);
        }
        public int DeleteScheduling(IEnumerable<int> ids)
        {
            var nodes = new List<string>() { "delete", "delete-mapping" };
            var datas = new List<object>() { new { Ids = ids }, new { TaskIds = ids } };
            return base.ExecuteTransaction(nodes, datas, null);
        }
        public object GetSchedulingById(int id)
        {
            return base.QueryObject<object>("get-by-id", new { id }, null);
        }
        public int UpdateStatus(int id,int status)
        {
            return base.ExecuteNonQuery("update-status", new { id, status }, null);
        }
    }
}
