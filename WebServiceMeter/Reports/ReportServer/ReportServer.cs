﻿using System;
using System.Threading.Tasks;

namespace WebServiceMeter.Reports
{
    public class ReportServer : Report
    {
        public ReportServer(string projectName, string testRunId)
            : base(projectName, testRunId) { }

        protected override Task ProcessAsync()
        {
            throw new NotImplementedException();
        }
    }
}
