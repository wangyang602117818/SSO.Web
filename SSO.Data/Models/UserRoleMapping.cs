using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSO.Data.Models
{
    public class UserRoleMapping : SqlBase
    {
        public UserRoleMapping() : base("user_role_mapping.sql.xml") { }
        public string UserId { get; set; }
        public string Role { get; set; }

        public List<UserRoleMapping> GetByUserId(string userId)
        {
            int count = 0;
            return base.QueryList<UserRoleMapping>("get-by-userId", new { UserId = userId }, ref count);
        }
    }
}
