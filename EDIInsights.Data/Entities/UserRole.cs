using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizTalkFusion.Solutions.Integration.Data.Entities
{
    public class UserRole : Auditable
    {
        [Required]
        public long UserId { get; set; }

        [Required]
        public long RoleId { get; set; }

        public virtual User User { get; set; }
        public virtual Role Role { get; set; }

    }
}
