using System;
using System.Collections.Generic;
using System.Linq;

namespace SSO.Business
{
    public class Role : ModelBase
    {
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
    }
}
