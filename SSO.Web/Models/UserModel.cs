using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SSO.Web.Models
{
    public class LoginModel
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public string PassWord { get; set; }
    }
    public class AddUserModel
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string CompanyCode { get; set; }
        public string IdCard { get; set; }
        public char Sex { get; set; }
        public List<string> Departments { get; set; }
        public List<string> Roles { get; set; }
    }
    public class UpdateUserModel : AddUserModel
    {
        [Required]
        public int Id { get; set; }
    }
    public class UpdatePasswordModel
    {
        [Required]
        public string oldPassword { get; set; }
        [Required]
        public string newPassword { get; set; }
    }
}