using SSO.Model;
using System.Linq;

namespace SSO.Business
{
    public class ModelBase
    {
        public Data.UserCenterContext userCenterContext = new Data.UserCenterContext();
        public OverviewTotal GetOverviewTotal()
        {
            return new OverviewTotal()
            {
                Companys = userCenterContext.Companies.Count(),
                Departments = userCenterContext.Departments.Count(),
                Roles = userCenterContext.Roles.Count(),
                Users = userCenterContext.UserBasics.Count()
            };
        }
    }
}
