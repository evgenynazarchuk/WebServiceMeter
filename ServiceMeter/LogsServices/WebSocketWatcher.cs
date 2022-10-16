using ServiceMeter.Interfaces;

namespace ServiceMeter.LogsServices;

public class WebSocketWatcher : Watcher, IGrpcWatcher
{
    public WebSocketWatcher(params IWebSocketReport[] reports)
        : base(reports)
    {
    }
}