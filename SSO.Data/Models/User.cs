using SSO.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSO.Data.Models
{
    public class User : ModelBase
    {
        public User() { }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string PassWord { get; set; }
        public string CompanyCode { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string IdCard { get; set; }
        public string Sex { get; set; }
        public bool IsModified { get; set; }
        public bool IsDelete { get; set; }
        public string FileId { get; set; }
        public string FileName { get; set; }
        public int PermissionCount { get; set; }
        public DateTime? DeleteTime { get; set; }
        public DateTime? LastLoginTime { get; set; }
        /// <summary>
        /// 额外字段,方便显示
        /// </summary>
        public string CompanyName { get; set; }
        /// <summary>
        /// 额外字段,方便显示
        /// </summary>
        public string DepartmentName { get; set; }
        /// <summary>
        /// 额外字段,方便显示
        /// </summary>
        public string RoleName { get; set; }

        public int InsertUserDepartmentRole(User user, List<string> departments, List<string> roles)
        {
            var nodes = new List<string>() { "delete-department-by-userId", "delete-role-by-userId" };
            var datas = new List<object>() { new { OldUserId = user.UserId }, new { OldUserId = user.UserId } };
            if (departments.Count() > 0)
            {
                nodes.Add("insert-department-many");
                datas.Add(new { UserId = user.UserId, Departments = departments });
            }
            if (roles.Count() > 0)
            {
                nodes.Add("insert-role-many");
                datas.Add(new { UserId = user.UserId, Roles = roles });
            }
            nodes.Add("insert");
            datas.Add(user);
            return base.ExecuteTransaction(nodes, datas, null);
        }
        public int UpdateUserDepartmentRole(string oldUserId, User user, List<string> departments, List<string> roles)
        {
            var nodes = new List<string>() { "delete-department-by-userId", "delete-role-by-userId" };
            var datas = new List<object>() { new { OldUserId = oldUserId }, new { OldUserId = oldUserId } };
            if (departments.Count() > 0)
            {
                nodes.Add("insert-department-many");
                datas.Add(new { UserId = user.UserId, Departments = departments });
            }
            if (roles.Count() > 0)
            {
                nodes.Add("insert-role-many");
                datas.Add(new { UserId = user.UserId, Roles = roles });
            }
            nodes.Add("update");
            datas.Add(user);
            return base.ExecuteTransaction(nodes, datas, null);
        }
        public User GetByUserId(string userId)
        {
            return base.QueryObject<User>("get-by-userId", new { UserId = userId }, null);
        }
        public int RemoveUser(IEnumerable<string> userIds)
        {
            return base.ExecuteNonQuery("remove-user", new { UserIds = userIds, DeleteTime = DateTime.Now });
        }
        public int RestoreUser(IEnumerable<string> userIds)
        {
            return base.ExecuteNonQuery("restore-user", new { UserIds = userIds });
        }
        public int DeleteUser(IEnumerable<string> userIds)
        {
            return base.ExecuteNonQuery("delete-user", new { UserIds = userIds });
        }
        public User Login(string userId, string password)
        {
            return base.QueryObject<User>("login", new { UserId = userId, PassWord = password, LastLoginTime = DateTime.Now }, null);
        }
        public int UpdatePassword(IEnumerable<string> userIds, string password)
        {
            return base.ExecuteNonQuery("update-password", new { UserIds = userIds, PassWord = password });
        }
        public int UpdateFile(string userId, string fileId, string fileName)
        {
            return base.ExecuteNonQuery("update-file", new { UserId = userId, FileName = fileName, FileId = fileId });
        }
        public IEnumerable<DateCountItem> GroupBySex()
        {
            int count = 0;
            return base.QueryList<DateCountItem>("group-by-sex", null, null, ref count);
        }
        public IEnumerable<DateCountItem> GroupByCompany()
        {
            int count = 0;
            return base.QueryList<DateCountItem>("group-by-company", null, null, ref count);
        }
        public IEnumerable<DateCountItem> GroupByDate(object paras)
        {
            int count = 0;
            return base.QueryList<DateCountItem>("group-by-date", paras, null, ref count);
        }
    }
}
