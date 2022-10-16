using ServiceMeter.Interfaces;

namespace ServiceMeter.LogsServices;

public class WebSocketReportFile : ReportFile, IWebSocketReportFile
{
    public WebSocketReportFile(string logName)
        : base(logName)
    {
    }
}