using ServiceMeter.Interfaces;

namespace ServiceMeter.LogsServices;

public class GrpcWatcher : Watcher, IGrpcWatcher
{
    public GrpcWatcher(params IGrpcReport[] reports)
        : base(reports)
    {
    }
}