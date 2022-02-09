using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace SSO.Web.Models
{
    public class UploadFileModel
    {
        private List<string> roles = new List<string>();
        private List<string> users = new List<string>();
        [Required]
        public List<HttpPostedFileBase> Files { get; set; }
        public List<string> Roles { get => roles; set => roles = value; }
        public List<string> Users { get => users; set => users = value; }
    }
}