﻿using System.ComponentModel.DataAnnotations;

namespace SSO.Web.Models
{
    public class DepartmentModel
    {
        [Required]
        public string Code { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string CompanyCode { get; set; }
        public string Description { get; set; }
        public int Order { get; set; }
        public int Layer { get; set; }
        public string ParentCode { get; set; }
    }
    public class UpdateDepartmentModel : DepartmentModel
    {
        [Required]
        public int Id { get; set; }
    }
}