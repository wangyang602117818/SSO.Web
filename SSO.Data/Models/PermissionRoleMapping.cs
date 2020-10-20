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
        public int InsertMany(string role, IEnumerable<string> permissions)
        {
            return base.ExecuteNonQuery("insert-many", new { Role = role, Permissions = permissions });
        }
        public List<PermissionRoleMapping> GetByRole(string role)
        {
            int count = 0;
            return base.QueryList<PermissionRoleMapping>("get-by-role", new { Role = role }, ref count);
        }
    }
}
