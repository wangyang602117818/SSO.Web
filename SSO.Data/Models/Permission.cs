using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSO.Data.Models
{
    public class Permission : SqlBase
    {
        public Permission() : base("permission.sql.xml") { }
        public string Origin { get; set; }
        public string Name { get; set; }

        public int DeleteMany(string origin)
        {
            return base.ExecuteNonQuery("delete-many", new { Origin = origin });
        }
        public List<Permission> GetAll()
        {
            int count = 0;
            return base.QueryList<Permission>("get-all", null, ref count);
        }
        public int InserMany(string origin, IEnumerable<string> names)
        {
            return base.ExecuteNonQuery("insert-many", new { Origin = origin, Names = names });
        }
    }
}
