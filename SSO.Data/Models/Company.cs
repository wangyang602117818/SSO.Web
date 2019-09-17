using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SSO.Data.Models
{
    public class Company : BaseModel
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

    }
}
