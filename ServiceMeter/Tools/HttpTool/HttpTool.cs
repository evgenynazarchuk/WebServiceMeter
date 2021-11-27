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

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ServiceMeter.Reports;
using ServiceMeter.Support;
using ServiceMeter.Tools;

namespace ServiceMeter;

public sealed partial class HttpTool : Tool
{
    public HttpTool(
        string baseAddress,
        Watcher? watcher = null,
        IDictionary<string, string>? defaultHeaders = null,
        IEnumerable<Cookie>? defaultCookies = null)
        : base(watcher)
    {
        var handler = new HttpClientHandler()
        {
            MaxConnectionsPerServer = int.MaxValue
        };

        handler.SetDefaultCookie(defaultCookies);

        this.HttpClient = new HttpClient(handler);
        this.HttpClient.SetDefaultHeader(defaultHeaders);

        //this.TurnOffConnectionLimit(baseAddress);
        this.SetBaseSettings(baseAddress);
    }

    public HttpTool(HttpClient client, Watcher? watcher = null)
        : base(watcher)
    {
        this.HttpClient = client;
    }

    private void SetBaseSettings(string baseAddress)
    {
        this.HttpClient.BaseAddress = new(baseAddress);
        this.HttpClient.DefaultRequestVersion = new(2, 0);
        this.HttpClient.Timeout = Timeout.InfiniteTimeSpan;
        this.HttpClient.DefaultVersionPolicy = HttpVersionPolicy.RequestVersionOrLower;
    }

    //private void TurnOffConnectionLimit(string baseAddress)
    //{
    //    var delayServicePoint = ServicePointManager.FindServicePoint(new Uri(baseAddress));
    //    delayServicePoint.ConnectionLeaseTimeout = 0;
    //}

    public async Task<HttpResponse> RequestAsync(
        HttpRequestMessage httpRequestMessage,
        string userName = "",
        string requestLabel = "")
    {
        long startSendRequest;
        long startWaitResponse;
        long startReceiveResponse;
        long endRequest;
        long requestSize = 0;

        Task<HttpResponseMessage>? httpResponseMessageTask;
        HttpResponseMessage httpResponseMessage;
        byte[] content;

        startSendRequest = ScenarioTimer.Time.Elapsed.Ticks;
        httpResponseMessageTask = HttpClient.SendAsync(httpRequestMessage, HttpCompletionOption.ResponseHeadersRead);

        startWaitResponse = ScenarioTimer.Time.Elapsed.Ticks;
        httpResponseMessage = await httpResponseMessageTask;

        startReceiveResponse = ScenarioTimer.Time.Elapsed.Ticks;
        content = await httpResponseMessage.Content.ReadAsByteArrayAsync();
        endRequest = ScenarioTimer.Time.Elapsed.Ticks;

        int responseSize = content.Length;

        if (httpRequestMessage.Content is not null && httpRequestMessage.Content.Headers.ContentLength is not null)
        {
            requestSize = httpRequestMessage.Content.Headers.ContentLength.Value;
        }

        if (this.Watcher is not null)
        {
            this.Watcher.SendMessage(
                logName: "HttpClientToolLog.json",
                logMessage: $"{userName},{httpRequestMessage.Method.Method},{httpRequestMessage.RequestUri},{requestLabel},{(int)httpResponseMessage.StatusCode},{startSendRequest},{startWaitResponse},{startReceiveResponse},{endRequest},{requestSize},{responseSize}",
                logMessageType: typeof(HttpLogMessage)
                );
        }

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
        string userName = "",
        string requestLabel = "")
    {
        // TODO
        //using HttpRequestMessage httpRequestMessage = new()
        var httpRequestMessage = new HttpRequestMessage()
        {
            Method = httpMethod,
            RequestUri = new Uri(path, UriKind.Relative),
            Content = requestContent,
        };

        if (requestHeaders is not null)
        {
            foreach ((var name, var value) in requestHeaders)
            {
                httpRequestMessage.Headers.Add(name, value);
            }
        }

        return this.RequestAsync(httpRequestMessage, userName, requestLabel);
    }

    public readonly HttpClient HttpClient;
}
