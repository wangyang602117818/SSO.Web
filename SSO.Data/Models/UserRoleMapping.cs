using System.ComponentModel.DataAnnotations;

namespace SSO.Data.Models
{
    public class UserRoleMapping : BaseModel
    {
        [StringLength(50)]
        public string UserId { get; set; }
        [StringLength(30)]
        public string Role { get; set; }
    }
}
