/*
 * MIT License
 *
 * Copyright (c) Evgeny Nazarchuk.
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

using System.Net;
using ServiceMeter.Support;
using ServiceMeter.Extensions;
using ServiceMeter.Interfaces;
using ServiceMeter.HttpService.Models;
using System.Text.Json;

namespace ServiceMeter.HttpService.Tools;

public partial class HttpTool : Tool
{
    private readonly HttpClient _httpClient;

    public HttpTool(
        string baseAddress,
        IHttpWatcher? watcher = null,
        IDictionary<string, string>? httpHeaders = null,
        IEnumerable<Cookie>? httpCookies = null,
        string userName = "")
        : base(watcher, userName)
    {
        var handler = new HttpClientHandler()
        {
            MaxConnectionsPerServer = int.MaxValue
        };

        if (httpCookies is not null)
        {
            handler.SetDefaultCookie(httpCookies);
        }

        this._httpClient = new HttpClient(handler);

        if (httpHeaders is not null)
        {
            this._httpClient.SetDefaultHeader(httpHeaders);
        }

        this.SetBaseSettings(baseAddress);
    }

    private void SetBaseSettings(string baseAddress)
    {
        this._httpClient.BaseAddress = new Uri(baseAddress);
        this._httpClient.DefaultRequestVersion = new Version(2, 0);
        this._httpClient.Timeout = Timeout.InfiniteTimeSpan;
        this._httpClient.DefaultVersionPolicy = HttpVersionPolicy.RequestVersionOrLower;
    }

    private async Task<HttpResponse> RequestAsync(HttpRequestMessage httpRequestMessage, string requestLabel = "")
    {
        var startSendRequest = ScenarioTimer.Time.Elapsed.Ticks;

        var httpResponseMessageTask = this._httpClient.SendAsync(httpRequestMessage, HttpCompletionOption.ResponseHeadersRead);

        var startWaitResponse = ScenarioTimer.Time.Elapsed.Ticks;

        var httpResponseMessage = await httpResponseMessageTask;

        var startReceiveResponse = ScenarioTimer.Time.Elapsed.Ticks;

        var content = await httpResponseMessage.Content.ReadAsByteArrayAsync();
        
        var endRequest = ScenarioTimer.Time.Elapsed.Ticks;

        var responseContentSize = content.Length;
        
        var requestContentSize = httpRequestMessage.Content?.Headers?.ContentLength.HasValue is true
            ? httpRequestMessage.Content.Headers.ContentLength.Value
            : 0;

        var httpLogMessage = new HttpLogMessage()
        {
            UserName = this.UserName,
            RequestLabel = requestLabel,
            RequestMethod = httpRequestMessage.Method.Method,
            RequestUri = $"{httpRequestMessage.RequestUri}",
            StatusCode = (int)httpResponseMessage.StatusCode,
            SendBytes = requestContentSize,
            ReceiveBytes = responseContentSize,
            StartSendRequestTime = startSendRequest,
            StartWaitResponseTime = startWaitResponse,
            StartResponseTime = startReceiveResponse,
            EndResponseTime = endRequest,
        };
        
        var logJson = JsonSerializer.Serialize(httpLogMessage);
        
        this.Watcher?.SendMessage(logMessage: logJson);

        var response = new HttpResponse(
            statusCode: (int)httpResponseMessage.StatusCode,
            content: content,
            filename: httpResponseMessage.Content.Headers.ContentDisposition?.FileName
        );

        return response;
    }

    public Task<HttpResponse> RequestAsync(
        HttpMethod httpMethod,
        string path,
        Dictionary<string, string>? requestHeaders = null,
        HttpContent? requestContent = null,
        string requestLabel = "")
    {
        var httpRequestMessage = new HttpRequestMessage()
        {
            Method = httpMethod,
            RequestUri = new Uri(path, UriKind.Relative),
            Content = requestContent,
        };

        if (requestHeaders is not null)
        {
            foreach (var (name, value) in requestHeaders)
            {
                httpRequestMessage.Headers.Add(name, value);
            }
        }

        return this.RequestAsync(httpRequestMessage, requestLabel);
    }
}
