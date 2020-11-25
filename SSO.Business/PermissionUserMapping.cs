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
        public int DeleteAndInsertMany(string userId, IEnumerable<string> permissions)
        {
            return instance.DeleteAndInsertMany(userId, permissions);
        }
        public IEnumerable<Data.Models.PermissionUserMapping> GetByUser(string userId)
        {
            return instance.GetByUser(userId);
        }
    }
}
