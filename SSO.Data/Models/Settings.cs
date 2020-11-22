using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSO.Data.Models
{
    public class Settings : ModelBase
    {
        public Settings() { }
        public string UserId { get; set; }
        public string Lang { get; set; }

        public Settings GetByUserId(string userId)
        {
            return base.QueryObject<Settings>("get-by-userId", new { UserId = userId }, null);
        }
    }
}
