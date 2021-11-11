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

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebServiceMeter.Users;

public abstract partial class BasicHttpUser : BasicUser
{
    public HttpTool Tool { get; set; }

    public HttpClient Client { get; set; }

    public BasicHttpUser(HttpClient client, string userName)
        : base(userName)
    {
        this.Client = client;
        this.Tool = new HttpTool(client, this.Watcher);
    }

    public BasicHttpUser(
        string address,
        string userName,
        IDictionary<string, string>? defaultHeaders = null,
        IEnumerable<Cookie>? defaultCookies = null)
        : base(userName)
    {
        this.Client = new HttpClient() { BaseAddress = new Uri(address) };
        this.Tool = new HttpTool(address, this.Watcher, defaultHeaders, defaultCookies);
    }

    public async Task Parallel(params Task[] actions)
    {
        foreach (var action in actions)
        {
            if (action is not null)
            {
                await action;
            }
        }
    }
}
