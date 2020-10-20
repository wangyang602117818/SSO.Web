using Newtonsoft.Json;
using SSO.Util.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace SSO.Business
{
    public class Company : ModelBase<Data.Models.Company>
    {
        public Company() : base(new Data.Models.Company()) { }
        public Data.Models.Company GetByCode(string code)
        {
            return instance.GetByCode(code);
        }
    }
}
