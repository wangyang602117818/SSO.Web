using System;
using System.Linq;

namespace SSO.Business
{
    public class Department : ModelBase
    {
        public int Insert(string code, string name, string description, string companyCode, int order, int layer, int parentCode)
        {
            userCenterContext.Departments.Add(new Data.Models.Department()
            {
                Code = code,
                Name = name,
                Description = description,
                CompanyCode = companyCode,
                Order = order,
                Layer = layer,
                ParentCode = parentCode,
                UpdateTime = DateTime.Now,
                CreateTime = DateTime.Now
            });
            return userCenterContext.SaveChanges();
        }
        public Data.Models.Department GetByCode(string code)
        {
            return userCenterContext.Departments.Where(c => c.Code == code).FirstOrDefault();
        }
    }
}
