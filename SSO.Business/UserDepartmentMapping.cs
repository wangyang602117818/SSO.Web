using SSO.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSO.Business
{
    public class UserDepartmentMapping : ModelBase<Data.Models.UserDepartmentMapping>
    {
        public UserDepartmentMapping() : base(new Data.Models.UserDepartmentMapping()) { }
        public List<DateCountItem> GetUserDepartmentRatio()
        {
            return instance.GroupByDepartment();
        }
        public IEnumerable<string> GetByUserId(string userId)
        {
            IEnumerable<string> roles = instance.GetByUserId(userId).Select(s => s.DepartmentCode);
            return roles;
        }
    }
}
