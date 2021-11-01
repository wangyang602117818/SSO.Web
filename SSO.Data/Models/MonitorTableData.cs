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
        public void Monitor(string table)
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
                            var list = base.QueryList<object>("select-sync-data", null, new { table });
                            if (list.Count() > 0)
                            {
                                var version = Convert.ToInt64(((JObject)list.ToList()[list.Count() - 1])["version"]);
                                //处理
                                foreach (var item in list)
                                {
                                    Console.WriteLine(JsonSerializerHelper.Serialize(item));
                                }
                                //更新
                                base.ExecuteNonQuery("update-version", new { version }, new { table });
                            }
                            transaction.Commit();
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
