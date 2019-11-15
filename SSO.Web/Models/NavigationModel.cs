using System.ComponentModel.DataAnnotations;

namespace SSO.Web.Models
{
    public class NavigationModel
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string BaseUrl { get; set; }
    }
    public class UpdateNavigationModel : NavigationModel
    {
        [Required]
        public int Id { get; set; }
    }
}