using SSO.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace SSO.Business
{
    public class User : ModelBase
    {
        public int Insert(string userId, string userName, string password, string mobile, string email, string companyCode, string idCard, char sex, List<string> departments, List<string> roles)
        {
            string companyName = "", departmentName = "", roleName = "";
            companyName = userCenterContext.Companies.Where(w => w.Code == companyCode).FirstOrDefault().Name;
            foreach (string department in departments)
            {
                departmentName += userCenterContext.Departments.Where(w => w.Code == department).FirstOrDefault().Name + ",";
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
                roleName += role + ",";
                userCenterContext.UserRoleMappings.Add(new Data.Models.UserRoleMapping()
                {
                    UserId = userId,
                    Role = role,
                    UpdateTime = DateTime.Now,
                    CreateTime = DateTime.Now
                });
            }
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
                CreateTime = DateTime.Now,
                CompanyName = companyName,
                DepartmentName = departmentName.TrimEnd(','),
                RoleName = roleName.TrimEnd(','),
            });
            return userCenterContext.SaveChanges();
        }
        public Data.Models.UserBasic GetUser(string userId)
        {
            return userCenterContext.UserBasics.Where(r => r.UserId == userId).FirstOrDefault();
        }
        public int DeleteUser(IEnumerable<string> userIds)
        {
            var userRoleMappings = userCenterContext.UserRoleMappings.Where(w => userIds.Contains(w.UserId));
            foreach (UserRoleMapping urm in userRoleMappings)
            {
                userCenterContext.UserRoleMappings.Attach(urm);
                userCenterContext.UserRoleMappings.Remove(urm);
            }
            var userDepartmentMappings = userCenterContext.UserDepartmentMappings.Where(w => userIds.Contains(w.UserId));
            foreach (UserDepartmentMapping udm in userDepartmentMappings)
            {
                userCenterContext.UserDepartmentMappings.Attach(udm);
                userCenterContext.UserDepartmentMappings.Remove(udm);
            }
            var userBasics = userCenterContext.UserBasics.Where(w => userIds.Contains(w.UserId));
            foreach (var item in userBasics)
            {
                item.Delete = true;
                item.DeleteTime = DateTime.Now;
            }
            return userCenterContext.SaveChanges();
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
