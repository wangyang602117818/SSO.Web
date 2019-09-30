using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SSO.Data.Models
{
    public class Role : BaseModel
    {
        [Index(IsUnique = true)]
        [StringLength(50)]
        [Required]
        public string Name { get; set; }
        [StringLength(512)]
        public string Description { get; set; }

    }
}
