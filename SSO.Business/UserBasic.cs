using SSO.Data.Models;
using SSO.Model;
using SSO.Util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SSO.Business
{
    public class UserBasic : ModelBase
    {
        public int Insert(string userId, string userName, string mobile, string email, string companyCode, string idCard, char sex, List<string> departments, List<string> roles)
        {
            string companyName = "", departmentName = "", roleName = "";
            companyName = userCenterContext.Companies.Where(w => w.Code == companyCode).FirstOrDefault().Name;
            if (departments != null)
            {
                foreach (string department in departments)
                {
                    if (department.IsNullOrEmpty()) continue;
                    departmentName += userCenterContext.Departments.Where(w => w.Code == department).FirstOrDefault().Name + ",";
                    userCenterContext.UserDepartmentMappings.Add(new Data.Models.UserDepartmentMapping()
                    {
                        UserId = userId,
                        DepartmentCode = department,
                        UpdateTime = DateTime.Now,
                        CreateTime = DateTime.Now
                    });
                }
            }
            if (roles != null)
            {
                foreach (string role in roles)
                {
                    if (role.IsNullOrEmpty()) continue;
                    roleName += role + ",";
                    userCenterContext.UserRoleMappings.Add(new Data.Models.UserRoleMapping()
                    {
                        UserId = userId,
                        Role = role,
                        UpdateTime = DateTime.Now,
                        CreateTime = DateTime.Now
                    });
                }
            }
            userCenterContext.UserBasics.Add(new Data.Models.UserBasic()
            {
                UserId = userId,
                UserName = userName,
                PassWord = AppSettings.defaultPassword.GetSha256(),
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
        public int Update(int id, string userId, string userName, string mobile, string email, string companyCode, string idCard, char sex, List<string> departments, List<string> roles)
        {
            //获取原始 user 信息
            Data.Models.UserBasic userBasic = userCenterContext.UserBasics.Where(r => r.Id == id).FirstOrDefault();
            //删除原始 role 信息
            if (roles != null)
            {
                var userRoleMappings = userCenterContext.UserRoleMappings.Where(w => w.UserId == userBasic.UserId);
                foreach (UserRoleMapping urm in userRoleMappings)
                {
                    userCenterContext.UserRoleMappings.Attach(urm);
                    userCenterContext.UserRoleMappings.Remove(urm);
                }
            }
            //删除原始 department 信息
            var userDepartmentMappings = userCenterContext.UserDepartmentMappings.Where(w => w.UserId == userBasic.UserId);
            foreach (Data.Models.UserDepartmentMapping udm in userDepartmentMappings)
            {
                userCenterContext.UserDepartmentMappings.Attach(udm);
                userCenterContext.UserDepartmentMappings.Remove(udm);
            }
            string companyName = "", departmentName = "", roleName = "";
            //获取新公司名称
            companyName = userCenterContext.Companies.Where(w => w.Code == companyCode).FirstOrDefault().Name;
            //添加新 department
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
            //添加新 role
            if (roles != null)
            {
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
            }
            //修改 user
            if (userBasic.UserId != userId && GetUser(userId) != null) return 0;
            userBasic.UserId = userId;
            userBasic.UserName = userName;
            userBasic.Mobile = mobile;
            userBasic.Email = email;
            userBasic.CompanyCode = companyCode;
            userBasic.IdCard = idCard;
            userBasic.Sex = sex.ToString();
            userBasic.CompanyName = companyName;
            userBasic.DepartmentName = departmentName.TrimEnd(',');
            if (roles != null) userBasic.RoleName = roleName.TrimEnd(',');
            userBasic.IsModified = true;
            userBasic.UpdateTime = DateTime.Now;
            return userCenterContext.SaveChanges();
        }
        public Data.Models.UserBasic GetUser(string userId)
        {
            return userCenterContext.UserBasics.Where(r => r.UserId == userId).FirstOrDefault();
        }
        public SSO.Model.UserBasicData GetUserUpdate(string userId)
        {
            Data.Models.UserBasic userBasic = userCenterContext.UserBasics.Where(r => r.UserId == userId).FirstOrDefault();
            if (userBasic == null) return null;
            IQueryable<string> departments = userCenterContext.UserDepartmentMappings.Where(w => w.UserId == userId).Select(s => s.DepartmentCode);
            IQueryable<string> roles = userCenterContext.UserRoleMappings.Where(w => w.UserId == userId).Select(s => s.Role);
            return new Model.UserBasicData()
            {
                Id = userBasic.Id,
                UserId = userBasic.UserId,
                UserName = userBasic.UserName,
                CompanyName = userBasic.CompanyName,
                CompanyCode = userBasic.CompanyCode,
                Mobile = userBasic.Mobile,
                Email = userBasic.Email,
                IdCard = userBasic.IdCard,
                Sex = userBasic.Sex,
                IsModified = userBasic.IsModified,
                Delete = userBasic.Delete,
                DepartmentCode = departments.ToList(),
                DepartmentName = userBasic.DepartmentName.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList(),
                Role = roles.ToList(),
                CreateTime = userBasic.CreateTime
            };
        }
        public int RemoveUser(IEnumerable<string> userIds)
        {
            //删除 role 信息
            var userRoleMappings = userCenterContext.UserRoleMappings.Where(w => userIds.Contains(w.UserId));
            foreach (UserRoleMapping urm in userRoleMappings)
            {
                userCenterContext.UserRoleMappings.Attach(urm);
                userCenterContext.UserRoleMappings.Remove(urm);
            }
            //删除 department 信息
            var userDepartmentMappings = userCenterContext.UserDepartmentMappings.Where(w => userIds.Contains(w.UserId));
            foreach (Data.Models.UserDepartmentMapping udm in userDepartmentMappings)
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
        public int RestoreUser(IEnumerable<string> userIds)
        {
            var userBasics = userCenterContext.UserBasics.Where(w => userIds.Contains(w.UserId));
            foreach (var item in userBasics)
            {
                item.Delete = false;
            }
            return userCenterContext.SaveChanges();
        }
        public int DeleteUser(IEnumerable<string> userIds)
        {
            var userBasics = userCenterContext.UserBasics.Where(w => userIds.Contains(w.UserId));
            foreach (var item in userBasics)
            {
                userCenterContext.UserBasics.Attach(item);
                userCenterContext.UserBasics.Remove(item);
            }
            return userCenterContext.SaveChanges();
        }
        public List<Data.Models.UserBasic> GetBasic(ref int count, string keyword = "", bool delete = false, int pageIndex = 1, int pageSize = 15)
        {
            var query = from userBasic in userCenterContext.UserBasics where userBasic.Delete == delete select userBasic;
            if (!string.IsNullOrEmpty(keyword)) query = query.Where(w => w.UserName.ToLower().Contains(keyword.ToLower()) || w.UserId.ToLower().Contains(keyword.ToLower()));
            count = query.Count();
            return query.OrderByDescending(o => o.CreateTime).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        }
        public Data.Models.UserBasic Login(string userId, string password)
        {
            var res = from user in userCenterContext.UserBasics
                      join company in userCenterContext.Companies
                      on user.CompanyCode equals company.Code
                      where user.UserId == userId && user.PassWord == password
                      select user;
            return res.ToList().FirstOrDefault();
        }
        public int UpdatePassword(string userId, string olpassword, string password)
        {
            Data.Models.UserBasic userBasic = GetUser(userId);
            if (userBasic.PassWord != olpassword.GetSha256()) return -1;
            if (userBasic == null) return 0;
            userBasic.PassWord = password.GetSha256();
            return userCenterContext.SaveChanges();
        }
        public int ResetPassword(IEnumerable<string> userIds)
        {
            var userBasics = userCenterContext.UserBasics.Where(w => userIds.Contains(w.UserId));
            foreach (var item in userBasics)
            {
                item.PassWord = AppSettings.defaultPassword.GetSha256();
                item.IsModified =true;
            }
            return userCenterContext.SaveChanges();
        }
        public IQueryable<DateCountItem> UserRecordInByDay(DateTime minDateTime, bool delete)
        {
            if (delete)
            {
                return userCenterContext.UserBasics.Where(w => (w.DeleteTime >= minDateTime.Date && w.Delete == delete)).Select(s => new
                {
                    s.DeleteTime.Value.Year,
                    s.DeleteTime.Value.Month,
                    s.DeleteTime.Value.Day,
                }).GroupBy(x => new { x.Year, x.Month, x.Day }, (key, group) => new
                  DateCountItem
                {
                    date = key.Year + "-" + key.Month + "-" + key.Day,
                    count = group.Count(),
                    type = "delete"
                });
            }
            else
            {
                return userCenterContext.UserBasics.Where(w => (w.CreateTime >= minDateTime.Date && w.Delete == delete)).Select(s => new
                {
                    s.CreateTime.Value.Year,
                    s.CreateTime.Value.Month,
                    s.CreateTime.Value.Day
                }).GroupBy(x => new { x.Year, x.Month, x.Day }, (key, group) => new
                  DateCountItem
                {
                    date = key.Year + "-" + key.Month + "-" + key.Day,
                    count = group.Count(),
                    type = "insert"
                })
                .OrderBy(o => o.date);
            }
        }
        public IQueryable<DateCountItem> UserRecordByMonth(DateTime minDateTime, bool delete)
        {
            if (delete)
            {
                return userCenterContext.UserBasics.Where(w => (w.DeleteTime >= minDateTime.Date && w.Delete == delete)).Select(k => new
                {
                    k.DeleteTime.Value.Year,
                    k.DeleteTime.Value.Month
                }).GroupBy(x => new { x.Year, x.Month }, (key, group) => new
                       DateCountItem
                {
                    date = key.Year + "-" + key.Month,
                    count = group.Count(),
                    type = "delete"
                });
            }
            else
            {
                return userCenterContext.UserBasics.Where(w => (w.CreateTime >= minDateTime.Date && w.Delete == delete)).Select(k => new
                {
                    k.CreateTime.Value.Year,
                    k.CreateTime.Value.Month
                })
                    .GroupBy(x => new { x.Year, x.Month }, (key, group) => new
                      DateCountItem
                    {
                        date = key.Year + "-" + key.Month,
                        count = group.Count(),
                        type = "insert"
                    })
                .OrderBy(o => o.date);
            }
        }
        public IQueryable<DateCountItem> UserRecordByYear(DateTime minDateTime, bool delete)
        {
            if (delete)
            {
                return userCenterContext.UserBasics
                 .Where(w => (w.DeleteTime >= minDateTime.Date && w.Delete == delete))
                 .Select(k => new
                 {
                     k.DeleteTime.Value.Year
                 })
                 .GroupBy(x => new { x.Year }, (key, group) => new
                   DateCountItem
                 {
                     date = key.Year.ToString(),
                     count = group.Count(),
                     type = "delete"
                 });
            }
            else
            {
                return userCenterContext.UserBasics
                 .Where(w => (w.CreateTime >= minDateTime.Date && w.Delete == delete))
                 .Select(k => new
                 {
                     k.CreateTime.Value.Year
                 })
                 .GroupBy(x => new { x.Year }, (key, group) => new
                    DateCountItem
                 {
                     date = key.Year.ToString(),
                     count = group.Count(),
                     type = "insert"
                 })
                 .OrderBy(o => o.date);
            }
        }
        public IQueryable<DateCountItem> GetUserRatio()
        {
            return userCenterContext.UserBasics
                 .Where(w => w.Delete == false)
                 .GroupBy(x => new { x.Sex }, (key, group) => new DateCountItem
                 {
                     type = key.Sex,
                     count = group.Count()
                 });
        }
        public IQueryable<DateCountItem> GetUserCompanyRatio()
        {
            return userCenterContext.UserBasics
                 .Where(w => w.Delete == false)
                 .GroupBy(x => new { x.CompanyName }, (key, group) => new DateCountItem
                 {
                     type = key.CompanyName,
                     count = group.Count()
                 });
        }
    }
}
