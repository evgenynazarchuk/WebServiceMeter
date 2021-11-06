using System.Collections.Generic;

namespace WebServiceMeter.Runner
{
    public class TestMethodSimpleDto : TestMethodIdentityDto
    {
        public List<string?> ParametersNames { get; set; } = new();
    }
}
