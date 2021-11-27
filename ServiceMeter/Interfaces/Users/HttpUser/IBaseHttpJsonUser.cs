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

namespace ServiceMeter.Interfaces;

public interface IBaseHttpJsonUser
{
    Task<ResponseObjectType?> RequestAsJson<ResponseObjectType, RequestObjectType>(
        HttpMethod httpMethod,
        string requestUri,
        RequestObjectType requestObject,
        string requestLabel = "",
        Dictionary<string, string>? requestHeaders = null)
        where RequestObjectType : class, new()
        where ResponseObjectType : class, new();

    Task<string> RequestAsJson<RequestObjectType>(
        HttpMethod httpMethod,
        string requestUri,
        RequestObjectType requestObject,
        string requestLabel = "",
        Dictionary<string, string>? requestHeaders = null)
        where RequestObjectType : class, new();

    Task<ResponseObjectType?> RequestAsJson<ResponseObjectType>(
        HttpMethod httpMethod,
        string requestUri,
        string requestLabel = "",
        Dictionary<string, string>? requestHeaders = null)
        where ResponseObjectType : class, new();

    Task<ResponseObjectType?> GetAsJson<ResponseObjectType>(
        string path,
        string requestLabel = "",
        Dictionary<string, string>? requestHeaders = null)
        where ResponseObjectType : class, new();

    Task<string> PostAsJson<RequestObjectType>(
        string path,
        RequestObjectType requestObject,
        string requestLabel = "",
        Dictionary<string, string>? requestHeaders = null)
        where RequestObjectType : class, new();

    Task<string> PutAsJson<RequestObjectType>(
        string path,
        RequestObjectType requestObject,
        string requestLabel = "",
        Dictionary<string, string>? requestHeaders = null)
        where RequestObjectType : class, new();

    Task<ResponseObjectType?> PostAsJson<ResponseObjectType, RequestObjectType>(
        string path,
        RequestObjectType requestObject,
        string requestLabel = "",
        Dictionary<string, string>? requestHeaders = null)
        where ResponseObjectType : class, new()
        where RequestObjectType : class, new();

    Task<ResponseObjectType?> PutAsJson<ResponseObjectType, RequestObjectType>(
        string path,
        RequestObjectType requestObject,
        string requestLabel = "",
        Dictionary<string, string>? requestHeaders = null)
        where ResponseObjectType : class, new()
        where RequestObjectType : class, new();
}
