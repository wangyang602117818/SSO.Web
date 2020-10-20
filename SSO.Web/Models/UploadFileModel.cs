using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace SSO.Web.Models
{
    public class UploadFileModel
    {
        private string roles = "";
        private string users = "";
        private string usersDisplay = "";
        [Required]
        public List<HttpPostedFileBase> Files { get; set; }
        public string Roles { get => roles; set => roles = value; }
        public string Users { get => users; set => users = value; }
        public string UsersDisplay { get => usersDisplay; set => usersDisplay = value; }
    }
}