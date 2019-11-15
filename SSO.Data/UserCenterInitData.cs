using SSO.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSO.Data
{
    public class UserCenterInitData : DropCreateDatabaseAlways<UserCenterContext>
    {
        protected override void Seed(UserCenterContext context)
        {
            //IList<Company> companies = new List<Company>();
            //companies.Add(new Company() { Code = "000", Name = "Compay", Description = "system" });
            //context.Companies.AddRange(companies);

            base.Seed(context);
        }
    }
}
