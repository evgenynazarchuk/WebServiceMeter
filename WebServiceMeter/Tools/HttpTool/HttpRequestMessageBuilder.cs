using System;
using System.Collections.Generic;
using System.Net.Http;

namespace WebServiceMeter
{
    public class HttpRequestMessageBuilder
    {
        public string RequestUri { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public HttpMethod HttpMethod { get; set; } = HttpMethod.Get;
        public Version HttpVersion { get; set; } = new(2, 0);

        private HttpVersionPolicy _policy = HttpVersionPolicy.RequestVersionOrLower;
        private Dictionary<string, IEnumerable<string>>? _headers;

        public HttpRequestMessageBuilder UseRequest(string requestUri)
        {
            RequestUri = requestUri;
            return this;
        }

        public HttpRequestMessageBuilder UseContent(string content)
        {
            Content = content;
            return this;
        }

        public HttpRequestMessageBuilder UseHttpMethod(string method)
        {
            HttpMethod = new HttpMethod(method);
            return this;
        }

        public HttpRequestMessageBuilder UseVersion(string version)
        {
            HttpVersion = new Version(version);
            return this;
        }

        public HttpRequestMessageBuilder UseVersionPolicy(HttpVersionPolicy policy)
        {
            _policy = policy;
            return this;
        }

        public HttpRequestMessageBuilder UseRequestHeaders(Dictionary<string, string> headers)
        {
            foreach ((var key, var value) in headers)
            {
                _headers?.Add(key, new List<string> { value });
            }

            return this;
        }

        public HttpRequestMessageBuilder UseRequestHeaders(Dictionary<string, IEnumerable<string>> headers)
        {
            _headers = headers;
            return this;
        }

        public HttpRequestMessage Build()
        {
            var httpResponseMessage = new HttpRequestMessage()
            {
                RequestUri = new Uri(this.RequestUri, UriKind.Relative),
                Content = new StringContent(Content),
                Method = HttpMethod,
                Version = HttpVersion,
                VersionPolicy = _policy
            };

            if (_headers is not null)
            {
                foreach ((var key, var value) in _headers)
                {
                    httpResponseMessage.Headers.Add(key, value);
                }
            }

            return httpResponseMessage;
        }
    }
}
