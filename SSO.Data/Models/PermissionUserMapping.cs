using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSO.Data.Models
{
    public class PermissionUserMapping: BaseModel
    {
        [StringLength(50)]
        public string UserId { get; set; }
        [StringLength(50)]
        public string Permission { get; set; }
    }
}
