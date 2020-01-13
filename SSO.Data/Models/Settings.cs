using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSO.Data.Models
{
    public class Settings : BaseModel
    {
        [Index(IsUnique = true)]
        [StringLength(50)]
        [Required]
        public string UserId { get; set; }
        [StringLength(8)]
        [Required]
        public string Lang { get; set; }
    }
}
