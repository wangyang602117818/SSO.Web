using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SSO.Web.Models
{
    public class PermissionModel
    {
        [Required]
        public string Origin { get; set; }
        public List<string> Names { get; set; }
    }
}