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
                   group t2 by udm.DepartmentCode into grouped
                   select grouped;
        }
    }
}
