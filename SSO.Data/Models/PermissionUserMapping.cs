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

        public int InsertMany(string user, IEnumerable<string> permissions)
        {
            return base.ExecuteNonQuery("insert-many", new { User = user, Permissions = permissions });
        }
    }
}
