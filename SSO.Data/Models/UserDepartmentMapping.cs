using SSO.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSO.Data.Models
{
    public class UserDepartmentMapping : SqlBase
    {
        public UserDepartmentMapping() : base("user_department_mapping.sql.xml") { }
        public string UserId { get; set; }
        public string DepartmentCode { get; set; }

        public List<UserDepartmentMapping> GetByUserId(string userId)
        {
            int count = 0;
            return base.QueryList<UserDepartmentMapping>("get-by-userId", new { UserId = userId }, ref count);
        }
        public List<DateCountItem> GroupByDepartment()
        {
            int count = 0;
            return base.QueryList<DateCountItem>("group-by-department", null, ref count);
        }
    }
}
