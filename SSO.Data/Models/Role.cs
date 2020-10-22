using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSO.Data.Models
{
    public class Role : SqlBase
    {
        public Role() : base("role.sql.xml") { }
        public string Name { get; set; }
        public string Description { get; set; }
        public int PermissionCount { get; set; }

        public Role GetByName(string name)
        {
            int count = 0;
            return base.QueryObject<Role>("get-by-name", new { Name = name }, ref count);
        }
    }
}
