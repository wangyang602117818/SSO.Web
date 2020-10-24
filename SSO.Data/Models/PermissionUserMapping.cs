using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSO.Data.Models
{
    public class PermissionUserMapping : SqlBase
    {
        public PermissionUserMapping() : base("permission_user_mapping.sql.xml") { }
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
            return base.ExecuteNonQueryTransaction(nodes, datas, null);
        }
        public List<PermissionUserMapping> GetByUser(string userId)
        {
            int count = 0;
            return base.QueryList<PermissionUserMapping>("get-by-user", new { UserId = userId }, ref count);
        }

    }
}
