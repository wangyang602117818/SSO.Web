using SSO.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SSO.Business
{
    public class Department : ModelBase<Data.Models.Department>
    {
        public Department() : base(new Data.Models.Department()) { }
        public int Update(int id, string code, string name, string description, int order, string parentCode, string companyCode, int layer)
        {
            Data.Models.Department dept = GetById(id);
            if (dept == null) return 0;
            string oldCode = dept.Code;
            dept.Code = code;
            dept.Name = name;
            dept.Description = description;
            dept.Order = order;
            dept.ParentCode = parentCode;
            dept.CompanyCode = companyCode;
            dept.Layer = layer;
            dept.UpdateTime = DateTime.Now;
            return instance.UpdateDepartment(oldCode, dept);
        }
        public Data.Models.Department GetByCode(string code)
        {
            return instance.GetByCode(code);
        }
        public List<DepartmentData> GetDepartment(string companyCode)
        {
            List<Data.Models.Department> list = instance.GetDepartment(companyCode);
            return GetDepartmentInner(list, null);
        }
        public int CountDepartmentByCompanyCode(string companyCode)
        {
            return instance.CountDepartmentByCompanyCode(companyCode);
        }
        public int Delete(int id)
        {
            List<int> ids = new List<int>() { id };
            Data.Models.Department dept = instance.GetById<Data.Models.Department>(id);
            //递归删除下级部门
            GetSubDepartmentIds(dept.Code, ref ids);
            return Delete(ids);
        }
        private List<DepartmentData> GetDepartmentInner(List<Data.Models.Department> list, string parentCode)
        {
            List<Data.Models.Department> depts = list.Where(w => w.ParentCode == parentCode).ToList();
            if (depts.Count == 0) return new List<DepartmentData>();
            List<DepartmentData> result = depts.Select(s => new DepartmentData()
            {
                Id = s.Id,
                Code = s.Code,
                Name = s.Name,
                Description = s.Description,
                Order = s.Order,
                Layer = s.Layer,
                ParentCode = s.ParentCode,
                Children = GetDepartmentInner(list, s.Code)
            }).OrderBy(o => o.Order).ToList();
            return result;
        }
        private void GetSubDepartmentIds(string code, ref List<int> ids)
        {
            List<Data.Models.Department> subDepts = instance.GetByParentCode(code);
            if (subDepts == null || subDepts.Count == 0) return;
            foreach (var item in subDepts)
            {
                ids.Add(item.Id);
                GetSubDepartmentIds(item.Code, ref ids);
            }
        }
    }
}
