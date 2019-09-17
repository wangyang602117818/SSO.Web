using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SSO.Data.Models
{
    public class UserBasic : BaseModel
    {
        [Index(IsUnique = true)]
        [StringLength(50)]
        [Required]
        public string UserId { get; set; }
        [StringLength(50)]
        [Required]
        public string UserName { get; set; }
        [StringLength(50)]
        public string Mobile { get; set; }
        [StringLength(50)]
        public string Email { get; set; }
        [StringLength(20)]
        public string IdCard { get; set; }
    }
}
