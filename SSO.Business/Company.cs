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
            userCenterContext.Companys.Add(new Data.Models.Company()
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
            var query = from company in userCenterContext.Companys select company;
            if (!string.IsNullOrEmpty(keyword)) query = query.Where(w => w.Code.Contains(keyword) || w.Name.Contains(keyword) || w.Description.Contains(keyword));
            count = query.Count();
            return query.OrderByDescending(o => o.CreateTime).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        }
        public IEnumerable<Data.Models.Company> GetAll(string filter)
        {
            var query = from company in userCenterContext.Companys select company;
            if (!string.IsNullOrEmpty(filter)) query = query.Where(w => w.Name.Contains(filter));
            return query.OrderBy(o=>o.Name).ToList();
        }
        public Data.Models.Company GetById(int id)
        {
            return userCenterContext.Companys.Where(r => r.Id == id).FirstOrDefault();
        }
        public Data.Models.Company GetByCode(string code)
        {
            return userCenterContext.Companys.Where(c => c.Code == code).FirstOrDefault();
        }
        public int Delete(IEnumerable<int> ids)
        {
            foreach (int id in ids)
            {
                Data.Models.Company company = new Data.Models.Company() { Id = id };
                DbEntityEntry<Data.Models.Company> entry = userCenterContext.Entry<Data.Models.Company>(company);
                entry.State = EntityState.Deleted;
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
            company.Code = code;
            company.Name = name;
            company.Description = description;
            company.Order = order;
            company.UpdateTime = DateTime.Now;
            return userCenterContext.SaveChanges();
        }
    }
}
