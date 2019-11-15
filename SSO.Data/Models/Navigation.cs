using System.ComponentModel.DataAnnotations;

namespace SSO.Data.Models
{
    public class Navigation : BaseModel
    {
        [StringLength(30)]
        [Required]
        public string Title { get; set; }
        [StringLength(512)]
        public string BaseUrl { get; set; }
    }
}
