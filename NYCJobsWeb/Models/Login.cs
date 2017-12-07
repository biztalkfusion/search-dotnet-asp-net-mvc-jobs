using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NYCJobsWeb.Models
{
    public class Login
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        public long RoleId { get; set; }
        public string RoleName { get; set; }
        public string ErrorMessage { get; set; }
        public bool IsLoginFailed { get; set; }
        public string WebToken { get; set; }
    }
}