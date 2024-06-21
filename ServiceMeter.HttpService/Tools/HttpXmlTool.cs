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

using System.Net;
using System.Text;
using System.Xml;
using ServiceMeter.Extensions;

namespace ServiceMeter.HttpService.Tools;

public partial class HttpTool
{
    private readonly XmlWriterSettings _xmlSerializationOptions = new()
    {
        Indent = true,
        OmitXmlDeclaration = true,
        CheckCharacters = false,
        Encoding = Encoding.UTF8
    };

    private static void AddXmlAcceptHeader(ref Dictionary<string, string>? headers)
    {
        if (headers is null)
        {
            headers = new();
        }

        headers.Add("Accept", "application/xml");
    }

    public async Task<TResponse?> RequestAsXmlAsync<TResponse, TRequest>(
        HttpMethod httpMethod,
        string path,
        TRequest requestObject,
        Dictionary<string, string>? requestHeaders = null,
        Encoding? requestContentEncoding = null,
        string requestLabel = "")
        where TRequest : class, new()
        where TResponse : class, new()
    {
        HttpTool.AddXmlAcceptHeader(ref requestHeaders);

        var requestXmlContentString = requestObject.FromObjectToXmlString(this._xmlSerializationOptions);

        var response = await this.RequestAsync(
            httpMethod: httpMethod,
            path: path,
            requestContent: new XmlContent(requestXmlContentString, requestContentEncoding ?? Encoding.UTF8),
            requestHeaders: requestHeaders,
            requestLabel: requestLabel);

        var responseObject = response.ContentAsUtf8.FromXmlStringToObject<TResponse>();

        return responseObject;
    }

    public async Task<HttpStatusCode> RequestAsXmlAsync<TRequest>(
        HttpMethod httpMethod,
        string path,
        TRequest requestObject,
        Dictionary<string, string>? requestHeaders = null,
        Encoding? requestContentEncoding = null,
        string requestLabel = "")
        where TRequest : class, new()
    {
        var requestXmlContentString = requestObject.FromObjectToXmlString(this._xmlSerializationOptions);

        var response = await this.RequestAsync(
            httpMethod: httpMethod,
            path: path,
            requestContent: new XmlContent(requestXmlContentString, requestContentEncoding ?? Encoding.UTF8),
            requestHeaders: requestHeaders,
            requestLabel: requestLabel);

        return (HttpStatusCode)response.StatusCode;
    }

    public async Task<TResponse?> RequestAsXmlAsync<TResponse>(
        HttpMethod httpMethod,
        string path,
        Dictionary<string, string>? requestHeaders = null,
        string requestLabel = "")
        where TResponse : class, new()
    {
        HttpTool.AddXmlAcceptHeader(ref requestHeaders);

        var response = await this.RequestAsync(
            httpMethod: httpMethod,
            path: path,
            requestHeaders: requestHeaders,
            requestLabel: requestLabel);

        var responseObject = response.ContentAsUtf8.FromXmlStringToObject<TResponse>();

        return responseObject;
    }

    //

    public async Task<XmlDocument> GetXmlDocumentAsync(
        string path,
        Dictionary<string, string>? requestHeaders = null,
        string requestLabel = "")
    {
        var response = await this.RequestAsync(
            httpMethod: HttpMethod.Get,
            path: path,
            requestHeaders: requestHeaders,
            requestLabel: requestLabel);

        var content = response.ContentAsUtf8;

        var xmlDocument = new XmlDocument();
        
        xmlDocument.LoadXml(content);

        return xmlDocument;
    }

    //
    public Task<TResponse?> GetAsXmlAsync<TResponse>(
        string path,
        Dictionary<string, string>? requestHeaders = null,
        string requestLabel = "")
        where TResponse : class, new()
    {
        return this.RequestAsXmlAsync<TResponse>(
            httpMethod: HttpMethod.Get,
            path: path,
            requestHeaders: requestHeaders,
            requestLabel: requestLabel);
    }

    public Task<HttpStatusCode> GetAsXmlAsync<TRequest>(
        string path,
        TRequest requestObject,
        Dictionary<string, string>? requestHeaders = null,
        Encoding? requestContentEncoding = null,
        string requestLabel = "")
        where TRequest : class, new()
    {
        return this.RequestAsXmlAsync<TRequest>(
            httpMethod: HttpMethod.Get,
            path: path,
            requestObject: requestObject,
            requestHeaders: requestHeaders,
            requestContentEncoding: requestContentEncoding,
            requestLabel: requestLabel);
    }

    public Task<TResponse?> GetAsXmlAsync<TResponse, TRequest>(
        string path,
        TRequest requestObject,
        Dictionary<string, string>? requestHeaders = null,
        Encoding? requestContentEncoding = null,
        string requestLabel = "")
        where TRequest : class, new()
        where TResponse : class, new()
    {
        return this.RequestAsXmlAsync<TResponse, TRequest>(
            httpMethod: HttpMethod.Get,
            path: path,
            requestObject: requestObject,
            requestHeaders: requestHeaders,
            requestContentEncoding: requestContentEncoding,
            requestLabel: requestLabel);
    }

    //
    public Task<HttpStatusCode> PostAsXmlAsync<TRequest>(
        string path,
        TRequest requestObject,
        Dictionary<string, string>? requestHeaders = null,
        Encoding? requestContentEncoding = null,
        string requestLabel = "")
        where TRequest : class, new()
    {
        return this.RequestAsXmlAsync<TRequest>(
            httpMethod: HttpMethod.Post,
            path: path,
            requestObject: requestObject,
            requestHeaders: requestHeaders,
            requestContentEncoding: requestContentEncoding,
            requestLabel: requestLabel);
    }

    public Task<TResponse?> PostAsXmlAsync<TResponse>(
        string path,
        Dictionary<string, string>? requestHeaders = null,
        string requestLabel = "")
        where TResponse : class, new()
    {
        return this.RequestAsXmlAsync<TResponse>(
            httpMethod: HttpMethod.Post,
            path: path,
            requestHeaders: requestHeaders,
            requestLabel: requestLabel);
    }

    public Task<TResponse?> PostAsXmlAsync<TResponse, TRequest>(
        string path,
        TRequest requestObject,
        Dictionary<string, string>? requestHeaders = null,
        Encoding? requestContentEncoding = null,
        string requestLabel = "")
        where TRequest : class, new()
        where TResponse : class, new()
    {
        return this.RequestAsXmlAsync<TResponse, TRequest>(
            httpMethod: HttpMethod.Post,
            path: path,
            requestObject: requestObject,
            requestHeaders: requestHeaders,
            requestContentEncoding: requestContentEncoding,
            requestLabel: requestLabel);
    }

    //
    public Task<TResponse?> PutAsXmlAsync<TResponse>(
        string path,
        Dictionary<string, string>? requestHeaders = null,
        string requestLabel = "")
        where TResponse : class, new()
    {
        return this.RequestAsXmlAsync<TResponse>(
            httpMethod: HttpMethod.Put,
            path: path,
            requestHeaders: requestHeaders,
            requestLabel: requestLabel);
    }

    public Task<HttpStatusCode> PutAsXmlAsync<TRequest>(
        string path,
        TRequest requestObject,
        Dictionary<string, string>? requestHeaders = null,
        Encoding? requestContentEncoding = null,
        string requestLabel = "")
        where TRequest : class, new()
    {
        return this.RequestAsXmlAsync<TRequest>(
            httpMethod: HttpMethod.Put,
            path: path,
            requestObject: requestObject,
            requestHeaders: requestHeaders,
            requestContentEncoding: requestContentEncoding,
            requestLabel: requestLabel);
    }

    public Task<TResponse?> PutAsXmlAsync<TResponse, TRequest>(
        string path,
        TRequest requestObject,
        Dictionary<string, string>? requestHeaders = null,
        Encoding? requestContentEncoding = null,
        string requestLabel = "")
        where TRequest : class, new()
        where TResponse : class, new()
    {
        return this.RequestAsXmlAsync<TResponse, TRequest>(
            httpMethod: HttpMethod.Put,
            path: path,
            requestObject: requestObject,
            requestHeaders: requestHeaders,
            requestContentEncoding: requestContentEncoding,
            requestLabel: requestLabel);
    }

    //
    public Task<TResponse?> DeleteAsXmlAsync<TResponse>(
        string path,
        Dictionary<string, string>? requestHeaders = null,
        string requestLabel = "")
        where TResponse : class, new()
    {
        return this.RequestAsXmlAsync<TResponse>(
            httpMethod: HttpMethod.Delete,
            path: path,
            requestHeaders: requestHeaders,
            requestLabel: requestLabel);
    }

    public Task<HttpStatusCode> DeleteAsXmlAsync<TRequest>(
        string path,
        TRequest requestObject,
        Dictionary<string, string>? requestHeaders = null,
        Encoding? requestContentEncoding = null,
        string requestLabel = "")
        where TRequest : class, new()
    {
        return this.RequestAsXmlAsync<TRequest>(
            httpMethod: HttpMethod.Delete,
            path: path, requestObject,
            requestHeaders: requestHeaders,
            requestContentEncoding: requestContentEncoding,
            requestLabel: requestLabel);
    }

    public Task<TResponse?> DeleteAsXmlAsync<TResponse, TRequest>(
        string path,
        TRequest requestObject,
        Dictionary<string, string>? requestHeaders = null,
        Encoding? requestContentEncoding = null,
        string requestLabel = "")
        where TRequest : class, new()
        where TResponse : class, new()
    {
        return this.RequestAsXmlAsync<TResponse, TRequest>(
            httpMethod: HttpMethod.Delete,
            path: path,
            requestObject: requestObject,
            requestHeaders: requestHeaders,
            requestContentEncoding: requestContentEncoding,
            requestLabel: requestLabel);
    }
}
