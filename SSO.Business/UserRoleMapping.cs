using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSO.Business
{
    public class UserRoleMapping : ModelBase
    {
        public IEnumerable<string> GetRolesByUserId(string userId)
        {
            IQueryable<string> roles = userCenterContext.UserRoleMappings.Where(w => w.UserId == userId).Select(s => s.Role);
            return roles;
        }
    }
}
