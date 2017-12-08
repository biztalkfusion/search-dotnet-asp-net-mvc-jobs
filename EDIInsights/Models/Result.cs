﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BizTalkFusion.Solutions.Integration.Models
{
    public class Result
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public dynamic Data { get; set; }
    }
}