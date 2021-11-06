﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebServiceMeter.Reports
{
    public class GrpcLogReceiveMessage
    {
        public string? UserName { get; set; }

        public string? Action { get; set; }

        public string? Label { get; set; }

        public long StartTime { get; set; }

        public long EndTime { get; set; }
    }
}