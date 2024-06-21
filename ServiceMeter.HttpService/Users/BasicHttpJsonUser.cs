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

using ServiceMeter.Support;

namespace ServiceMeter.HttpService.Users;

public abstract partial class BasicHttpUser : User
{
    //
    protected Task<TResponse?> RequestAsJson<TResponse, TRequest>(
        HttpMethod httpMethod,
        string path,
        TRequest requestObject,
        Dictionary<string, string>? requestHeaders = null,
        string requestLabel = "")
        where TRequest : class, new()
        where TResponse : class, new()
    {
        return this._httpTool.RequestAsJsonAsync<TResponse, TRequest>(
            httpMethod: httpMethod,
            path: path,
            requestObject: requestObject,
            requestHeaders: requestHeaders,
            requestLabel: requestLabel);
    }

    protected Task<int> RequestAsJson<TRequest>(
        HttpMethod httpMethod,
        string path,
        TRequest requestObject,
        Dictionary<string, string>? requestHeaders = null,
        string requestLabel = "")
        where TRequest : class, new()
    {
        var httpResult = this._httpTool.RequestAsJsonAsync<TRequest>(
            httpMethod: httpMethod,
            path: path,
            requestObject: requestObject,
            requestHeaders: requestHeaders,
            requestLabel: requestLabel);

        return httpResult;
    }

    protected Task<TResponse?> RequestAsJson<TResponse>(
        HttpMethod httpMethod,
        string path,
        Dictionary<string, string>? requestHeaders = null,
        string requestLabel = "")
        where TResponse : class, new()
    {
        return this._httpTool.RequestAsJsonAsync<TResponse>(
            httpMethod: httpMethod,
            path: path,
            requestHeaders: requestHeaders,
            requestLabel: requestLabel);
    }

    //
    protected Task<TResponse?> GetAsJson<TResponse>(
        string path,
        Dictionary<string, string>? requestHeaders = null,
        string requestLabel = "")
        where TResponse : class, new()
    {
        return this._httpTool.GetAsJsonAsync<TResponse>(
            path: path,
            requestHeaders: requestHeaders,
            requestLabel: requestLabel);
    }

    protected Task<TResponse?> GetAsJson<TResponse, TRequest>(
        string path,
        TRequest requestObject,
        Dictionary<string, string>? requestHeaders = null,
        string requestLabel = "")
        where TResponse : class, new()
        where TRequest : class, new()
    {
        return this._httpTool.GetAsJsonAsync<TResponse, TRequest>(
            path: path,
            requestObject: requestObject,
            requestHeaders: requestHeaders,
            requestLabel: requestLabel);
    }

    protected Task<int> GetAsJson<TRequest>(
        string path,
        TRequest requestObject,
        Dictionary<string, string>? requestHeaders = null,
        string requestLabel = "")
        where TRequest : class, new()
    {
        return this._httpTool.GetAsJsonAsync<TRequest>(
            path: path,
            requestObject: requestObject,
            requestHeaders: requestHeaders,
            requestLabel: requestLabel);
    }

    //
    protected Task<int> PostAsJson<TRequest>(
        string path,
        TRequest requestObject,
        Dictionary<string, string>? requestHeaders = null,
        string requestLabel = "")
        where TRequest : class, new()
    {
        return this._httpTool.PostAsJsonAsync<TRequest>(
            path: path,
            requestObject: requestObject,
            requestHeaders: requestHeaders,
            requestLabel: requestLabel);
    }

    protected Task<TResponse?> PostAsJson<TResponse, TRequest>(
        string path,
        TRequest requestObject,
        Dictionary<string, string>? requestHeaders = null,
        string requestLabel = "")
        where TResponse : class, new()
        where TRequest : class, new()
    {
        return this._httpTool.PostAsJsonAsync<TResponse, TRequest>(
            path: path,
            requestObject: requestObject,
            requestHeaders: requestHeaders,
            requestLabel: requestLabel);
    }

    protected Task<TResponse?> PostAsJson<TResponse>(
        string path,
        Dictionary<string, string>? requestHeaders = null,
        string requestLabel = "")
        where TResponse : class, new()
    {
        return this._httpTool.PostAsJsonAsync<TResponse>(
            path: path,
            requestHeaders: requestHeaders,
            requestLabel: requestLabel);
    }

    //
    protected Task<int> PutAsJson<TRequest>(
        string path,
        TRequest requestObject,
        Dictionary<string, string>? requestHeaders = null,
        string requestLabel = "")
        where TRequest : class, new()
    {
        return this._httpTool.PutAsJsonAsync<TRequest>(
            path: path,
            requestObject: requestObject,
            requestHeaders: requestHeaders,
            requestLabel: requestLabel);
    }

    protected Task<TResponse?> PutAsJson<TResponse, TRequest>(
        string path,
        TRequest requestObject,
        Dictionary<string, string>? requestHeaders = null,
        string requestLabel = "")
        where TResponse : class, new()
        where TRequest : class, new()
    {
        return this._httpTool.PutAsJsonAsync<TResponse, TRequest>(
            path: path,
            requestObject: requestObject,
            requestHeaders: requestHeaders,
            requestLabel: requestLabel);
    }

    protected Task<TResponse?> PutAsJson<TResponse>(
        string path,
        Dictionary<string, string>? requestHeaders = null,
        string requestLabel = "")
        where TResponse : class, new()
    {
        return this._httpTool.PutAsJsonAsync<TResponse>(
            path: path,
            requestHeaders: requestHeaders,
            requestLabel: requestLabel);
    }

    //
    protected Task<int> DeleteAsJson<TRequest>(
        string path,
        TRequest requestObject,
        Dictionary<string, string>? requestHeaders = null,
        string requestLabel = "")
        where TRequest : class, new()
    {
        return this._httpTool.DeleteAsJsonAsync<TRequest>(
            path: path,
            requestObject: requestObject,
            requestHeaders: requestHeaders,
            requestLabel: requestLabel);
    }

    protected Task<TResponse?> DeleteAsJson<TResponse, TRequest>(
        string path,
        TRequest requestObject,
        Dictionary<string, string>? requestHeaders = null,
        string requestLabel = "")
        where TResponse : class, new()
        where TRequest : class, new()
    {
        return this._httpTool.DeleteAsJsonAsync<TResponse, TRequest>(
            path: path,
            requestObject: requestObject,
            requestHeaders: requestHeaders,
            requestLabel: requestLabel);
    }

    protected Task<TResponse?> DeleteAsJson<TResponse>(
        string path,
        Dictionary<string, string>? requestHeaders = null,
        string requestLabel = "")
        where TResponse : class, new()
    {
        return this._httpTool.DeleteAsJsonAsync<TResponse>(
            path: path,
            requestHeaders: requestHeaders,
            requestLabel: requestLabel);
    }
}
