using ServiceMeter.Support;
using ServiceMeter.LogsServices;
using ServiceMeter.PerformancePlans;
using ServiceMeter.HttpTools.GenerateReports.Services;
using ServiceMeter.Runner.Attributes;
using PerformanceJsonPlaceholder.TestUsers;
using ServiceMeter.Extensions;

namespace PerformanceJsonPlaceholder.Tests;

[PerformanceTestClass]
public class UsersActiveOnPeriodTests
{
    [PerformanceTest]
    public void Test1()
    {
        var url = "https://jsonplaceholder.typicode.com";
        var jsonLog = "HttpServiceLog.txt";
        var htmlReport = "HttpServiceReport.html";
        
        var fileReport = new ReportFile(jsonLog);
        var reports = new Reports(fileReport);

        reports.StartProcess();

        var user = new TestUser1(
            address: url, 
            reports: reports, 
            userName: "Test User");
        
        // Удерживать 5 активных пользоваталей на протяжении 10 секунд
        var plan = new UsersActiveOnPeriod(
            user, 
            activeUsersCount: 5, 
            performancePlanDuration: 10.Seconds());
        
        new Scenario().AddSequentialPlans(plan).StartAsync().Wait();
        
        reports.StopProcess();
        
        new HttpRequestReportBuilder(jsonLog, htmlReport).BuildHtml();
    }
}