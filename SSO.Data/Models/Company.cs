using SSO.Util.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SSO.Data.Models
{
    public class Company : SqlBase
    {
        public Company() : base("company.sql.xml") { }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Order { get; set; }

        public Company GetByCode(string code)
        {
            int count = 0;
            return base.QueryObject<Company>("get-by-code", new { Code = code }, ref count);
        }
    }
}
