using ServiceMeter.Runner.Runner;
using System.Reflection;

namespace PerformanceJsonPlaceholder;

class Program
{
    static async Task Main(string[] args)
    {
        var runner = new TestRunner(args, Assembly.GetExecutingAssembly());
        await runner.StartAsync();
    }
}