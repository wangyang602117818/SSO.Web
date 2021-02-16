using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SSO.Web.Models
{
    public class SchedulingModel
    {
        private List<int> triggerIds = new List<int>();
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public string Api { get; set; }
        public int? Trigger { get; set; }
        public List<int> TriggerIds { get => triggerIds; set => triggerIds = value; }
    }
    public class UpdateSchedulingModel: SchedulingModel
    {
        [Required]
        public int Id { get; set; }
    }
}