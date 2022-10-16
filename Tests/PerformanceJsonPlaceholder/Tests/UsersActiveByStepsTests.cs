using ServiceMeter.Support;
using ServiceMeter.LogsServices;
using ServiceMeter.PerformancePlans;
using ServiceMeter.HttpTools.GenerateReports.Services;
using ServiceMeter.Runner.Attributes;
using PerformanceJsonPlaceholder.TestUsers;
using ServiceMeter.Extensions;

namespace PerformanceJsonPlaceholder.Tests;

[PerformanceTestClass]
public class UsersActiveByStepsTests
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
        
        //
        var plan = new UsersActiveBySteps(
            user, 
            fromActiveUsersCount: 5,
            toActiveUsersCount: 10,
            usersStep: 5,
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