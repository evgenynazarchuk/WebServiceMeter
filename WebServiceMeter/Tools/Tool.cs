using WebServiceMeter.Reports;

namespace WebServiceMeter.Tools
{
    public class Tool
    {
        public readonly Watcher? Watcher = null;

        public Tool(Watcher? watcher = null)
        {
            this.Watcher = watcher;
        }
    }
}
