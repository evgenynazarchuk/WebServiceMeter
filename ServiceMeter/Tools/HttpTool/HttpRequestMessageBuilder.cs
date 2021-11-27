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
using System.Net.Http;

namespace ServiceMeter;

public class HttpRequestMessageBuilder
{
    public string RequestUri { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public HttpMethod HttpMethod { get; set; } = HttpMethod.Get;
    public Version HttpVersion { get; set; } = new(2, 0);

    private HttpVersionPolicy _policy = HttpVersionPolicy.RequestVersionOrLower;
    private Dictionary<string, IEnumerable<string>>? _headers;

    public HttpRequestMessageBuilder UseRequest(string requestUri)
    {
        RequestUri = requestUri;
        return this;
    }

    public HttpRequestMessageBuilder UseContent(string content)
    {
        Content = content;
        return this;
    }

    public HttpRequestMessageBuilder UseHttpMethod(string method)
    {
        HttpMethod = new HttpMethod(method);
        return this;
    }

    public HttpRequestMessageBuilder UseVersion(string version)
    {
        HttpVersion = new Version(version);
        return this;
    }

    public HttpRequestMessageBuilder UseVersionPolicy(HttpVersionPolicy policy)
    {
        _policy = policy;
        return this;
    }

    public HttpRequestMessageBuilder UseRequestHeaders(Dictionary<string, string> headers)
    {
        foreach ((var key, var value) in headers)
        {
            _headers?.Add(key, new List<string> { value });
        }

        return this;
    }

    public HttpRequestMessageBuilder UseRequestHeaders(Dictionary<string, IEnumerable<string>> headers)
    {
        _headers = headers;
        return this;
    }

    public HttpRequestMessage Build()
    {
        var httpResponseMessage = new HttpRequestMessage()
        {
            RequestUri = new Uri(this.RequestUri, UriKind.Relative),
            Content = new StringContent(Content),
            Method = HttpMethod,
            Version = HttpVersion,
            VersionPolicy = _policy
        };

        if (_headers is not null)
        {
            foreach ((var key, var value) in _headers)
            {
                httpResponseMessage.Headers.Add(key, value);
            }
        }

        return httpResponseMessage;
    }
}
