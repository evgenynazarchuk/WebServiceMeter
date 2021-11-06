using System.Collections.Generic;

namespace WebServiceMeter.Runner
{
    public class TestMethodDetailsDto : TestMethodSimpleDto
    {
        public List<object[]> ParametersValues { get; set; } = new();
    }
}
