using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSO.Model
{
    public class UserData
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string CompanyCode { get; set; }
        public string CompanyName { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string IdCard { get; set; }
        public string Sex { get; set; }
        public bool IsModified { get; set; }
        public bool IsDelete { get; set; }
        public string FileId { get; set; }
        public string FileName { get; set; }
        public List<string> DepartmentCode { get; set; }
        public List<string> DepartmentName { get; set; }
        public List<string> Role { get; set; }
        public DateTime? CreateTime { get; set; }
        public DateTime? LastLoginTime { get; set; }
        public DateTime? UpdateTime { get; set; }
    }
}
