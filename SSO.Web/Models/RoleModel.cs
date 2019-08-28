using System.ComponentModel.DataAnnotations;

namespace SSO.Web.Models
{
    public class RoleModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
    }
    public class UpdateRoleModel : RoleModel
    {
        [Required]
        public int Id { get; set; }
    }
}