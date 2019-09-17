using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SSO.Data.Models
{
    public class UserAuthority : BaseModel
    {
        [StringLength(50)]
        public string UserId { get; set; }
        [StringLength(50)]
        public string UserName { get; set; }
        [StringLength(20)]
        public string LoginId { get; set; }
        [StringLength(30)]
        public string Password { get; set; }
        [StringLength(30)]
        public string Company { get; set; }
        public List<string> Department { get; set; }
        public List<string> Role { get; set; }
        public bool IsModified { get; set; }


    }
}
