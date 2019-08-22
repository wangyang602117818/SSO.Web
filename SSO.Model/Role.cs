using System;
using System.ComponentModel.DataAnnotations;

namespace SSO.Model
{
    public class Role
    {
        [Key]
        public int Id { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        [StringLength(512)]
        public string Description { get; set; }
        public DateTime? UpdateTime { get; set; }
        public DateTime? CreateTime { get; set; }
    }
}
