using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SSO.Web.Models
{
    public class RoleModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
    }
}