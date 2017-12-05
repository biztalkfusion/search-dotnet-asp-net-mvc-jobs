using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NYCJobsWeb.Models
{
    public class User
    {
        public long Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string UserName { get; set; }

        [Required]
        [MaxLength(250)]
        public string Password { get; set; }

        [Required]
        public string FolderName { get; set; }

        [Required]
        public string Role { get; set; }
        public long RoleId { get; set; }
        public string RoleName { get; set; }
        public IList<SelectListItem> Folders { get; set; }
        public IList<SelectListItem> Roles { get; set; }
    }
}