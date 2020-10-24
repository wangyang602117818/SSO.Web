using SSO.Util.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSO.Data
{
    public class SqlHelper
    {
        public static string connStr = null;
        static SqlHelper()
        {
            connStr = AppSettings.GetValue("sqlserver");
        }
        public SqlHelper()
        {
        }
        public int ExecuteNonQuery(string sql, params SqlParameter[] parameters)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand comm = conn.CreateCommand())
                {
                    conn.Open();
                    comm.CommandText = sql;
                    comm.Parameters.AddRange(parameters);
                    return comm.ExecuteNonQuery();
                }
            }
        }
        public object ExecuteScalar(string sql, params SqlParameter[] parameters)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand comm = conn.CreateCommand())
                {
                    conn.Open();
                    comm.CommandText = sql;
                    comm.Parameters.AddRange(parameters);
                    return comm.ExecuteScalar();
                }
            }
        }
        public DataTable ExecuteDataTable(string sql, params SqlParameter[] parameters)
        {
            using (SqlDataAdapter adapter = new SqlDataAdapter(sql, connStr))
            {
                DataTable dt = new DataTable();
                adapter.SelectCommand.Parameters.AddRange(parameters);
                adapter.Fill(dt);
                return dt;
            }
        }
        public SqlDataReader ExecuteReader(string sql, params SqlParameter[] parameters)
        {
            SqlConnection conn = new SqlConnection(connStr);
            SqlCommand cmd = conn.CreateCommand();
            conn.Open();
            cmd.CommandText = sql;
            cmd.Parameters.AddRange(parameters);
            return cmd.ExecuteReader(CommandBehavior.CloseConnection);
        }
        public int ExecuteNonQueryTransaction(IEnumerable<string> sqls, IEnumerable<SqlParameter[]> parameters)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
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
                            for (var i = 0; i < sqls.Count(); i++)
                            {
                                command.CommandText = sqls.ElementAt(i);
                                command.Parameters.AddRange(parameters.ElementAt(i));
                                count = command.ExecuteNonQuery();
                                command.Parameters.Clear();
                            }
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            Log4Net.ErrorLog(ex);
                            transaction.Rollback();
                        }
                        return count;
                    }
                }
            }
        }
    }
}
