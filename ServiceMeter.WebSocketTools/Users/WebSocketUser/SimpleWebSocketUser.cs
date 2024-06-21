﻿/*
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

using System.Threading.Tasks;
using ServiceMeter.HttpTools.Tools.HttpTool.Interfaces;
using ServiceMeter.HttpTools.Tools.HttpTool.Users;

namespace ServiceMeter.HttpTools.Tools.HttpTool;

public abstract partial class WebSocketUser : BasicWebSocketUser, ISimpleUser
{
    public WebSocketUser(
        string host,
        int port,
        string path,
        string? userName = null)
        : base(host, port, path, userName ?? typeof(WebSocketUser).Name) { }

    public async Task InvokeAsync(int userLoopCount = 1)
    {
        var client = new WebSocketTool(
            this.host,
            this.port,
            this.path,
            this.Watcher,
            this.sendBufferSize,
            this.receiveBufferSize);

        for (int i = 0; i < userLoopCount; i++)
        {
            await client.ConnectAsync(this.UserName);
            await PerformanceAsync(client);
            await client.DisconnectAsync(this.UserName);
        }
    }

    protected abstract Task PerformanceAsync(WebSocketTool client);
}