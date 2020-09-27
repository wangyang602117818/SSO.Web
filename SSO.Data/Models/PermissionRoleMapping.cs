using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSO.Data.Models
{
    public class PermissionRoleMapping: BaseModel
    {
        [StringLength(50)]
        public string Role { get; set; }
        [StringLength(50)]
        public string Permission { get; set; }
    }
}
