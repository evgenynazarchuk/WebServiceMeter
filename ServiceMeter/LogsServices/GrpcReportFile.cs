using ServiceMeter.Interfaces;

namespace ServiceMeter.LogsServices;

public class GrpcReportFile : ReportFile, IGrpcReportFile
{
    public GrpcReportFile(string logName)
        : base(logName)
    {
    }
}