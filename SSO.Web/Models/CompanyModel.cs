using System.ComponentModel.DataAnnotations;

namespace SSO.Web.Models
{
    public class CompanyModel
    {
        [Required]
        public string Code { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public int Order { get; set; }
    }
    public class UpdateCompanyModel : CompanyModel
    {
        [Required]
        public int Id { get; set; }
    }
}