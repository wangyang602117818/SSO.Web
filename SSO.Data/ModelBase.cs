using Newtonsoft.Json;
using SSO.Util.Client.SqlBatisLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSO.Data
{
    public class ModelBase : EntityBase
    {
        static SessionFactory sessionFactory = null;
        static ModelBase()
        {
            sessionFactory = new Configuration().Configure();
        }
        private DateTime? createTime = DateTime.Now;
        public int Id { get; set; }
        public DateTime? UpdateTime { get; set; }
        public DateTime? CreateTime { get => createTime; set => createTime = value; }
        public ModelBase() : base(sessionFactory) { }
        [JsonIgnore]
        public int PageIndex { get; set; }
        [JsonIgnore]
        public int PageSize { get; set; }

        public int Count()
        {
            return (int)ExecuteScalar("count", null);
        }
        public T GetById<T>(int id) where T : class
        {
            return QueryObject<T>("get-by-id", new { Id = id }, null);
        }
        public List<T> GetAll<T>(object t) where T : class
        {
            int count = 0;
            return QueryList<T>("get-all", t, null, ref count);
        }
        public List<T> GetPageList<T>(ref int count, object t, object replacement) where T : class
        {
            return QueryList<T>("get-page-list", t, replacement, ref count);
        }
    }
}
