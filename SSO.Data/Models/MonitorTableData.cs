using Newtonsoft.Json.Linq;
using SSO.Util.Client;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SSO.Data.Models
{
    public class MonitorTableData : ModelBase
    {
        /// <summary>
        /// 监视并处理数据回调
        /// </summary>
        /// <param name="table"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public string Monitor(string table, Func<object, string> action)
        {
            try
            {
                //开启事务
                session.BeginTransaction();
                //插入记录,返回主键id
                var list = base.QueryList<object>("select-sync-data", null, new { table }).OrderBy(o => ((JObject)o)["version"]).ToList();
                var result = new List<string>();
                if (list.Count() > 0)
                {
                    var version = Convert.ToInt64(((JObject)list[list.Count - 1])["version"]);
                    //处理
                    foreach (var item in list)
                    {
                        if (action != null) result.Add(action(item));
                    }
                    //更新
                    base.ExecuteNonQuery("update-version", new { version }, new { table });
                }
                //提交事务
                session.CommitTransaction();
                return string.Join(",", result);
            }
            catch
            {
                //回滚事务
                session.RollBackTransaction();
                throw;
            }
        }
        [Obsolete("该方法为原始的事务处理,比较麻烦")]
        public IEnumerable<string> MonitorOld(string table, Func<object, string> action)
        {
            using (SqlConnection conn = new SqlConnection(session.connstring))
            {
                conn.Open();
                using (SqlTransaction transaction = conn.BeginTransaction())
                {
                    using (SqlCommand command = conn.CreateCommand())
                    {
                        command.Transaction = transaction;
                        try
                        {
                            var list = base.QueryList<object>("select-sync-data", null, new { table }).OrderBy(o => ((JObject)o)["version"]).ToList();
                            var result = new List<string>();
                            if (list.Count() > 0)
                            {
                                var version = Convert.ToInt64(((JObject)list[list.Count - 1])["version"]);
                                //处理
                                foreach (var item in list)
                                {
                                    if (action != null) result.Add(action(item));
                                }
                                //更新
                                base.ExecuteNonQuery("update-version", new { version }, new { table });
                            }
                            transaction.Commit();
                            return result;
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw ex;
                        }
                    }
                }
            }
        }
    }
}
