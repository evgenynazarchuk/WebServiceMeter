using ServiceMeter.HttpTools.GenerateReports.Services;

namespace ServiceMeter.HttpTools.GenerateReports;

public class Program
{
    public static void Main()
    {
        new HttpRequestHtmlReport("HttpServiceLogs.json", "HttpServiceReport.html").BuildHtml();
    }
}