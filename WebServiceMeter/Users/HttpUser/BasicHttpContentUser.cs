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

using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text;

namespace WebServiceMeter.Users;

public abstract partial class BasicHttpUser : BasicUser
{
    public Task<HttpResponse> Request(
        HttpMethod httpMethod,
        string path,
        Dictionary<string, string>? requestHeaders = null,
        HttpContent? requestContent = null,
        string requestLabel = "")
    {
        return this.Tool.RequestAsync(
            httpMethod,
            path,
            requestHeaders,
            requestContent,
            this.UserName,
            requestLabel);
    }

    public Task<HttpResponse> Get(
        string path,
        Dictionary<string, string>? requestHeaders = null,
        string? requestContent = null,
        Encoding? requestContentEncoding = null,
        string requestLabel = "")
    {
        return this.Tool.GetAsync(
            path: path,
            requestContent: requestContent,
            requestContentEncoding: requestContentEncoding,
            requestHeaders: requestHeaders,
            userName: this.UserName,
            requestLabel: requestLabel);
    }

    public Task<HttpResponse> Post(
        string path,
        Dictionary<string, string>? requestHeaders = null,
        string? requestContent = null,
        Encoding? requestContentEncoding = null,
        string requestLabel = "")
    {
        return this.Tool.PostAsync(
            path: path,
            requestContent: requestContent,
            requestContentEncoding: requestContentEncoding,
            requestHeaders: requestHeaders,
            userName: this.UserName,
            requestLabel: requestLabel);
    }

    public Task<HttpResponse> Put(
        string path,
        Dictionary<string, string>? requestHeaders = null,
        string? requestContent = null,
        Encoding? requestContentEncoding = null,
        string requestLabel = "")
    {
        return this.Tool.PutAsync(
            path: path,
            requestHeaders: requestHeaders,
            requestContent: requestContent,
            requestContentEncoding: requestContentEncoding,
            userName: this.UserName,
            requestLabel: requestLabel);
    }

    public Task<HttpResponse> Delete(
        string path,
        Dictionary<string, string>? requestHeaders = null,
        string? requestContent = null,
        Encoding? requestContentEncoding = null,
        string requestLabel = "")
    {
        return this.Tool.DeleteAsync(
            path: path,
            requestHeaders: requestHeaders,
            requestContent: requestContent,
            requestContentEncoding: requestContentEncoding,
            userName: this.UserName,
            requestLabel: requestLabel);
    }
}
