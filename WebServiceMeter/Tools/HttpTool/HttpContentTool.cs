using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WebServiceMeter.Tools.HttpTool;

namespace WebServiceMeter
{
    public partial class HttpTool
    {
        public Task<HttpResponse> GetAsync(
            string path,
            Dictionary<string, string>? requestHeaders = null,
            string? requestContent = null,
            Encoding? requestContentEncoding = null,
            string userName = "",
            string requestLabel = "")
        {
            return this.RequestAsync(
                httpMethod: HttpMethod.Get,
                path: path,
                requestContent: new StringContent(requestContent ?? "", requestContentEncoding ?? Encoding.UTF8),
                requestHeaders: requestHeaders,
                userName: userName,
                requestLabel: requestLabel);
        }

        public Task<HttpResponse> PostAsync(
            string path,
            Dictionary<string, string>? requestHeaders = null,
            string? requestContent = null,
            Encoding? requestContentEncoding = null,
            string userName = "",
            string requestLabel = "")
        {
            return this.RequestAsync(
                httpMethod: HttpMethod.Post,
                path: path,
                requestContent: new StringContent(requestContent ?? "", requestContentEncoding ?? Encoding.UTF8),
                requestHeaders: requestHeaders,
                userName: userName,
                requestLabel: requestLabel);
        }

        public Task<HttpResponse> PutAsync(
            string path,
            Dictionary<string, string>? requestHeaders = null,
            string? requestContent = null,
            Encoding? requestContentEncoding = null,
            string userName = "",
            string requestLabel = "")
        {
            return this.RequestAsync(
                httpMethod: HttpMethod.Put,
                path: path,
                requestHeaders: requestHeaders,
                requestContent: new StringContent(requestContent ?? "", requestContentEncoding ?? Encoding.UTF8),
                userName: userName,
                requestLabel: requestLabel);
        }

        public Task<HttpResponse> DeleteAsync(
            string path,
            Dictionary<string, string>? requestHeaders = null,
            string? requestContent = null,
            Encoding? requestContentEncoding = null,
            string userName = "",
            string requestLabel = "")
        {
            return this.RequestAsync(
                httpMethod: HttpMethod.Delete,
                path: path,
                requestHeaders: requestHeaders,
                requestContent: new StringContent(requestContent ?? "", requestContentEncoding ?? Encoding.UTF8),
                userName: userName,
                requestLabel: requestLabel);
        }
    }
}
