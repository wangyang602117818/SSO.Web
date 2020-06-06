﻿using SSO.Model;
using System;
using System.Collections.Generic;
using System.Data;
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
        public int Update(int id, string code, string name, string description, int order, string parentCode, int layer)
        {
            Data.Models.Department department = GetById(id);
            if (department == null) return 0;
            if (department.Code != code && GetByCode(code) != null) return 0;
            if (department.Code != code)
            {
                var departments = userCenterContext.Departments.Where(w => w.ParentCode == department.Code);
                foreach(var item in departments)
                {
                    item.ParentCode = code;
                    item.UpdateTime = DateTime.Now;
                }
                var userDepartmentMappings = userCenterContext.UserDepartmentMappings.Where(w => w.DepartmentCode == department.Code);
                foreach(var item in userDepartmentMappings)
                {
                    item.DepartmentCode = code;
                    item.UpdateTime = DateTime.Now;
                }
            }
            department.Code = code;
            department.Name = name;
            department.Description = description;
            department.Order = order;
            department.ParentCode = parentCode;
            department.Layer = layer;
            department.UpdateTime = DateTime.Now;
            return userCenterContext.SaveChanges();
        }
        public Data.Models.Department GetById(int id)
        {
            return userCenterContext.Departments.Where(r => r.Id == id).FirstOrDefault();
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
        public int Delete(int id)
        {
            List<int> ids = new List<int>() { id };
            Data.Models.Department dept = userCenterContext.Departments.Where(w => w.Id == id).FirstOrDefault();
            //递归删除下级部门
            GetSubDepartmentIds(dept.Code, ref ids);
            foreach (var item in ids)
            {
                var deptList = from it in userCenterContext.Departments where ids.Contains(it.Id) select it;
                if (deptList.Count() > 0)
                {
                    userCenterContext.Departments.RemoveRange(deptList);
                }
            }
            return userCenterContext.SaveChanges();
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
            List<Data.Models.Department> subDepts = userCenterContext.Departments.Where(w => w.ParentCode == code).ToList();
            if (subDepts.Count == 0) return;
            foreach (var item in subDepts)
            {
                ids.Add(item.Id);
                GetSubDepartmentIds(item.Code, ref ids);
            }
        }
    }
}
