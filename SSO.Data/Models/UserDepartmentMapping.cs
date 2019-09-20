using System.ComponentModel.DataAnnotations;

namespace SSO.Data.Models
{
    public class UserDepartmentMapping : BaseModel
    {
        [StringLength(50)]
        public string UserId { get; set; }
        [StringLength(30)]
        public string DepartmentCode { get; set; }
    }
}
