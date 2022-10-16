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
        const string url = "https://jsonplaceholder.typicode.com";
        const string json = "HttpServiceLog.txt";
        const string html = "HttpServiceReport.html";
        
        var reportFile = new HttpReportFile(json);
        var reportConsole = new ReportConsole("LOG");
        var watcher = new HttpWatcher(reportFile, reportConsole);

        var user = new SimpleHttpUser(
            address: url, 
            watcher: watcher, 
            userName: "Test User");
        
        // Удерживать 5 активных пользоваталей на протяжении 10 секунд
        var plan = new UsersActiveOnPeriod(
            user, 
            activeUsersCount: 5, 
            performancePlanDuration: 10.Seconds());
        
        new Scenario()
            .AddWatchers(watcher)
            .AddSequentialPlans(plan)
            .StartAsync()
            .Wait();
        
        new HttpRequestHtmlReport(
            sourceJsonPath: json, 
            resultHtmlPath: html)
            .BuildHtml();
    }
}