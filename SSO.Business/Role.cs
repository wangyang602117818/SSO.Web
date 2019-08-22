using System;
using System.Collections.Generic;
using System.Linq;

namespace SSO.Business
{
    public class Role : ModelBase
    {
        public int Insert(string name, string description)
        {
            userCenterContext.Roles.Add(new Model.Role()
            {
                Name = name,
                Description = description,
                UpdateTime = DateTime.Now,
                CreateTime = DateTime.Now
            });
            return userCenterContext.SaveChanges();
        }
        public IEnumerable<Model.Role> GetList(string keyword = "", int pageIndex = 1, int pageSize = 15)
        {
            var filter = userCenterContext.Roles;
            if (!string.IsNullOrEmpty(keyword)) return userCenterContext.Roles.Skip(pageIndex * pageSize).Take(pageSize);
            return userCenterContext.Roles.Where(w => w.Name.Contains(keyword) || w.Description.Contains(keyword)).Skip(pageIndex * pageSize).Take(pageSize).ToList();
        }
    }
}
