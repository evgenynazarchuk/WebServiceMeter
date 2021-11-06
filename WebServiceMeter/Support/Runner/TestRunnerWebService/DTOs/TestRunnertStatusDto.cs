﻿using System;

namespace WebServiceMeter.Runner
{
    public class TestRunnertStatusDto : TestMethodSimpleDto
    {
        public long TestRunIdentifier { get; set; }

        public object[]? ParametersValues { get; set; } = null;

        public DateTime StartTime { get; set; }
    }
}
