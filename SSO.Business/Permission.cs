using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSO.Business
{
    public class Permission : ModelBase
    {
        public int DeleteMany(string origin)
        {
            var query = from permission in userCenterContext.Permissions where permission.Origin == origin select permission;
            foreach (var item in query)
            {
                userCenterContext.Permissions.Attach(item);
                userCenterContext.Permissions.Remove(item);
            }
            return userCenterContext.SaveChanges();
        }
        public int InsertMany(string origin, IEnumerable<string> names)
        {
            foreach (var item in names)
            {
                userCenterContext.Permissions.Add(new Data.Models.Permission()
                {
                    Origin = origin,
                    Name = item,
                    UpdateTime = DateTime.Now,
                    CreateTime = DateTime.Now
                });
            }
            return userCenterContext.SaveChanges();
        }
        public List<Data.Models.Permission> GetList()
        {
            return userCenterContext.Permissions.ToList();
        }
    }
}
