﻿using SSO.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSO.Data.Models
{
    public class UserBasic : SqlBase
    {
        public UserBasic() : base("userbasic.sql.xml") { }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string PassWord { get; set; }
        public string CompanyCode { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string IdCard { get; set; }
        public string Sex { get; set; }
        public bool IsModified { get; set; }
        public bool Delete { get; set; }
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

        public UserBasic GetByUserId(string userId)
        {
            int count = 0;
            return base.QueryObject<UserBasic>("get-by-userId", new { UserId = userId }, ref count);
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
        public UserBasic Login(string userId, string password)
        {
            int count = 0;
            return base.QueryObject<UserBasic>("login", new { UserId = userId, PassWord = password, LastLoginTime = DateTime.Now }, ref count);
        }
        public int UpdatePassword(IEnumerable<string> userIds, string password)
        {
            return base.ExecuteNonQuery("update-password", new { UserIds = userIds, PassWord = password });
        }
        public int UpdateFile(string userId, string fileId, string fileName)
        {
            return base.ExecuteNonQuery("update-file", new { UserId = userId, FileName = fileName, FileId = fileId });
        }
        public List<DateCountItem> GroupBySex()
        {
            int count = 0;
            return base.QueryList<DateCountItem>("group-by-sex", null, ref count);
        }
        public List<DateCountItem> GroupByCompany()
        {
            int count = 0;
            return base.QueryList<DateCountItem>("group-by-company", null, ref count);
        }
        public List<DateCountItem> GroupByDate(object paras)
        {
            int count = 0;
            return base.QueryList<DateCountItem>("group-by-date", paras, ref count);
        }
    }
}
