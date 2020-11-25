using SSO.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSO.Data.Models
{
    public class UserDepartmentMapping : ModelBase
    {
        public UserDepartmentMapping() { }
        public string UserId { get; set; }
        public string DepartmentCode { get; set; }

        public IEnumerable<UserDepartmentMapping> GetByUserId(string userId)
        {
            int count = 0;
            return base.QueryList<UserDepartmentMapping>("get-by-userId", new { UserId = userId },null, ref count);
        }
        public IEnumerable<DateCountItem> GroupByDepartment()
        {
            int count = 0;
            return base.QueryList<DateCountItem>("group-by-department", null,null, ref count);
        }
    }
}
