﻿/*
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

using System.Text;
using ServiceMeter.HttpService.Models;

namespace ServiceMeter.HttpService.Tools;

public partial class HttpTool
{
    public Task<HttpResponse> GetAsync(
        string path,
        Dictionary<string, string>? requestHeaders = null,
        string? requestContent = null,
        Encoding? requestContentEncoding = null,
        string requestLabel = "")
    {
        return this.RequestAsync(
            httpMethod: HttpMethod.Get,
            path: path,
            requestContent: new StringContent(requestContent ?? "", requestContentEncoding ?? Encoding.UTF8),
            requestHeaders: requestHeaders,
            requestLabel: requestLabel);
    }

    public Task<HttpResponse> PostAsync(
        string path,
        Dictionary<string, string>? requestHeaders = null,
        string? requestContent = null,
        Encoding? requestContentEncoding = null,
        string requestLabel = "")
    {
        return this.RequestAsync(
            httpMethod: HttpMethod.Post,
            path: path,
            requestContent: new StringContent(requestContent ?? "", requestContentEncoding ?? Encoding.UTF8),
            requestHeaders: requestHeaders,
            requestLabel: requestLabel);
    }

    public Task<HttpResponse> PutAsync(
        string path,
        Dictionary<string, string>? requestHeaders = null,
        string? requestContent = null,
        Encoding? requestContentEncoding = null,
        string requestLabel = "")
    {
        return this.RequestAsync(
            httpMethod: HttpMethod.Put,
            path: path,
            requestHeaders: requestHeaders,
            requestContent: new StringContent(requestContent ?? "", requestContentEncoding ?? Encoding.UTF8),
            requestLabel: requestLabel);
    }

    public Task<HttpResponse> DeleteAsync(
        string path,
        Dictionary<string, string>? requestHeaders = null,
        string? requestContent = null,
        Encoding? requestContentEncoding = null,
        string requestLabel = "")
    {
        return this.RequestAsync(
            httpMethod: HttpMethod.Delete,
            path: path,
            requestHeaders: requestHeaders,
            requestContent: new StringContent(requestContent ?? "", requestContentEncoding ?? Encoding.UTF8),
            requestLabel: requestLabel);
    }
}
