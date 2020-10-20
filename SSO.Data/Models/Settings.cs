using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSO.Data.Models
{
    public class Settings : SqlBase
    {
        public Settings() : base("settings.sql.xml") { }
        public string UserId { get; set; }
        public string Lang { get; set; }

        public Settings GetByUserId(string userId)
        {
            int count = 0;
            return base.QueryObject<Settings>("get-by-userId", new { UserId = userId }, ref count);
        }
    }
}
