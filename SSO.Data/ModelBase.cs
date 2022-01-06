using Newtonsoft.Json;
using SSO.Util.Client.SqlBatisLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSO.Data
{
    public abstract class ModelBase : EntityBase
    {
        static SessionFactory sessionFactory = null;
        /// <summary>
        /// 默认读取 sbl.config 文件实例化sessionFactory,如果有不同的数据库,指定配置文件即可
        /// </summary>
        static ModelBase()
        {
            sessionFactory = new Configuration().Configure();
            sessionFactory.CreateTables();
        }
        private DateTime? createTime = DateTime.Now;
        public int Id { get; set; }
        public DateTime? UpdateTime { get; set; }
        public DateTime? CreateTime { get => createTime; set => createTime = value; }
        public int Total { get; set; } = 0;
        public ModelBase() : base(sessionFactory) { }
        [JsonIgnore]
        public int PageIndex { get; set; }
        [JsonIgnore]
        public int PageSize { get; set; }

        public int RecordCount()
        {
            return (int)ExecuteScalar("count", null);
        }
        public T GetById<T>(int id)
        {
            return QueryObject<T>("get-by-id", new { Id = id }, null);
        }
        public IEnumerable<T> GetAll<T>(object t)
        {
            return QueryList<T>("get-all", t, null);
        }
        public IEnumerable<T> GetPageList<T>(object t, object replacement) where T : new()
        {
            return QueryList<T>("get-page-list", t, replacement);
        }
    }
}
