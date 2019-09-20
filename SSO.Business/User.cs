using System;
using System.Collections.Generic;
using System.Linq;

namespace SSO.Business
{
    public class User : ModelBase
    {
        public int Insert(string userId, string userName, string password, string mobile, string email, string companyCode, string idCard, char sex, List<string> departments, List<string> roles)
        {
            userCenterContext.UserBasics.Add(new Data.Models.UserBasic()
            {
                UserId = userId,
                UserName = userName,
                PassWord = password,
                CompanyCode = companyCode,
                Mobile = mobile,
                Email = email,
                IdCard = idCard,
                Sex = sex.ToString(),
                IsModified = false,
                Delete = false,
                UpdateTime = DateTime.Now,
                CreateTime = DateTime.Now
            });
            foreach (string department in departments)
            {
                userCenterContext.UserDepartmentMappings.Add(new Data.Models.UserDepartmentMapping()
                {
                    UserId = userId,
                    DepartmentCode = department,
                    UpdateTime = DateTime.Now,
                    CreateTime = DateTime.Now
                });
            }
            foreach (string role in roles)
            {
                userCenterContext.UserRoleMappings.Add(new Data.Models.UserRoleMapping()
                {
                    UserId = userId,
                    Role = role,
                    UpdateTime = DateTime.Now,
                    CreateTime = DateTime.Now
                });
            }
            return userCenterContext.SaveChanges();
        }
        public Data.Models.UserBasic GetUser(string userId)
        {
            return userCenterContext.UserBasics.Where(r => r.UserId == userId).FirstOrDefault();
        }
        public List<Data.Models.UserBasic> GetBasic(ref int count, string keyword = "", int pageIndex = 1, int pageSize = 15)
        {
            var query = from userBasic in userCenterContext.UserBasics where userBasic.Delete == false select userBasic;
            if (!string.IsNullOrEmpty(keyword)) query = query.Where(w => w.UserName.Contains(keyword));
            count = query.Count();
            return query.OrderByDescending(o => o.CreateTime).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        }
    }
}
