using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebServiceMeter.Users
{
    public abstract partial class BasicHttpUser : BasicUser
    {
        //
        public Task<TResponse?> RequestAsJson<TResponse, TRequest>(
            HttpMethod httpMethod,
            string requestUri,
            TRequest requestObject,
            string requestLabel = "",
            Dictionary<string, string>? requestHeaders = null)
            where TRequest : class, new()
            where TResponse : class, new()
        {
            return this.Tool.RequestAsJsonAsync<TResponse, TRequest>(
                httpMethod,
                requestUri,
                requestObject,
                requestHeaders,
                this.UserName,
                requestLabel);
        }

        public Task<int> RequestAsJson<TRequest>(
            HttpMethod httpMethod,
            string requestUri,
            TRequest requestObject,
            string requestLabel = "",
            Dictionary<string, string>? requestHeaders = null)
            where TRequest : class, new()
        {
            var httpResult = this.Tool.RequestAsJsonAsync<TRequest>(
                httpMethod,
                requestUri,
                requestObject,
                requestHeaders,
                this.UserName,
                requestLabel);

            return httpResult;
        }

        public Task<TResponse?> RequestAsJson<TResponse>(
            HttpMethod httpMethod,
            string requestUri,
            string requestLabel = "",
            Dictionary<string, string>? requestHeaders = null)
            where TResponse : class, new()
        {
            return this.Tool.RequestAsJsonAsync<TResponse>(
                httpMethod,
                requestUri,
                requestHeaders,
                this.UserName,
                requestLabel);
        }

        //
        public Task<TResponse?> GetAsJson<TResponse>(
            string path,
            string requestLabel = "",
            Dictionary<string, string>? requestHeaders = null)
            where TResponse : class, new()
        {
            return this.Tool.GetAsJsonAsync<TResponse>(
                path: path,
                requestHeaders: requestHeaders,
                userName: this.UserName,
                requestLabel: requestLabel);
        }

        public Task<TResponse?> GetAsJson<TResponse, TRequest>(
            string path,
            TRequest requestObject,
            string requestLabel = "",
            Dictionary<string, string>? requestHeaders = null)
            where TResponse : class, new()
            where TRequest : class, new()
        {
            return this.Tool.GetAsJsonAsync<TResponse, TRequest>(
                path: path,
                requestObject: requestObject,
                requestHeaders: requestHeaders,
                userName: this.UserName,
                requestLabel: requestLabel);
        }

        public Task<int> GetAsJson<TRequest>(
            string path,
            TRequest requestObject,
            string requestLabel = "",
            Dictionary<string, string>? requestHeaders = null)
            where TRequest : class, new()
        {
            return this.Tool.GetAsJsonAsync<TRequest>(
                path: path,
                requestObject: requestObject,
                requestHeaders: requestHeaders,
                userName: this.UserName,
                requestLabel: requestLabel);
        }

        //
        public Task<int> PostAsJson<TRequest>(
            string path,
            TRequest requestObject,
            string requestLabel = "",
            Dictionary<string, string>? requestHeaders = null)
            where TRequest : class, new()
        {
            return this.Tool.PostAsJsonAsync<TRequest>(
                path: path,
                requestObject: requestObject,
                requestHeaders: requestHeaders,
                userName: this.UserName,
                requestLabel: requestLabel);
        }

        public Task<TResponse?> PostAsJson<TResponse, TRequest>(
            string path,
            TRequest requestObject,
            string requestLabel = "",
            Dictionary<string, string>? requestHeaders = null)
            where TResponse : class, new()
            where TRequest : class, new()
        {
            return this.Tool.PostAsJsonAsync<TResponse, TRequest>(
                path: path,
                requestObject: requestObject,
                requestHeaders: requestHeaders,
                userName: this.UserName,
                requestLabel: requestLabel);
        }

        public Task<TResponse?> PostAsJson<TResponse>(
            string path,
            string requestLabel = "",
            Dictionary<string, string>? requestHeaders = null)
            where TResponse : class, new()
        {
            return this.Tool.PostAsJsonAsync<TResponse>(
                path: path,
                requestHeaders: requestHeaders,
                userName: this.UserName,
                requestLabel: requestLabel);
        }

        //
        public Task<int> PutAsJson<TRequest>(
            string path,
            TRequest requestObject,
            string requestLabel = "",
            Dictionary<string, string>? requestHeaders = null)
            where TRequest : class, new()
        {
            return this.Tool.PutAsJsonAsync<TRequest>(
                path: path,
                requestObject: requestObject,
                requestHeaders: requestHeaders,
                userName: this.UserName,
                requestLabel: requestLabel);
        }

        public Task<TResponse?> PutAsJson<TResponse, TRequest>(
            string path,
            TRequest requestObject,
            string requestLabel = "",
            Dictionary<string, string>? requestHeaders = null)
            where TResponse : class, new()
            where TRequest : class, new()
        {
            return this.Tool.PutAsJsonAsync<TResponse, TRequest>(
                path: path,
                requestObject: requestObject,
                requestHeaders: requestHeaders,
                userName: this.UserName,
                requestLabel: requestLabel);
        }

        public Task<TResponse?> PutAsJson<TResponse>(
            string path,
            string requestLabel = "",
            Dictionary<string, string>? requestHeaders = null)
            where TResponse : class, new()
        {
            return this.Tool.PutAsJsonAsync<TResponse>(
                path: path,
                requestHeaders: requestHeaders,
                userName: this.UserName,
                requestLabel: requestLabel);
        }

        //
        public Task<int> DeleteAsJson<TRequest>(
            string path,
            TRequest requestObject,
            string requestLabel = "",
            Dictionary<string, string>? requestHeaders = null)
            where TRequest : class, new()
        {
            return this.Tool.DeleteAsJsonAsync<TRequest>(
                path: path,
                requestObject: requestObject,
                requestHeaders: requestHeaders,
                userName: this.UserName,
                requestLabel: requestLabel);
        }

        public Task<TResponse?> DeleteAsJson<TResponse, TRequest>(
            string path,
            TRequest requestObject,
            string requestLabel = "",
            Dictionary<string, string>? requestHeaders = null)
            where TResponse : class, new()
            where TRequest : class, new()
        {
            return this.Tool.DeleteAsJsonAsync<TResponse, TRequest>(
                path: path,
                requestObject: requestObject,
                requestHeaders: requestHeaders,
                userName: this.UserName,
                requestLabel: requestLabel);
        }

        public Task<TResponse?> DeleteAsJson<TResponse>(
            string path,
            string requestLabel = "",
            Dictionary<string, string>? requestHeaders = null)
            where TResponse : class, new()
        {
            return this.Tool.DeleteAsJsonAsync<TResponse>(
                path: path,
                requestHeaders: requestHeaders,
                userName: this.UserName,
                requestLabel: requestLabel);
        }
    }
}
