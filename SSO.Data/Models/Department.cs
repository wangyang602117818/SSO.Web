using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SSO.Data.Models
{
    public class Department: BaseModel
    {
        [Index(IsUnique = true)]
        [StringLength(30)]
        [Required]
        public string Code { get; set; }
        [StringLength(50)]
        [Required]
        public string Name { get; set; }
        [StringLength(512)]
        public string Description { get; set; }
        public int Order { get; set; }
        public int Layer { get; set; }
        [Required]
        public int ParentCode { get; set; }
        [StringLength(30)]
        [Required]
        public string CompanyCode { get; set; }

        public virtual Company Company { get; set; }
    }
}
