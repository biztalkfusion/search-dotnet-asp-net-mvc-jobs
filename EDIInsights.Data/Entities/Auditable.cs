using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizTalkFusion.Solutions.Integration.Data.Entities
{
    public abstract class Auditable
    {
        public long Id { get; set; }
    }
}
