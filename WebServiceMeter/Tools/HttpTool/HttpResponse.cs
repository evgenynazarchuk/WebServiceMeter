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
            this.Content = content;
            this.Filename = filename;
        }

        public readonly int StatusCode;

        public readonly byte[] Content;

        public readonly string? Filename = null;

        public string ContentAsUTF8 => Encoding.UTF8.GetString(this.Content);

        public string ContentAsASCII => Encoding.ASCII.GetString(this.Content);

        public string ContentAsUnicode => Encoding.Unicode.GetString(this.Content);

        public string ContentAsUTF32 => Encoding.UTF32.GetString(this.Content);
    }
}
