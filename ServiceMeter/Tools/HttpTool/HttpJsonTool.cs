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
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ServiceMeter.Tools.HttpTool;

namespace ServiceMeter;

public partial class HttpTool
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true,
    };

    //
    public async Task<TResponse?> RequestAsJsonAsync<TResponse, TRequest>(
        HttpMethod httpMethod,
        string path,
        TRequest requestObject,
        Dictionary<string, string>? requestHeaders = null,
        string userName = "",
        string requestLabel = "")
        where TResponse : class, new()
        where TRequest : class, new()
    {
        string requestContent = JsonSerializer.Serialize(requestObject, JsonSerializerOptions);

        var response = await this.RequestAsync(
            httpMethod: httpMethod,
            path: path,
            requestContent: new JsonContent(requestContent, Encoding.UTF8),
            requestHeaders: requestHeaders,
            userName: userName,
            requestLabel: requestLabel);

        var responseObject = JsonSerializer.Deserialize<TResponse>(response.Content, JsonSerializerOptions);

        return responseObject;
    }

    public async Task<int> RequestAsJsonAsync<TRequest>(
        HttpMethod httpMethod,
        string path,
        TRequest requestObject,
        Dictionary<string, string>? requestHeaders = null,
        string userName = "",
        string requestLabel = "")
        where TRequest : class, new()
    {
        string requestContentString = JsonSerializer.Serialize(requestObject, JsonSerializerOptions);

        HttpResponse response = await this.RequestAsync(
            httpMethod: httpMethod,
            path: path,
            requestContent: new JsonContent(requestContentString, Encoding.UTF8),
            requestHeaders: requestHeaders,
            userName: userName,
            requestLabel: requestLabel);

        return response.StatusCode;
    }

    public async Task<TResponse?> RequestAsJsonAsync<TResponse>(
        HttpMethod httpMethod,
        string path,
        Dictionary<string, string>? requestHeaders = null,
        string user = "",
        string requestLabel = "")
        where TResponse : class, new()
    {
        HttpResponse response = await this.RequestAsync(
            httpMethod: httpMethod,
            path: path,
            requestHeaders: requestHeaders,
            userName: user,
            requestLabel: requestLabel);

        TResponse? responseObject = JsonSerializer.Deserialize<TResponse>(response.ContentAsUTF8, JsonSerializerOptions);

        return responseObject;
    }

    //
    public Task<int> GetAsJsonAsync<TRequest>(
        string path,
        TRequest requestObject,
        string userName = "",
        string requestLabel = "",
        Dictionary<string, string>? requestHeaders = null)
        where TRequest : class, new()
    {
        return this.RequestAsJsonAsync<TRequest>(
            HttpMethod.Get,
            path,
            requestObject,
            requestHeaders,
            userName,
            requestLabel);
    }

    public Task<TResponse?> GetAsJsonAsync<TResponse>(
        string path,
        string userName = "",
        string requestLabel = "",
        Dictionary<string, string>? requestHeaders = null)
        where TResponse : class, new()
    {
        return this.RequestAsJsonAsync<TResponse>(
            HttpMethod.Get,
            path,
            requestHeaders,
            userName,
            requestLabel);
    }

    public Task<TResponse?> GetAsJsonAsync<TResponse, TRequest>(
        string path,
        TRequest requestObject,
        string userName = "",
        string requestLabel = "",
        Dictionary<string, string>? requestHeaders = null)
        where TResponse : class, new()
        where TRequest : class, new()
    {
        return this.RequestAsJsonAsync<TResponse, TRequest>(
            HttpMethod.Get,
            path,
            requestObject,
            requestHeaders,
            userName,
            requestLabel);
    }

    //
    public Task<int> PostAsJsonAsync<TRequest>(
        string path,
        TRequest requestObject,
        string userName = "",
        string requestLabel = "",
        Dictionary<string, string>? requestHeaders = null)
        where TRequest : class, new()
    {
        return this.RequestAsJsonAsync<TRequest>(
            HttpMethod.Post,
            path,
            requestObject,
            requestHeaders,
            userName,
            requestLabel);
    }

    public Task<TResponse?> PostAsJsonAsync<TResponse, TRequest>(
        string path,
        TRequest requestObject,
        string userName = "",
        string requestLabel = "",
        Dictionary<string, string>? requestHeaders = null)
        where TResponse : class, new()
        where TRequest : class, new()
    {
        return this.RequestAsJsonAsync<TResponse, TRequest>(
            HttpMethod.Post,
            path,
            requestObject,
            requestHeaders,
            userName,
            requestLabel);
    }

    public Task<TResponse?> PostAsJsonAsync<TResponse>(
        string path,
        string userName = "",
        string requestLabel = "",
        Dictionary<string, string>? requestHeaders = null)
        where TResponse : class, new()
    {
        return this.RequestAsJsonAsync<TResponse>(
            HttpMethod.Post,
            path,
            requestHeaders,
            userName,
            requestLabel);
    }

    //
    public Task<int> PutAsJsonAsync<TRequest>(
        string path,
        TRequest requestObject,
        string userName = "",
        string requestLabel = "",
        Dictionary<string, string>? requestHeaders = null)
        where TRequest : class, new()
    {
        return this.RequestAsJsonAsync<TRequest>(
            HttpMethod.Put,
            path,
            requestObject,
            requestHeaders,
            userName,
            requestLabel);
    }

    public Task<TResponse?> PutAsJsonAsync<TResponse, TRequest>(
        string path,
        TRequest requestObject,
        string userName = "",
        string requestLabel = "",
        Dictionary<string, string>? requestHeaders = null)
        where TResponse : class, new()
        where TRequest : class, new()
    {
        return this.RequestAsJsonAsync<TResponse, TRequest>(
            HttpMethod.Put,
            path,
            requestObject,
            requestHeaders,
            userName,
            requestLabel);
    }

    public Task<TResponse?> PutAsJsonAsync<TResponse>(
        string path,
        string userName = "",
        string requestLabel = "",
        Dictionary<string, string>? requestHeaders = null)
        where TResponse : class, new()
    {
        return this.RequestAsJsonAsync<TResponse>(
            HttpMethod.Put,
            path,
            requestHeaders,
            userName,
            requestLabel);
    }

    //
    public Task<int> DeleteAsJsonAsync<TRequest>(
        string path,
        TRequest requestObject,
        string userName = "",
        string requestLabel = "",
        Dictionary<string, string>? requestHeaders = null)
        where TRequest : class, new()
    {
        return this.RequestAsJsonAsync<TRequest>(
            HttpMethod.Delete,
            path,
            requestObject,
            requestHeaders,
            userName,
            requestLabel);
    }

    public Task<TResponse?> DeleteAsJsonAsync<TResponse, TRequest>(
        string path,
        TRequest requestObject,
        string userName = "",
        string requestLabel = "",
        Dictionary<string, string>? requestHeaders = null)
        where TResponse : class, new()
        where TRequest : class, new()
    {
        return this.RequestAsJsonAsync<TResponse, TRequest>(
            HttpMethod.Delete,
            path,
            requestObject,
            requestHeaders,
            userName,
            requestLabel);
    }

    public Task<TResponse?> DeleteAsJsonAsync<TResponse>(
        string path,
        string userName = "",
        string requestLabel = "",
        Dictionary<string, string>? requestHeaders = null)
        where TResponse : class, new()
    {
        return this.RequestAsJsonAsync<TResponse>(
            HttpMethod.Delete,
            path,
            requestHeaders,
            userName,
            requestLabel);
    }
}
