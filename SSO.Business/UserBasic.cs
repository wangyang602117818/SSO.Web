using SSO.Data.Models;
using SSO.Model;
using SSO.Util.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;

namespace SSO.Business
{
    public class UserBasic : ModelBase<Data.Models.UserBasic>
    {
        public UserBasic() : base(new Data.Models.UserBasic()) { }
        public string defaultPassword = AppSettings.GetValue("defaultPassword");
        Company company = new Company();
        Department department = new Department();
        UserDepartmentMapping departmentMapping = new UserDepartmentMapping();
        UserRoleMapping roleMapping = new UserRoleMapping();
        public int Insert(string userId, string userName, string mobile, string email, string companyCode, string idCard, char sex, List<string> departments, List<string> roles)
        {
            string companyName = "", departmentName = "", roleName = "";
            companyName = company.GetByCode(companyCode).Name;
            if (departments != null)
            {
                foreach (string dept in departments)
                {
                    if (dept.IsNullOrEmpty()) continue;
                    departmentName += department.GetByCode(dept).Name + ",";
                    departmentMapping.Insert(userId, dept);
                }
            }
            if (roles != null)
            {
                foreach (string role in roles)
                {
                    if (role.IsNullOrEmpty()) continue;
                    roleName += role + ",";
                    roleMapping.Insert(userId, role);
                }
            }
            return instance.Insert(new Data.Models.UserBasic()
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
                Delete = false,
                CreateTime = DateTime.Now,
                CompanyName = companyName,
                DepartmentName = departmentName.TrimEnd(','),
                RoleName = roleName.TrimEnd(','),
            });
        }
        public int Update(int id, string userId, string userName, string mobile, string email, string companyCode, string idCard, char sex, List<string> departments, List<string> roles)
        {
            //获取原始 user 信息
            Data.Models.UserBasic user = instance.GetById<Data.Models.UserBasic>(id);
            //删除原始 role 信息
            if (roles != null)
            {
                roleMapping.DeleteByUserId(user.UserId);
            }
            //删除原始 department 信息
            if (department != null)
            {
                departmentMapping.DeleteByUserId(user.UserId);
            }
            string companyName = "", departmentName = "", roleName = "";
            //获取新公司名称
            companyName = company.GetByCode(companyCode).Name;
            //添加新 department
            if (departments != null)
            {
                foreach (string dept in departments)
                {
                    departmentName += department.GetByCode(dept).Name + ",";
                    departmentMapping.Insert(userId, dept);
                }
            }
            //添加新 role
            if (roles != null)
            {
                foreach (string role in roles)
                {
                    roleName += role + ",";
                    roleMapping.Insert(userId, role);
                }
            }
            //修改 user
            if (user.UserId != userId && GetUser(userId) != null) return 0;
            user.UserId = userId;
            user.UserName = userName;
            user.Mobile = mobile;
            user.Email = email;
            user.CompanyCode = companyCode;
            user.IdCard = idCard;
            user.Sex = sex.ToString();
            user.CompanyName = companyName;
            user.DepartmentName = departmentName.TrimEnd(',');
            if (roles != null) user.RoleName = roleName.TrimEnd(',');
            user.IsModified = true;
            user.UpdateTime = DateTime.Now;
            return instance.Update(user);
        }
        public Data.Models.UserBasic GetUser(string userId)
        {
            return instance.GetByUserId(userId);
        }
        public SSO.Model.UserBasicData GetUserUpdate(string userId)
        {
            Data.Models.UserBasic user = instance.GetByUserId(userId);
            if (user == null) return null;
            IEnumerable<string> departments = departmentMapping.GetByUserId(userId);
            IEnumerable<string> roles = roleMapping.GetByUserId(userId);
            var departmentName = user.DepartmentName == null ? new string[] { } : user.DepartmentName.Split(new char[] { ',' },
                StringSplitOptions.RemoveEmptyEntries);
            return new Model.UserBasicData()
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
                Delete = user.Delete,
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
            foreach (string userId in userIds)
            {
                //删除 role 信息
                roleMapping.DeleteByUserId(userId);
                //删除 department 信息
                departmentMapping.DeleteByUserId(userId);
            }
            return instance.RemoveUser(userIds);
        }
        public int RestoreUser(IEnumerable<string> userIds)
        {
            return instance.RestoreUser(userIds);
        }
        public int DeleteUser(IEnumerable<string> userIds)
        {
            return instance.DeleteUser(userIds);
        }
        public Data.Models.UserBasic Login(string userId, string password)
        {
            return instance.Login(userId, password);
        }
        public int UpdatePassword(string userId, string oldPassword, string password)
        {
            Data.Models.UserBasic user = instance.GetByUserId(userId);
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
        public List<DateCountItem> UserRecordInByDay(DateTime minDateTime, bool delete)
        {
            return instance.GroupByDate(new { Delete = delete, CreateTime = minDateTime });
        }
        public List<DateCountItem> GetUserRatio()
        {
            return instance.GroupBySex();
        }
        public List<DateCountItem> GetUserCompanyRatio()
        {
            return instance.GroupByCompany();
        }
    }
}
