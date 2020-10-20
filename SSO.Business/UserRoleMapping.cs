using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSO.Business
{
    public class UserRoleMapping: ModelBase<Data.Models.UserRoleMapping>
    {
        public UserRoleMapping() : base(new Data.Models.UserRoleMapping()) { }
        public IEnumerable<string> GetByUserId(string userId)
        {
            IEnumerable<string> roles = instance.GetByUserId(userId).Select(s => s.Role);
            return roles;
        }
        public int Insert(string userId, string role)
        {
            return instance.Insert(new Data.Models.UserRoleMapping()
            {
                UserId = userId,
                Role = role,
                CreateTime = DateTime.Now
            });
        }
        public int DeleteByUserId(string userId)
        {
            return instance.DeleteByUserId(userId);
        }
    }
}
