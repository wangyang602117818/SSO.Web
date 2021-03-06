﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSO.Business
{
    public class PermissionRoleMapping : ModelBase<Data.Models.PermissionRoleMapping>
    {
        public PermissionRoleMapping() : base(new Data.Models.PermissionRoleMapping()) { }
        public int DeleteAndInsertMany(string role, IEnumerable<string> permissions)
        {
            return instance.DeleteAndInsertMany(role, permissions);
        }
        public IEnumerable<Data.Models.PermissionRoleMapping> GetByRole(string role)
        {
            return instance.GetByRole(role);
        }
    }
}
