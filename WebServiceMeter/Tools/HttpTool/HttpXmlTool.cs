using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Net;
using WebServiceMeter.Tools.HttpTool;

namespace WebServiceMeter
{
    public partial class HttpTool
    {
        private readonly XmlWriterSettings _xmlSerializationOptions = new()
        {
            Indent = true,
            OmitXmlDeclaration = true,
            CheckCharacters = false,
            Encoding = Encoding.UTF8
        };

        //private static void AddXmlAcceptHeader(ref Dictionary<string, string>? headers)
        //{
        //    if (headers is null)
        //    {
        //        headers = new();
        //    }
        //
        //    headers.Add("Accept", "application/xml");
        //}

        public async Task<TypeResponseObject?> RequestXmlAsync<TypeResponseObject, TypeRequestObject>(
            HttpMethod httpMethod,
            string requestUri,
            TypeRequestObject requestObject,
            Dictionary<string, string>? requestHeaders = null,
            Encoding? requestContentEncoding = null
            )
            where TypeRequestObject : class, new()
            where TypeResponseObject : class, new()
        {
            // TODO сделать чтение параметра Accept из конфига
            // так как могут быть application/xml или text/xml
            //HttpTool.AddXmlAcceptHeader(ref requestHeaders);

            string requestXmlContentString = requestObject.FromObjectToXmlString(this._xmlSerializationOptions);

            HttpResponse response = await this.RequestAsync(
                httpMethod: httpMethod,
                path: requestUri,
                requestContent: new XmlContent(requestXmlContentString, requestContentEncoding),
                requestHeaders: requestHeaders);

            TypeResponseObject? responseObject = response.ContentAsUtf8String.FromXmlStringToObject<TypeResponseObject>();

            return responseObject;
        }

        public async Task<HttpStatusCode> RequestXmlAsync<TypeRequestObject>(
            HttpMethod httpMethod,
            string requestUri,
            TypeRequestObject requestObject,
            Dictionary<string, string>? requestHeaders = null,
            Encoding? requestContentEncoding = null
            )
            where TypeRequestObject : class, new()
        {
            string requestXmlContentString = requestObject.FromObjectToXmlString(this._xmlSerializationOptions);

            HttpResponse response = await this.RequestAsync(
                httpMethod: httpMethod,
                path: requestUri,
                requestContent: new XmlContent(requestXmlContentString, requestContentEncoding),
                requestHeaders: requestHeaders);

            return (HttpStatusCode)response.StatusCode;
        }

        public async Task<TypeResponseObject?> RequestXmlAsync<TypeResponseObject>(
            HttpMethod httpMethod,
            string requestUri,
            Dictionary<string, string>? requestHeaders = null)
            where TypeResponseObject : class, new()
        {
            // TODO сделать чтение параметра Accept из конфига
            // так как могут быть application/xml или text/xml
            //HttpTool.AddXmlAcceptHeader(ref requestHeaders);

            HttpResponse response = await this.RequestAsync(
                httpMethod: httpMethod,
                path: requestUri,
                requestHeaders: requestHeaders);

            TypeResponseObject? responseObject = response.ContentAsUtf8String.FromXmlStringToObject<TypeResponseObject>();

            return responseObject;
        }
    }
}
