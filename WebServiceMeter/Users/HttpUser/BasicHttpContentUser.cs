﻿using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text;

namespace WebServiceMeter.Users
{
    public abstract partial class BasicHttpUser : BasicUser
    {
        public Task<HttpResponse> Get(
            string path,
            Dictionary<string, string>? requestHeaders = null,
            string? requestContent = null,
            Encoding? requestContentEncoding = null,
            string requestLabel = "")
        {
            return this.Tool.GetAsync(
                path: path,
                requestContent: requestContent,
                requestContentEncoding: requestContentEncoding,
                requestHeaders: requestHeaders,
                userName: this.UserName,
                requestLabel: requestLabel);
        }

        public Task<HttpResponse> Post(
            string path,
            Dictionary<string, string>? requestHeaders = null,
            string? requestContent = null,
            Encoding? requestContentEncoding = null,
            string requestLabel = "")
        {
            return this.Tool.PostAsync(
                path: path,
                requestContent: requestContent,
                requestContentEncoding: requestContentEncoding,
                requestHeaders: requestHeaders,
                userName: this.UserName,
                requestLabel: requestLabel);
        }

        public Task<HttpResponse> Put(
            string path,
            Dictionary<string, string>? requestHeaders = null,
            string? requestContent = null,
            Encoding? requestContentEncoding = null,
            string requestLabel = "")
        {
            return this.Tool.PutAsync(
                path: path,
                requestHeaders: requestHeaders,
                requestContent: requestContent,
                requestContentEncoding: requestContentEncoding,
                userName: this.UserName,
                requestLabel: requestLabel);
        }

        public Task<HttpResponse> Delete(
            string path,
            Dictionary<string, string>? requestHeaders = null,
            string? requestContent = null,
            Encoding? requestContentEncoding = null,
            string requestLabel = "")
        {
            return this.Tool.DeleteAsync(
                path: path,
                requestHeaders: requestHeaders,
                requestContent: requestContent,
                requestContentEncoding: requestContentEncoding,
                userName: this.UserName,
                requestLabel: requestLabel);
        }
    }
}