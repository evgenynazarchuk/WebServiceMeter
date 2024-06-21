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

using System.Net;
using System.Net.Http.Headers;
using ServiceMeter.Support;

namespace ServiceMeter.HttpService.Users;

public abstract partial class BasicHttpUser : User
{
    protected async Task<HttpStatusCode> UploadFile(
        string path,
        string fileName,
        byte[] file,
        Dictionary<string, string>? requestHeaders = null,
        string httpFileParameter = "file",
        string requestLabel = "")
    {
        using var form = new MultipartFormDataContent();
        using var fileContent = new ByteArrayContent(file);

        fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/octet-stream");
        form.Add(fileContent, httpFileParameter, fileName);

        var response = await this._httpTool.RequestAsync(
            httpMethod: HttpMethod.Post,
            path: path,
            requestHeaders: requestHeaders,
            requestContent: form,
            requestLabel: requestLabel);

        return (HttpStatusCode)response.StatusCode;
    }

    protected async Task<HttpStatusCode> UploadFiles(
        string path,
        IEnumerable<(string, byte[])> files,
        Dictionary<string, string>? requestHeaders = null,
        string httpFileParameter = "files",
        string requestLabel = "")
    {
        var form = new MultipartFormDataContent();

        foreach (var (fileName, fileBytes) in files)
        {
            var fileContent = new ByteArrayContent(fileBytes);
            
            fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data");
            
            fileContent.Headers.ContentDisposition.Name = httpFileParameter;
            
            fileContent.Headers.ContentDisposition.FileName = fileName;
            
            fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            
            form.Add(fileContent);
        }

        var response = await this._httpTool.RequestAsync(
            httpMethod: HttpMethod.Post,
            path: path,
            requestHeaders: requestHeaders,
            requestContent: form,
            requestLabel: requestLabel);

        return (HttpStatusCode)response.StatusCode;
    }

    protected async Task<(string?, byte[])> DownloadFile(
        string path, 
        Dictionary<string, string>? requestHeaders = null,
        string requestLabel = "")
    {
        var response = await this._httpTool.RequestAsync(
            httpMethod: HttpMethod.Get, 
            path: path, 
            requestHeaders: requestHeaders, 
            requestContent: null,
            requestLabel: requestLabel);
        
        var fileName = response.Filename;
        
        var bytes = response.Content;

        return (fileName, bytes);
    }
}
