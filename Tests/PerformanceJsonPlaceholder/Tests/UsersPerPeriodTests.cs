using ServiceMeter.Support;
using ServiceMeter.LogsServices;
using ServiceMeter.PerformancePlans;
using ServiceMeter.HttpTools.GenerateReports.Services;
using ServiceMeter.Runner.Attributes;
using PerformanceJsonPlaceholder.TestUsers;
using ServiceMeter.Extensions;

namespace PerformanceJsonPlaceholder.Tests;

[PerformanceTestClass]
public class UsersPerPeriodTests
{
    [PerformanceTest]
    public void Test1()
    {
        const string url = "https://jsonplaceholder.typicode.com";
        const string json = "HttpServiceLog.txt";
        const string html = "HttpServiceReport.html";
        
        var reportFile = new ReportFile(json);
        var reportConsole = new ReportConsole("LOG");
        var watcher = new Watcher(reportFile, reportConsole);

        var user = new TestUser1(
            address: url, 
            watcher: watcher, 
            userName: "Test User");
        
        // Каждую секунду запускать 10 пользователей на протяжении 10 секунд
        var plan = new UsersPerPeriod(
            user, 
            usersCountPerPeriod: 10, 
            performancePlanDuration: 10.Seconds(),
            perPeriod: 1.Seconds());
        
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