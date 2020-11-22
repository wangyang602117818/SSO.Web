using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSO.Data.Models
{
    public class Role : ModelBase
    {
        public Role() { }
        public string Name { get; set; }
        public string Description { get; set; }
        public int PermissionCount { get; set; }

        public Role GetByName(string name)
        {
            return base.QueryObject<Role>("get-by-name", new { Name = name }, null);
        }
    }
}
