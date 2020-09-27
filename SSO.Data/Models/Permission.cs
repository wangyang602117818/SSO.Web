using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSO.Data.Models
{
    public class Permission : BaseModel
    {
        [StringLength(512)]
        public string Origin { get; set; }
        [StringLength(50)]
        [Required]
        public string Name { get; set; }
    }
}
