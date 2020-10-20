using System;
using System.Collections.Generic;
using System.Linq;

namespace SSO.Business
{
    public class Role: ModelBase<Data.Models.Role>
    {
        public Role() : base(new Data.Models.Role()) { }
        public Data.Models.Role GetByName(string name)
        {
            return instance.GetByName(name);
        }

    }
}
