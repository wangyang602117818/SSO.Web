using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SSO.Web.Models
{
    public class NavigationModel
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Url { get; set; }
        public string IconUrl { get; set; }
    }
}