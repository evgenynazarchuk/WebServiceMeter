using ServiceMeter.Interfaces;

namespace ServiceMeter.LogsServices;

public class HttpReportFile : ReportFile, IHttpReportFile
{
    public HttpReportFile(string logName)
        : base(logName)
    {
    }
}