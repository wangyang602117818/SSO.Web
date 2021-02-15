using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SSO.Web.Models
{
    public class TriggerModel
    {
        [Required]
        public string Crons { get; set; }
        public string Description { get; set; }
        public string Description1 { get; set; }

        public DateTime? Activate { get; set; }
        public DateTime? Expire { get; set; }
    }
    public class UpdateTriggerModel: TriggerModel
    {
        [Required]
        public int Id { get; set; }
    }
}