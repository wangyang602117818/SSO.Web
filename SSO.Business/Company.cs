using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace SSO.Business
{
    public class Company : ModelBase
    {
        public int Insert(string code, string name, string description, int order)
        {
            userCenterContext.Companies.Add(new Data.Models.Company()
            {
                Code = code,
                Name = name,
                Description = description,
                Order = order,
                UpdateTime = DateTime.Now,
                CreateTime = DateTime.Now
            });
            return userCenterContext.SaveChanges();
        }
        public IEnumerable<Data.Models.Company> GetList(ref int count, string keyword = "", int pageIndex = 1, int pageSize = 15)
        {
            var query = from company in userCenterContext.Companies select company;
            if (!string.IsNullOrEmpty(keyword)) query = query.Where(w => w.Code.ToLower().Contains(keyword.ToLower()) || w.Name.ToLower().Contains(keyword.ToLower()) || w.Description.ToLower().Contains(keyword.ToLower()));
            count = query.Count();
            return query.OrderByDescending(o => o.CreateTime).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        }
        public IEnumerable<Data.Models.Company> GetAll(string filter)
        {
            var query = from company in userCenterContext.Companies select company;
            if (!string.IsNullOrEmpty(filter)) query = query.Where(w => w.Name.ToLower().Contains(filter.ToLower()));
            return query.OrderBy(o=>o.Name).ToList();
        }
        public Data.Models.Company GetById(int id)
        {
            return userCenterContext.Companies.Where(r => r.Id == id).FirstOrDefault();
        }
        public Data.Models.Company GetByCode(string code)
        {
            return userCenterContext.Companies.Where(c => c.Code == code).FirstOrDefault();
        }
        public int Delete(IEnumerable<int> ids)
        {
            foreach (int id in ids)
            {
                Data.Models.Company com = new Company().GetById(id);
                int count = new Department().CountDepartmentByCompanyCode(com.Code);
                if (count > 0) continue;
                userCenterContext.Companies.Attach(com);
                userCenterContext.Companies.Remove(com);
            }
            return userCenterContext.SaveChanges();
        }
        /// <summary>
        /// 更新,并且保证code不重复
        /// </summary>
        /// <param name="id"></param>
        /// <param name="code"></param>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public int Update(int id, string code, string name, string description, int order)
        {
            Data.Models.Company company = GetById(id);
            if (company == null) return 0;
            if (company.Code != code && GetByCode(code) != null) return 0;
            if (company.Code != code)
            {
                var departments = userCenterContext.Departments.Where(w => w.CompanyCode == company.Code);
                foreach (var item in departments)
                {
                    item.CompanyCode = code;
                    item.UpdateTime = DateTime.Now;
                }
                var users = userCenterContext.UserBasics.Where(w => w.CompanyCode == company.Code);
                foreach(var item in users)
                {
                    item.CompanyCode = code;
                    item.UpdateTime = DateTime.Now;
                }
            }
            company.Code = code;
            company.Name = name;
            company.Description = description;
            company.Order = order;
            company.UpdateTime = DateTime.Now;
            return userCenterContext.SaveChanges();
        }
    }
}
