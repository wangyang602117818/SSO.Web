using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Hosting;
using System.Web.Mvc;

namespace SSO.Web.Models
{
    public class LoginModel
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public string PassWord { get; set; }
    }
    public class LoginReturnTokenModel : LoginModel
    {
        [Required]
        public string From { get; set; }
    }

    public class AddUserModel
    {
        private List<string> departments = new List<string>();
        private List<string> roles = new List<string>();
        [Required]
        public string UserId { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string CompanyCode { get; set; }
        [Required]
        public char Sex { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Mobile { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Email { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string IdCard { get; set; }
        public List<string> Departments { get => departments; set => departments = value; }
        public List<string> Roles { get => roles; set => roles = value; }

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