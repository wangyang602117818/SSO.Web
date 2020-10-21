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

        public int DeleteMany(string userId)
        {
            return base.ExecuteNonQuery("delete-many", new { UserId = userId });
        }
        public int InsertMany(string userId, IEnumerable<string> permissions)
        {
            return base.ExecuteNonQuery("insert-many", new { UserId = userId, Permissions = permissions });
        }
        public List<PermissionUserMapping> GetByUser(string userId)
        {
            int count = 0;
            return base.QueryList<PermissionUserMapping>("get-by-user", new { UserId = userId }, ref count);
        }

    }
}
