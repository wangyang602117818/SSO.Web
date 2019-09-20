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
        public string PassWord { get; set; }
        [StringLength(30)]
        public string CompanyCode { get; set; }
        [StringLength(50)]
        public string Mobile { get; set; }
        [StringLength(50)]
        public string Email { get; set; }
        [StringLength(20)]
        public string IdCard { get; set; }
        [StringLength(1)]
        public string Sex { get; set; }
        public bool IsModified { get; set; }
        public bool Delete { get; set; }
    }
}
