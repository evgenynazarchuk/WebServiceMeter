using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text;
using System.Net;
using System.Xml;

namespace WebServiceMeter.Users
{
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
}