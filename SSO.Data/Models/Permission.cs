using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSO.Data.Models
{
    public class Permission : ModelBase
    {
        public Permission() { }
        public string Origin { get; set; }
        public string Name { get; set; }

        public IEnumerable<Permission> GetAll()
        {
            int count = 0;
            return base.QueryList<Permission>("get-all", null, null, ref count);
        }
        public int DeleteAndInsertMany(string origin, IEnumerable<string> names)
        {
            var nodes = new List<string>() { "delete-many" };
            var datas = new List<object>() { new { Origin = origin } };
            if (names.Count() > 0)
            {
                nodes.Add("insert-many");
                datas.Add(new { Origin = origin, Names = names, CreateTime = DateTime.Now });
            }
            return base.ExecuteTransaction(nodes, datas, null);
        }
        public object CheckPermission(string userId, string permission)
        {
            return base.ExecuteScalar("check-permission", new { UserId = userId, Permission = permission });
        }
    }
}
