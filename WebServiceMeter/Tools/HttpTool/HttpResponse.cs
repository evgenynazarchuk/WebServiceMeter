using System.Text;
using System.Net.Http.Headers;

namespace WebServiceMeter
{
    public sealed class HttpResponse
    {
        public HttpResponse(
            int statusCode,
            byte[] content,
            string? filename)
        {
            this.StatusCode = statusCode;
            this.ContentAsBytes = content;
            this.Filename = filename;
        }

        public readonly int StatusCode;

        public readonly byte[] ContentAsBytes;

        public readonly string? Filename = null;

        public string ContentAsUtf8String => Encoding.UTF8.GetString(this.ContentAsBytes);
    }
}
