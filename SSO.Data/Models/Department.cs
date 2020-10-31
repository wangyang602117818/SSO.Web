using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSO.Data.Models
{
    public class Department : SqlBase
    {
        public Department() { }
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
        public int UpdateDepartment(string oldParentCode, Department department)
        {
            List<string> nodes = new List<string>() { };
            List<object> objs = new List<object>() { };
            if (oldParentCode != department.Code)
            {
                nodes.Add("update-department-parent-code");
                objs.Add(
                    new { OldParentCode = oldParentCode, NewParentCode = department.Code });
            }
            nodes.Add("update");
            objs.Add(department);
            return base.ExecuteNonQueryTransaction(nodes, objs, null);
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
