using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSO.Data.Models
{
    public class UserRoleMapping : ModelBase
    {
        public UserRoleMapping() { }
        public string UserId { get; set; }
        public string Role { get; set; }

        public IEnumerable<UserRoleMapping> GetByUserId(string userId)
        {
            int count = 0;
            return base.QueryList<UserRoleMapping>("get-by-userId", new { UserId = userId }, null, ref count);
        }
    }
}
