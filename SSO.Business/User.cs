using SSO.Data.Models;
using SSO.Model;
using SSO.Util.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;

namespace SSO.Business
{
    public class User : ModelBase<Data.Models.User>
    {
        public User() : base(new Data.Models.User()) { }
        public string defaultPassword = AppSettings.GetValue("defaultPassword");
        Company company = new Company();
        Department department = new Department();
        UserDepartmentMapping departmentMapping = new UserDepartmentMapping();
        UserRoleMapping roleMapping = new UserRoleMapping();
        public int InsertUserDepartmentRole(string userId, string userName, string mobile, string email, string companyCode, string idCard, char sex, List<string> departments, List<string> roles)
        {
            string departmentName = "",
                roleName = "",
                companyName = company.GetByCode(companyCode).Name;
            foreach (string dept in departments)
            {
                if (dept.IsNullOrEmpty()) continue;
                departmentName += department.GetByCode(dept).Name + ",";
            }
            foreach (string role in roles)
            {
                if (role.IsNullOrEmpty()) continue;
                roleName += role + ",";
            }
            Data.Models.User u = new Data.Models.User
            {
                UserId = userId,
                UserName = userName,
                PassWord = defaultPassword.GetSha256(),
                CompanyCode = companyCode,
                Mobile = mobile,
                Email = email,
                IdCard = idCard,
                Sex = sex.ToString(),
                IsModified = false,
                IsDelete = false,
                CreateTime = DateTime.Now,
                CompanyName = companyName,
                DepartmentName = departmentName.TrimEnd(','),
                RoleName = roleName.TrimEnd(',')
            };
            return instance.InsertUserDepartmentRole(u, departments, roles);
        }
        public int UpdateUserDepartmentRole(int id, string userId, string userName, string mobile, string email, string companyCode, string idCard, char sex, List<string> departments, List<string> roles)
        {
            //获取原始 user 信息
            Data.Models.User user = instance.GetById<Data.Models.User>(id);
            string oldUserId = user.UserId;
            //修改的userId数据库已存在
            if (user.UserId != userId && GetUser(userId) != null) return 0;
            string departmentName = "",
                roleName = "",
                companyName = company.GetByCode(companyCode).Name;
            foreach (string dept in departments)
            {
                if (dept.IsNullOrEmpty()) continue;
                departmentName += department.GetByCode(dept).Name + ",";
            }
            if (roles == null)
            {
                roles = roleMapping.GetByUserId(userId).ToList();
            }
            foreach (string role in roles)
            {
                if (role.IsNullOrEmpty()) continue;
                roleName += role + ",";
            }
            user.UserId = userId;
            user.UserName = userName;
            user.Mobile = mobile;
            user.Email = email;
            user.CompanyCode = companyCode;
            user.IdCard = idCard;
            user.Sex = sex.ToString();
            user.CompanyName = companyName;
            user.DepartmentName = departmentName.TrimEnd(',');
            user.RoleName = roleName.TrimEnd(',');
            user.IsModified = true;
            user.UpdateTime = DateTime.Now;
            return instance.UpdateUserDepartmentRole(oldUserId, user, departments, roles);
        }
        public Data.Models.User GetUser(string userId)
        {
            return instance.GetByUserId(userId);
        }
        public SSO.Model.UserData GetUserUpdate(string userId)
        {
            Data.Models.User user = instance.GetByUserId(userId);
            if (user == null) return null;
            IEnumerable<string> departments = departmentMapping.GetByUserId(userId);
            IEnumerable<string> roles = roleMapping.GetByUserId(userId);
            var departmentName = user.DepartmentName == null ? new string[] { } : user.DepartmentName.Split(new char[] { ',' },
                StringSplitOptions.RemoveEmptyEntries);
            return new Model.UserData()
            {
                Id = user.Id,
                UserId = user.UserId,
                UserName = user.UserName,
                CompanyName = user.CompanyName,
                CompanyCode = user.CompanyCode,
                Mobile = user.Mobile,
                Email = user.Email,
                IdCard = user.IdCard,
                Sex = user.Sex,
                IsModified = user.IsModified,
                IsDelete = user.IsDelete,
                FileId = user.FileId,
                FileName = user.FileName,
                DepartmentCode = departments.ToList(),
                DepartmentName = departmentName.ToList(),
                Role = roles.ToList(),
                CreateTime = user.CreateTime,
                LastLoginTime = user.LastLoginTime,
                UpdateTime = user.UpdateTime
            };
        }
        public int RemoveUser(IEnumerable<string> userIds)
        {
            return instance.RemoveUser(userIds);
        }
        public int RestoreUser(IEnumerable<string> userIds)
        {
            return instance.RestoreUser(userIds);
        }
        public int DeleteUser(IEnumerable<string> userIds)
        {
            int count = 0;
            try
            {
                count = instance.DeleteUser(userIds);
            }
            catch (Exception ex)
            {
                if (ex.ToString().Contains("constraint"))
                    count = -1;
            }
            return count;
        }
        public Data.Models.User Login(string userId, string password)
        {
            return instance.Login(userId, password);
        }
        public int UpdatePassword(string userId, string oldPassword, string password)
        {
            Data.Models.User user = instance.GetByUserId(userId);
            if (user.PassWord != oldPassword.GetSha256()) return -1;
            if (user == null) return 0;
            return instance.UpdatePassword(new List<string>() { userId }, password.GetSha256());
        }
        public int UpdateFileId(string userId, string fileId, string fileName)
        {
            return instance.UpdateFile(userId, fileId, fileName);
        }
        public int ResetPassword(IEnumerable<string> userIds)
        {
            return instance.UpdatePassword(userIds, defaultPassword.GetSha256());
        }
        public IEnumerable<DateCountItem> UserRecordInByDay(DateTime minDateTime, bool delete)
        {
            return instance.GroupByDate(new { IsDelete = delete, CreateTime = minDateTime });
        }
        public IEnumerable<DateCountItem> GetUserRatio()
        {
            return instance.GroupBySex();
        }
        public IEnumerable<DateCountItem> GetUserCompanyRatio()
        {
            return instance.GroupByCompany();
        }
    }
}
