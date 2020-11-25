using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSO.Data.Models
{
    public class PermissionUserMapping : ModelBase
    {
        public PermissionUserMapping() { }
        public string UserId { get; set; }
        public string Permission { get; set; }

        public int DeleteAndInsertMany(string userId, IEnumerable<string> permissions)
        {
            var nodes = new List<string>() { "delete-many", "update-user-count" };
            var datas = new List<object>() { new { UserId = userId }, new { UserId = userId, PermissionCount = permissions.Count() } };
            if (permissions.Count() > 0)
            {
                nodes.Add("insert-many");
                datas.Add(new { UserId = userId, Permissions = permissions });
            }
            return base.ExecuteTransaction(nodes, datas, null);
        }
        public IEnumerable<PermissionUserMapping> GetByUser(string userId)
        {
            int count = 0;
            return base.QueryList<PermissionUserMapping>("get-by-user", new { UserId = userId }, null, ref count);
        }

    }
}
