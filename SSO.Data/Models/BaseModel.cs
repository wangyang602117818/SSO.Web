using System;
using System.ComponentModel.DataAnnotations;

namespace SSO.Data.Models
{
    public class BaseModel
    {
        [Key]
        public int Id { get; set; }
        public DateTime? UpdateTime { get; set; }
        public DateTime? CreateTime { get; set; }
    }
}
