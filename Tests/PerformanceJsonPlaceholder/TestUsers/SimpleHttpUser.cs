using System.Net;
using ServiceMeter.HttpService.Users;
using ServiceMeter.Interfaces;

namespace PerformanceJsonPlaceholder.TestUsers;

public class SimpleHttpUser : HttpUser
{
    public SimpleHttpUser(string address, 
        IHttpWatcher? watcher = null, 
        IDictionary<string,string>? httpHeaders = null, 
        IEnumerable<Cookie>? httpCookies = null, 
        string userName = "")
        : base(address, watcher, httpHeaders, httpCookies, userName)
    {
    }

    protected override async Task UserActionsAsync()
    {
        await Get("/posts", requestLabel: "GET_POSTS");
    }
}