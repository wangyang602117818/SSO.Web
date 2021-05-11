using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSO.Data.Models
{
    public class PermissionRoleMapping : ModelBase
    {
        public PermissionRoleMapping() { }
        public string Role { get; set; }
        public string Permission { get; set; }

        public int DeleteAndInsertMany(string role, IEnumerable<string> permissions)
        {
            var nodes = new List<string>() { "delete-many", "update-role-count" };
            var datas = new List<object>() { new { Role = role }, new { Name = role, PermissionCount = permissions.Count() } };
            if (permissions.Count() > 0)
            {
                nodes.Add("insert-many");
                datas.Add(new { Role = role, Permissions = permissions });
            }
            return base.ExecuteTransaction(nodes, datas, null);
        }
        public IEnumerable<PermissionRoleMapping> GetByRole(string role)
        {
            return base.QueryList<PermissionRoleMapping>("get-by-role", new { Role = role }, null);
        }
    }
}
