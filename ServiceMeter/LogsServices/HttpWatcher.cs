using ServiceMeter.Interfaces;

namespace ServiceMeter.LogsServices;

public class HttpWatcher : Watcher, IHttpWatcher
{
    public HttpWatcher(params IHttpReport[] reports)
        : base(reports)
    {
    }
}