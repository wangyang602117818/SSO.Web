using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSO.Data.Models
{
    public class Department : SqlBase
    {
        public Department() : base("department.sql.xml") { }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Order { get; set; }
        public int Layer { get; set; }
        public string ParentCode { get; set; }
        public string CompanyCode { get; set; }

        public Department GetByCode(string code)
        {
            int count = 0;
            return base.QueryObject<Department>("get-by-code", new { Code = code }, ref count);
        }
        public List<Department> GetDepartment(string companyCode)
        {
            int count = 0;
            return base.QueryList<Department>("get-department", new { CompanyCode = companyCode }, ref count);
        }
        public int CountDepartmentByCompanyCode(string companyCode)
        {
            return base.ExecuteNonQuery("count-department", new { CompanyCode = companyCode });
        }
        public List<Department> GetByParentCode(string parentCode)
        {
            int count = 0;
            return base.QueryList<Department>("get-by-parentCode", new { ParentCode = parentCode }, ref count);
        }
        public int UpdateDepartmentParentCode(string code, string parentCode)
        {
            return base.ExecuteNonQuery("update-department-parent-code", new { Code = code, ParentCode = parentCode });
        }
    }
}
