using SSO.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSO.Data.Models
{
    public class Log : BaseModel
    {
        [StringLength(30)]
        public string From { get; set; }
        [Required]
        public LogType Type { get; set; }
        [StringLength(50)]
        public string RecordId { get; set; }
        [StringLength(50)]
        [Required]
        public string Content { get; set; }
        [StringLength(512)]
        public string Remark { get; set; }
        [StringLength(30)]
        public string UserId { get; set; }
        [StringLength(30)]
        public string UserName { get; set; }
        public string UserHost { get; set; }
        public string UserAgent { get; set; }

    }
}
