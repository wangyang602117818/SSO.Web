using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace SSO.Business
{
    public class Role : ModelBase
    {
        public Data.Models.Role GetByRoleName(string name)
        {
            var query = from role in userCenterContext.Roles where role.Name == name select role;
            return query.FirstOrDefault();
        }
        public Data.Models.Role GetById(int id)
        {
            return userCenterContext.Roles.Where(r => r.Id == id).FirstOrDefault();
        }
        /// <summary>
        /// 更新,并且保证name不重复
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public int Update(int id, string name, string description)
        {
            Data.Models.Role role = GetById(id);
            if (role == null) return 0;
            if (role.Name != name && GetByRoleName(name) != null) return 0;
            role.Name = name;
            role.Description = description;
            role.UpdateTime = DateTime.Now;
            return userCenterContext.SaveChanges();
        }
        public int Insert(string name, string description)
        {
            userCenterContext.Roles.Add(new Data.Models.Role()
            {
                Name = name,
                Description = description,
                UpdateTime = DateTime.Now,
                CreateTime = DateTime.Now
            });
            return userCenterContext.SaveChanges();
        }
        public IEnumerable<Data.Models.Role> GetList(ref int count, string keyword = "", int pageIndex = 1, int pageSize = 15)
        {
            var query = from role in userCenterContext.Roles select role;
            if (!string.IsNullOrEmpty(keyword)) query = query.Where(w => w.Name.Contains(keyword) || w.Description.Contains(keyword));
            count = query.Count();
            return query.OrderByDescending(o => o.CreateTime).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        }

        public int Delete(IEnumerable<int> ids)
        {
            foreach (int id in ids)
            {
                Data.Models.Role role = new Data.Models.Role() { Id = id };
                DbEntityEntry<Data.Models.Role> entry = userCenterContext.Entry<Data.Models.Role>(role);
                entry.State = EntityState.Deleted;
            }
            return userCenterContext.SaveChanges();
        }
    }
}
