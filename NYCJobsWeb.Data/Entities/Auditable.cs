﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NYCJobsWeb.Data.Entities
{
    public abstract class Auditable
    {
        public long Id { get; set; }
    }
}
