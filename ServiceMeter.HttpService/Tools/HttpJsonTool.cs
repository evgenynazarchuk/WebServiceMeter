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

using System.Text;
using System.Text.Json;
using ServiceMeter.HttpService.Models;

namespace ServiceMeter.HttpService.Tools;

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
        string requestLabel = "")
        where TResponse : class, new()
        where TRequest : class, new()
    {
        var requestContent = JsonSerializer.Serialize(requestObject, JsonSerializerOptions);

        var response = await this.RequestAsync(
            httpMethod: httpMethod,
            path: path,
            requestContent: new JsonContent(requestContent, Encoding.UTF8),
            requestHeaders: requestHeaders,
            requestLabel: requestLabel);

        var responseObject = JsonSerializer.Deserialize<TResponse>(response.Content, JsonSerializerOptions);

        return responseObject;
    }

    public async Task<int> RequestAsJsonAsync<TRequest>(
        HttpMethod httpMethod,
        string path,
        TRequest requestObject,
        Dictionary<string, string>? requestHeaders = null,
        string requestLabel = "")
        where TRequest : class, new()
    {
        var requestContentString = JsonSerializer.Serialize(requestObject, JsonSerializerOptions);

        var response = await this.RequestAsync(
            httpMethod: httpMethod,
            path: path,
            requestContent: new JsonContent(requestContentString, Encoding.UTF8),
            requestHeaders: requestHeaders,
            requestLabel: requestLabel);

        return response.StatusCode;
    }

    public async Task<TResponse?> RequestAsJsonAsync<TResponse>(
        HttpMethod httpMethod,
        string path,
        Dictionary<string, string>? requestHeaders = null,
        string requestLabel = "")
        where TResponse : class, new()
    {
        var response = await this.RequestAsync(
            httpMethod: httpMethod,
            path: path,
            requestHeaders: requestHeaders,
            requestLabel: requestLabel);

        var responseObject = JsonSerializer.Deserialize<TResponse>(response.ContentAsUtf8, JsonSerializerOptions);

        return responseObject;
    }

    //
    public Task<int> GetAsJsonAsync<TRequest>(
        string path,
        TRequest requestObject,
        Dictionary<string, string>? requestHeaders = null,
        string requestLabel = "")
        where TRequest : class, new()
    {
        return this.RequestAsJsonAsync<TRequest>(
            httpMethod: HttpMethod.Get,
            path: path,
            requestObject: requestObject,
            requestHeaders: requestHeaders,
            requestLabel: requestLabel);
    }

    public Task<TResponse?> GetAsJsonAsync<TResponse>(
        string path,
        Dictionary<string, string>? requestHeaders = null,
        string requestLabel = "")
        where TResponse : class, new()
    {
        return this.RequestAsJsonAsync<TResponse>(
            httpMethod: HttpMethod.Get,
            path: path,
            requestHeaders: requestHeaders,
            requestLabel: requestLabel);
    }

    public Task<TResponse?> GetAsJsonAsync<TResponse, TRequest>(
        string path,
        TRequest requestObject,
        Dictionary<string, string>? requestHeaders = null,
        string requestLabel = "")
        where TResponse : class, new()
        where TRequest : class, new()
    {
        return this.RequestAsJsonAsync<TResponse, TRequest>(
            httpMethod: HttpMethod.Get,
            path: path,
            requestObject: requestObject,
            requestHeaders: requestHeaders,
            requestLabel: requestLabel);
    }

    //
    public Task<int> PostAsJsonAsync<TRequest>(
        string path,
        TRequest requestObject,
        Dictionary<string, string>? requestHeaders = null,
        string requestLabel = "")
        where TRequest : class, new()
    {
        return this.RequestAsJsonAsync<TRequest>(
            httpMethod: HttpMethod.Post,
            path: path,
            requestObject: requestObject,
            requestHeaders: requestHeaders,
            requestLabel: requestLabel);
    }

    public Task<TResponse?> PostAsJsonAsync<TResponse, TRequest>(
        string path,
        TRequest requestObject,
        Dictionary<string, string>? requestHeaders = null,
        string requestLabel = "")
        where TResponse : class, new()
        where TRequest : class, new()
    {
        return this.RequestAsJsonAsync<TResponse, TRequest>(
            httpMethod: HttpMethod.Post,
            path: path,
            requestObject: requestObject,
            requestHeaders: requestHeaders,
            requestLabel: requestLabel);
    }
    
    public Task<TResponse?> PostAsJsonAsync<TResponse>(
        string path,
        Dictionary<string, string>? requestHeaders = null,
        string requestLabel = "")
        where TResponse : class, new()
    {
        return this.RequestAsJsonAsync<TResponse>(
            httpMethod: HttpMethod.Post,
            path: path,
            requestHeaders: requestHeaders,
            requestLabel: requestLabel);
    }

    //
    public Task<int> PutAsJsonAsync<TRequest>(
        string path,
        TRequest requestObject,
        Dictionary<string, string>? requestHeaders = null,
        string requestLabel = "")
        where TRequest : class, new()
    {
        return this.RequestAsJsonAsync<TRequest>(
            httpMethod: HttpMethod.Put,
            path: path,
            requestObject: requestObject,
            requestHeaders: requestHeaders,
            requestLabel: requestLabel);
    }

    public Task<TResponse?> PutAsJsonAsync<TResponse, TRequest>(
        string path,
        TRequest requestObject,
        Dictionary<string, string>? requestHeaders = null,
        string requestLabel = "")
        where TResponse : class, new()
        where TRequest : class, new()
    {
        return this.RequestAsJsonAsync<TResponse, TRequest>(
            httpMethod: HttpMethod.Put,
            path: path,
            requestObject: requestObject,
            requestHeaders: requestHeaders,
            requestLabel: requestLabel);
    }

    public Task<TResponse?> PutAsJsonAsync<TResponse>(
        string path,
        Dictionary<string, string>? requestHeaders = null,
        string requestLabel = "")
        where TResponse : class, new()
    {
        return this.RequestAsJsonAsync<TResponse>(
            httpMethod: HttpMethod.Put,
            path: path,
            requestHeaders: requestHeaders, 
            requestLabel: requestLabel);
    }

    //
    public Task<int> DeleteAsJsonAsync<TRequest>(
        string path,
        TRequest requestObject,
        Dictionary<string, string>? requestHeaders = null,
        string requestLabel = "")
        where TRequest : class, new()
    {
        return this.RequestAsJsonAsync<TRequest>(
            httpMethod: HttpMethod.Delete,
            path: path,
            requestObject: requestObject,
            requestHeaders: requestHeaders,
            requestLabel: requestLabel);
    }

    public Task<TResponse?> DeleteAsJsonAsync<TResponse, TRequest>(
        string path,
        TRequest requestObject,
        Dictionary<string, string>? requestHeaders = null,
        string requestLabel = "")
        where TResponse : class, new()
        where TRequest : class, new()
    {
        return this.RequestAsJsonAsync<TResponse, TRequest>(
            httpMethod: HttpMethod.Delete,
            path: path,
            requestObject: requestObject,
            requestHeaders: requestHeaders,
            requestLabel: requestLabel);
    }

    public Task<TResponse?> DeleteAsJsonAsync<TResponse>(
        string path,
        Dictionary<string, string>? requestHeaders = null,
        string requestLabel = "")
        where TResponse : class, new()
    {
        return this.RequestAsJsonAsync<TResponse>(
            httpMethod: HttpMethod.Delete,
            path: path,
            requestHeaders: requestHeaders,
            requestLabel: requestLabel);
    }
}
