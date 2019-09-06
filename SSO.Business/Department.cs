using SSO.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SSO.Business
{
    public class Department : ModelBase
    {
        public int Insert(string code, string name, string description, string companyCode, int order, int layer, string parentCode)
        {
            userCenterContext.Departments.Add(new Data.Models.Department()
            {
                Code = code,
                Name = name,
                Description = description,
                CompanyCode = companyCode,
                Order = order,
                Layer = layer,
                ParentCode = parentCode,
                UpdateTime = DateTime.Now,
                CreateTime = DateTime.Now
            });
            return userCenterContext.SaveChanges();
        }
        public Data.Models.Department GetByCode(string code)
        {
            return userCenterContext.Departments.Where(c => c.Code == code).FirstOrDefault();
        }
        public List<DepartmentData> GetDepartment(string companyCode)
        {
            List<Data.Models.Department> list = userCenterContext.Departments.Where(c => c.CompanyCode == companyCode).ToList();
            return GetDepartmentInner(list, "");
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
                Departments = GetDepartmentInner(list, s.Code)
            }).OrderBy(o => o.Order).ToList();
            return result;
        }
    }
}
