using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SSO.Web.Models
{
    public class PermissionModel
    {
        [Required]
        public string Origin { get; set; }
        public List<string> Names { get; set; }
    }
    public class RolePermissionModel
    {
        [Required]
        public string Role { get; set; }
        public List<string> Names { get; set; }
    }
    public class UserPermissionModel
    {
        [Required]
        public string User { get; set; }
        public List<string> Names { get; set; }
    }
}