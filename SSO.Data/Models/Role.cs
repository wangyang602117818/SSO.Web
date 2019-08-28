﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SSO.Data.Models
{
    public class Role
    {
        [Key]
        public int Id { get; set; }
        [Index(IsUnique = true)]
        [StringLength(50)]
        public string Name { get; set; }
        [StringLength(512)]
        public string Description { get; set; }
        public DateTime? UpdateTime { get; set; }
        public DateTime? CreateTime { get; set; }


    }
}
