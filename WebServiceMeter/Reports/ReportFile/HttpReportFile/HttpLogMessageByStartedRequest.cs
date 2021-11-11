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

namespace WebServiceMeter.Reports;

public class HttpLogMessageByStartedRequest
{
    public string? UserName { get; set; }

    public string? RequestMethod { get; set; }

    public string? Request { get; set; }

    public string? RequestLabel { get; set; }

    public int StatusCode { get; set; }

    public long StartRequestTime { get; set; }

    public long CountStartedRequest { get; set; }

    public HttpLogMessageByStartedRequest(
        string? userName,
        string? requestMethod,
        string? request,
        string? requestLabel,
        int statusCode,
        long startRequestTime,
        long countStartedRequest
        )
    {
        this.UserName = userName;
        this.RequestMethod = requestMethod;
        this.RequestLabel = requestLabel;
        this.Request = request;
        this.StatusCode = statusCode;
        this.StartRequestTime = startRequestTime;
        this.CountStartedRequest = countStartedRequest;
    }
}
