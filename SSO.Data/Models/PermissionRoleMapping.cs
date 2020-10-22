using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSO.Data.Models
{
    public class PermissionRoleMapping : SqlBase
    {
        public PermissionRoleMapping() : base("permission_role_mapping.sql.xml") { }
        public string Role { get; set; }
        public string Permission { get; set; }

        public int DeleteMany(string role)
        {
            return base.ExecuteNonQuery("delete-many", new { Role = role });
        }
        public int DeleteAndInsertMany(string role, IEnumerable<string> permissions)
        {
            return base.ExecuteNonQueryTransaction(new List<string>() { "delete-many", "update-role-count", "insert-many" }, new List<object>() { new { Role = role }, new { Name = role, PermissionCount = permissions.Count() }, new { Permissions = permissions } }, null);
        }
        public List<PermissionRoleMapping> GetByRole(string role)
        {
            int count = 0;
            return base.QueryList<PermissionRoleMapping>("get-by-role", new { Role = role }, ref count);
        }
    }
}
