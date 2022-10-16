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

namespace ServiceMeter.HttpService.Interfaces;

public interface IHttpJsonUser
{
    Task<TResponseObject?> RequestAsJson<TResponseObject, TRequestObject>(
        HttpMethod httpMethod,
        string requestUri,
        TRequestObject requestObject,
        string requestLabel = "",
        Dictionary<string, string>? requestHeaders = null)
        where TRequestObject : class, new()
        where TResponseObject : class, new();

    Task<string> RequestAsJson<TRequestObject>(
        HttpMethod httpMethod,
        string requestUri,
        TRequestObject requestObject,
        string requestLabel = "",
        Dictionary<string, string>? requestHeaders = null)
        where TRequestObject : class, new();

    Task<TResponseObject?> RequestAsJson<TResponseObject>(
        HttpMethod httpMethod,
        string requestUri,
        string requestLabel = "",
        Dictionary<string, string>? requestHeaders = null)
        where TResponseObject : class, new();

    Task<TResponseObject?> GetAsJson<TResponseObject>(
        string path,
        string requestLabel = "",
        Dictionary<string, string>? requestHeaders = null)
        where TResponseObject : class, new();

    Task<string> PostAsJson<TRequestObject>(
        string path,
        TRequestObject requestObject,
        string requestLabel = "",
        Dictionary<string, string>? requestHeaders = null)
        where TRequestObject : class, new();

    Task<string> PutAsJson<TRequestObject>(
        string path,
        TRequestObject requestObject,
        string requestLabel = "",
        Dictionary<string, string>? requestHeaders = null)
        where TRequestObject : class, new();

    Task<TResponseObject?> PostAsJson<TResponseObject, TRequestObject>(
        string path,
        TRequestObject requestObject,
        string requestLabel = "",
        Dictionary<string, string>? requestHeaders = null)
        where TResponseObject : class, new()
        where TRequestObject : class, new();

    Task<TResponseObject?> PutAsJson<TResponseObject, TRequestObject>(
        string path,
        TRequestObject requestObject,
        string requestLabel = "",
        Dictionary<string, string>? requestHeaders = null)
        where TResponseObject : class, new()
        where TRequestObject : class, new();
}
