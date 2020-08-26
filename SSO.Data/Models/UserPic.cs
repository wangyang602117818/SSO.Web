using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSO.Data.Models
{
    public class UserPic: BaseModel
    {
        [Index(IsUnique = true)]
        [StringLength(24)]
        [Required]
        public string FileId { get; set; }
        [StringLength(512)]
        public string FileName { get; set; }
    }
}
