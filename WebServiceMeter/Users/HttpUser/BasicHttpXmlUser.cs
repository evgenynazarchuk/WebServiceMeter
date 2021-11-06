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
        public async Task<XmlDocument> GetXmlDocumentAsync(
            string requestUri,
            Dictionary<string, string>? requestHeaders = null
            )
        {
            HttpResponse response = await this.Tool.RequestAsync(
                httpMethod: HttpMethod.Get,
                path: requestUri,
                requestHeaders: requestHeaders);

            var content = response.ContentAsUtf8String;

            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(content);

            return xmlDocument;
        }
        
        public Task<TypeResponseObject?> GetXmlAsync<TypeResponseObject>(
            string requestUri,
            Dictionary<string, string>? requestHeaders = null)
            where TypeResponseObject : class, new()
        {
            return this.Tool.RequestXmlAsync<TypeResponseObject>(
                httpMethod: HttpMethod.Get,
                requestUri: requestUri,
                requestHeaders: requestHeaders);
        }

        public Task<HttpStatusCode> GetXmlAsync<TypeRequestObject>(
            string requestUri,
            TypeRequestObject requestObject,
            Dictionary<string, string>? requestHeaders = null,
            Encoding? requestContentEncoding = null
            )
            where TypeRequestObject : class, new()
        {
            return this.Tool.RequestXmlAsync<TypeRequestObject>(
                httpMethod: HttpMethod.Get,
                requestUri: requestUri,
                requestObject: requestObject,
                requestHeaders: requestHeaders,
                requestContentEncoding: requestContentEncoding);
        }

        public Task<TypeResponseObject?> GetXmlAsync<TypeResponseObject, TypeRequestObject>(
            string requestUri,
            TypeRequestObject requestObject,
            Dictionary<string, string>? requestHeaders = null,
            Encoding? requestContentEncoding = null
            )
            where TypeRequestObject : class, new()
            where TypeResponseObject : class, new()
        {
            return this.Tool.RequestXmlAsync<TypeResponseObject, TypeRequestObject>(
                httpMethod: HttpMethod.Get,
                requestUri: requestUri,
                requestObject: requestObject,
                requestHeaders: requestHeaders,
                requestContentEncoding: requestContentEncoding);
        }

        public Task<HttpStatusCode> PostXmlAsync<TypeRequestObject>(
            string requestUri,
            TypeRequestObject requestObject,
            Dictionary<string, string>? requestHeaders = null,
            Encoding? requestContentEncoding = null
            )
            where TypeRequestObject : class, new()
        {
            return this.Tool.RequestXmlAsync<TypeRequestObject>(
                httpMethod: HttpMethod.Post,
                requestUri: requestUri,
                requestObject: requestObject,
                requestHeaders: requestHeaders,
                requestContentEncoding: requestContentEncoding);
        }

        public Task<TypeResponseObject?> PostXmlAsync<TypeResponseObject>(
            string requestUri,
            Dictionary<string, string>? requestHeaders = null
            )
            where TypeResponseObject : class, new()
        {
            return this.Tool.RequestXmlAsync<TypeResponseObject>(
                httpMethod: HttpMethod.Post,
                requestUri: requestUri,
                requestHeaders: requestHeaders);
        }

        public Task<TypeResponseObject?> PostXmlAsync<TypeResponseObject, TypeRequestObject>(
            string requestUri,
            TypeRequestObject requestObject,
            Dictionary<string, string>? requestHeaders = null,
            Encoding? requestContentEncoding = null
            )
            where TypeRequestObject : class, new()
            where TypeResponseObject : class, new()
        {
            return this.Tool.RequestXmlAsync<TypeResponseObject, TypeRequestObject>(
                httpMethod: HttpMethod.Post,
                requestUri: requestUri,
                requestObject: requestObject,
                requestHeaders: requestHeaders,
                requestContentEncoding: requestContentEncoding);
        }

        public Task<TypeResponseObject?> PutXmlAsync<TypeResponseObject>(
            string requestUri,
            Dictionary<string, string>? requestHeaders = null
            )
            where TypeResponseObject : class, new()
        {
            return this.Tool.RequestXmlAsync<TypeResponseObject>(
                httpMethod: HttpMethod.Put,
                requestUri: requestUri,
                requestHeaders: requestHeaders);
        }

        public Task<HttpStatusCode> PutXmlAsync<TypeRequestObject>(
            string requestUri,
            TypeRequestObject requestObject,
            Dictionary<string, string>? requestHeaders = null,
            Encoding? requestContentEncoding = null
            )
            where TypeRequestObject : class, new()
        {
            return this.Tool.RequestXmlAsync<TypeRequestObject>(
                httpMethod: HttpMethod.Put,
                requestUri: requestUri,
                requestObject: requestObject,
                requestHeaders: requestHeaders,
                requestContentEncoding: requestContentEncoding);
        }

        public Task<TypeResponseObject?> PutXmlAsync<TypeResponseObject, TypeRequestObject>(
            string requestUri,
            TypeRequestObject requestObject,
            Dictionary<string, string>? requestHeaders = null,
            Encoding? requestContentEncoding = null
            )
            where TypeRequestObject : class, new()
            where TypeResponseObject : class, new()
        {
            return this.Tool.RequestXmlAsync<TypeResponseObject, TypeRequestObject>(
                httpMethod: HttpMethod.Put,
                requestUri: requestUri,
                requestObject: requestObject,
                requestHeaders: requestHeaders,
                requestContentEncoding: requestContentEncoding);
        }

        public Task<TypeResponseObject?> DeleteXmlAsync<TypeResponseObject>(
            string requestUri,
            Dictionary<string, string>? requestHeaders = null
            )
            where TypeResponseObject : class, new()
        {
            return this.Tool.RequestXmlAsync<TypeResponseObject>(
                httpMethod: HttpMethod.Delete,
                requestUri: requestUri,
                requestHeaders: requestHeaders);
        }

        public Task<HttpStatusCode> DeleteXmlAsync<TypeRequestObject>(
            string requestUri,
            TypeRequestObject requestObject,
            Dictionary<string, string>? requestHeaders = null,
            Encoding? requestContentEncoding = null
            )
            where TypeRequestObject : class, new()
        {
            return this.Tool.RequestXmlAsync<TypeRequestObject>(
                httpMethod: HttpMethod.Delete,
                requestUri: requestUri, requestObject,
                requestHeaders: requestHeaders,
                requestContentEncoding: requestContentEncoding);
        }

        public Task<TypeResponseObject?> DeleteXmlAsync<TypeResponseObject, TypeRequestObject>(
            string requestUri,
            TypeRequestObject requestObject,
            Dictionary<string, string>? requestHeaders = null,
            Encoding? requestContentEncoding = null
            )
            where TypeRequestObject : class, new()
            where TypeResponseObject : class, new()
        {
            return this.Tool.RequestXmlAsync<TypeResponseObject, TypeRequestObject>(
                httpMethod: HttpMethod.Delete,
                requestUri: requestUri,
                requestObject: requestObject,
                requestHeaders: requestHeaders,
                requestContentEncoding: requestContentEncoding);
        }
    }
}
