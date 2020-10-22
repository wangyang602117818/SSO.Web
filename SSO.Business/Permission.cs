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
        public int DeleteMany(string origin)
        {
            return instance.DeleteMany(origin);
        }
        public int InsertMany(string origin, IEnumerable<string> names)
        {
            return instance.InserMany(origin, names);
        }
        public List<Data.Models.Permission> GetAll()
        {
            return instance.GetAll();
        }
        public int CheckPermission(string userId, string permission)
        {
            return (int)instance.CheckPermission(userId, permission);
        }
    }
}
