using SSO.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSO.Business
{
    public class UserDepartmentMapping : ModelBase
    {
        public IQueryable<DateCountItem> GetUserDepartmentRatio()
        {
            return from udm in userCenterContext.UserDepartmentMappings
                   join dept in userCenterContext.Departments
                   on udm.DepartmentCode equals dept.Code into t1
                   from t2 in t1.DefaultIfEmpty()
                   select new DateCountItem { type = t2.Name, count = 1 } into t3
                   group t3 by t3.type into grouped
                   select new DateCountItem() { type = grouped.Key, count = grouped.Count() };
            //group t2 by udm.DepartmentCode into grouped
            //select new DateCountItem() { type = grouped.Key, count = grouped.Count() };
        }
    }
}
