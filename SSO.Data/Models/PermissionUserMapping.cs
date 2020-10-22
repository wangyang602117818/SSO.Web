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
            return base.ExecuteNonQueryTransaction(new List<string>() { "delete-many", "update-user-count", "insert-many" }, new List<object>() { new { UserId = userId }, new { PermissionCount = permissions.Count() }, new { Permissions = permissions } }, null);
        }
        public List<PermissionUserMapping> GetByUser(string userId)
        {
            int count = 0;
            return base.QueryList<PermissionUserMapping>("get-by-user", new { UserId = userId }, ref count);
        }

    }
}
