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
using System.Net;
using System.Xml;

namespace WebServiceMeter.Users;

public abstract partial class BasicHttpUser : BasicUser
{
    //
    public Task<XmlDocument> GetXmlDocument(
        string path,
        Dictionary<string, string>? requestHeaders = null,
        string requestLabel = "")
    {
        return this.Tool.GetXmlDocumentAsync(
            path: path,
            requestHeaders: requestHeaders,
            userName: this.UserName,
            requestLabel: requestLabel);
    }

    //        
    public Task<TResponse?> GetAsXml<TResponse>(
        string path,
        Dictionary<string, string>? requestHeaders = null,
        string requestLabel = "")
        where TResponse : class, new()
    {
        return this.Tool.GetAsXmlAsync<TResponse>(
            path: path,
            requestHeaders: requestHeaders,
            userName: this.UserName,
            requestLabel: requestLabel);
    }

    public Task<HttpStatusCode> GetAsXml<TRequest>(
        string path,
        TRequest requestObject,
        Dictionary<string, string>? requestHeaders = null,
        Encoding? requestContentEncoding = null,
        string requestLabel = "")
        where TRequest : class, new()
    {
        return this.Tool.GetAsXmlAsync<TRequest>(
            path: path,
            requestObject: requestObject,
            requestHeaders: requestHeaders,
            requestContentEncoding: requestContentEncoding,
            userName: this.UserName,
            requestLabel: requestLabel);
    }

    public Task<TResponse?> GetAsXml<TResponse, TRequest>(
        string path,
        TRequest requestObject,
        Dictionary<string, string>? requestHeaders = null,
        Encoding? requestContentEncoding = null,
        string requestLabel = "")
        where TRequest : class, new()
        where TResponse : class, new()
    {
        return this.Tool.GetAsXmlAsync<TResponse, TRequest>(
            path: path,
            requestObject: requestObject,
            requestHeaders: requestHeaders,
            requestContentEncoding: requestContentEncoding,
            userName: this.UserName,
            requestLabel: requestLabel);
    }

    //
    public Task<HttpStatusCode> PostAsXml<TRequest>(
        string path,
        TRequest requestObject,
        Dictionary<string, string>? requestHeaders = null,
        Encoding? requestContentEncoding = null,
        string requestLabel = "")
        where TRequest : class, new()
    {
        return this.Tool.PostAsXmlAsync<TRequest>(
            path: path,
            requestObject: requestObject,
            requestHeaders: requestHeaders,
            requestContentEncoding: requestContentEncoding,
            userName: this.UserName,
            requestLabel: requestLabel);
    }

    public Task<TResponse?> PostAsXml<TResponse>(
        string path,
        Dictionary<string, string>? requestHeaders = null,
        string requestLabel = "")
        where TResponse : class, new()
    {
        return this.Tool.PostAsXmlAsync<TResponse>(
            path: path,
            requestHeaders: requestHeaders,
            userName: this.UserName,
            requestLabel: requestLabel);
    }

    public Task<TResponse?> PostAsXml<TResponse, TRequest>(
        string path,
        TRequest requestObject,
        Dictionary<string, string>? requestHeaders = null,
        Encoding? requestContentEncoding = null,
        string requestLabel = "")
        where TRequest : class, new()
        where TResponse : class, new()
    {
        return this.Tool.PostAsXmlAsync<TResponse, TRequest>(
            path: path,
            requestObject: requestObject,
            requestHeaders: requestHeaders,
            requestContentEncoding: requestContentEncoding,
            userName: this.UserName,
            requestLabel: requestLabel);
    }

    //
    public Task<TResponse?> PutAsXml<TResponse>(
        string path,
        Dictionary<string, string>? requestHeaders = null,
        string requestLabel = "")
        where TResponse : class, new()
    {
        return this.Tool.PutAsXmlAsync<TResponse>(
            path: path,
            requestHeaders: requestHeaders,
            userName: this.UserName,
            requestLabel: requestLabel);
    }

    public Task<HttpStatusCode> PutAsXml<TRequest>(
        string path,
        TRequest requestObject,
        Dictionary<string, string>? requestHeaders = null,
        Encoding? requestContentEncoding = null,
        string requestLabel = "")
        where TRequest : class, new()
    {
        return this.Tool.PutAsXmlAsync<TRequest>(
            path: path,
            requestObject: requestObject,
            requestHeaders: requestHeaders,
            requestContentEncoding: requestContentEncoding,
            userName: this.UserName,
            requestLabel: requestLabel);
    }

    public Task<TResponse?> PutAsXml<TResponse, TRequest>(
        string path,
        TRequest requestObject,
        Dictionary<string, string>? requestHeaders = null,
        Encoding? requestContentEncoding = null,
        string requestLabel = "")
        where TRequest : class, new()
        where TResponse : class, new()
    {
        return this.Tool.PutAsXmlAsync<TResponse, TRequest>(
            path: path,
            requestObject: requestObject,
            requestHeaders: requestHeaders,
            requestContentEncoding: requestContentEncoding,
            userName: this.UserName,
            requestLabel: requestLabel);
    }

    //
    public Task<TResponse?> DeleteAsXml<TResponse>(
        string path,
        Dictionary<string, string>? requestHeaders = null,
        string requestLabel = "")
        where TResponse : class, new()
    {
        return this.Tool.DeleteAsXmlAsync<TResponse>(
            path: path,
            requestHeaders: requestHeaders,
            userName: this.UserName,
            requestLabel: requestLabel);
    }

    public Task<HttpStatusCode> DeleteAsXml<TRequest>(
        string path,
        TRequest requestObject,
        Dictionary<string, string>? requestHeaders = null,
        Encoding? requestContentEncoding = null,
        string requestLabel = "")
        where TRequest : class, new()
    {
        return this.Tool.DeleteAsXmlAsync<TRequest>(
            path: path,
            requestObject: requestObject,
            requestHeaders: requestHeaders,
            requestContentEncoding: requestContentEncoding,
            userName: this.UserName,
            requestLabel: requestLabel);
    }

    public Task<TypeResponseObject?> DeleteAsXml<TypeResponseObject, TypeRequestObject>(
        string path,
        TypeRequestObject requestObject,
        Dictionary<string, string>? requestHeaders = null,
        Encoding? requestContentEncoding = null,
        string requestLabel = "")
        where TypeRequestObject : class, new()
        where TypeResponseObject : class, new()
    {
        return this.Tool.DeleteAsXmlAsync<TypeResponseObject, TypeRequestObject>(
            path: path,
            requestObject: requestObject,
            requestHeaders: requestHeaders,
            requestContentEncoding: requestContentEncoding,
            userName: this.UserName,
            requestLabel: requestLabel);
    }
}