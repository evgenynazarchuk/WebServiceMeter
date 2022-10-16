using ServiceMeter.Support;
using ServiceMeter.LogsServices;
using ServiceMeter.PerformancePlans;
using ServiceMeter.HttpTools.GenerateReports.Services;
using ServiceMeter.Runner.Attributes;
using PerformanceJsonPlaceholder.TestUsers;

namespace PerformanceJsonPlaceholder.Tests;

[PerformanceTestClass]
public class UsersConstantTests
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
        
        // Разово запустить указанное кол-во пользователей
        var plan = new UsersConstant(user, usersCount: 10);
        
        new Scenario().AddSequentialPlans(plan).StartAsync().Wait();
        
        reports.StopProcess();
        
        new HttpRequestReportBuilder(jsonLog, htmlReport).BuildHtml();
    }
}