using System;
using ServiceMeter;
using ServiceMeter.Support;
using ServiceMeter.HttpService;
using ServiceMeter.HttpTools;
using System.Net;
using ServiceMeter.LogsServices;
using ServiceMeter.HttpService.Users;

namespace PerformanceJsonPlaceholder.TestUsers;

public class TestUser1 : HttpUser
{
    public TestUser1(string address, 
        Reports? reports = null, 
        IDictionary<string,string>? httpHeaders = null, 
        IEnumerable<Cookie>? httpCookies = null, 
        string userName = "")
        : base(address, reports, httpHeaders, httpCookies, userName)
    {
    }

    protected override async Task UserActionsAsync()
    {
        await Get("/posts", requestLabel: "GET_POSTS");
    }
}