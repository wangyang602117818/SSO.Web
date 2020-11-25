using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSO.Business
{
    public class Permission : ModelBase<Data.Models.Permission>
    {
        public Permission() : base(new Data.Models.Permission()) { }
        public int DeleteAndInsertMany(string origin, IEnumerable<string> names)
        {
            return instance.DeleteAndInsertMany(origin, names);
        }
        public IEnumerable<Data.Models.Permission> GetAll()
        {
            return instance.GetAll();
        }
        public int CheckPermission(string userId, string permission)
        {
            return (int)instance.CheckPermission(userId, permission);
        }
    }
}
