using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSO.Business
{
    public class PermissionUserMapping : ModelBase<Data.Models.PermissionUserMapping>
    {
        public PermissionUserMapping() : base(new Data.Models.PermissionUserMapping()) { }
        public int DeleteMany(string role)
        {
            return instance.DeleteMany(role);
        }
        public int InsertMany(string user, IEnumerable<string> permissions)
        {
            return instance.InsertMany(user, permissions);
        }
        public List<Data.Models.PermissionUserMapping> GetByUser(string userId)
        {
            return instance.GetByUser(userId);
        }
    }
}
