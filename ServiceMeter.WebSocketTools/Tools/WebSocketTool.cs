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
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ServiceMeter.HttpTools.Tools.HttpTool.Reports;
using ServiceMeter.HttpTools.Tools.HttpTool.Support;
using ServiceMeter.HttpTools.Tools.HttpTool.Tools;

namespace ServiceMeter.HttpTools.Tools.HttpTool;

public class WebSocketTool : Tool, IAsyncDisposable
{
    public readonly ClientWebSocket ClientWebSocket;

    public readonly Uri Uri;

    public readonly int ReceiveBufferSize;

    public readonly int SendBufferSize;

    public WebSocketTool(
        string host,
        int port,
        string path,
        Watcher watcher,
        int receiveBufferSize = 1024,
        int sendBufferSize = 1024)
        : base(watcher)
    {
        this.ClientWebSocket = new ClientWebSocket();
        this.Uri = new UriBuilder()
        {
            Scheme = "ws",
            Host = host,
            Port = port,
            Path = path
        }.Uri;

        this.ReceiveBufferSize = receiveBufferSize;
        this.SendBufferSize = sendBufferSize;
    }

    public async ValueTask ConnectAsync(string userName = "")
    {
        long startConnect;
        long endConnect;

        startConnect = ScenarioTimer.Time.Elapsed.Ticks;
        await this.ClientWebSocket.ConnectAsync(this.Uri, CancellationToken.None);
        endConnect = ScenarioTimer.Time.Elapsed.Ticks;

        if (this.Watcher is not null)
        {
            this.Watcher.SendMessage(
                "WebSocketLogMessage.json",
                $"{userName},,connect,{startConnect},{endConnect}",
                typeof(WebSocketLogMessage));
        }
    }

    public async ValueTask DisconnectAsync(string userName = "")
    {

        if (this.ClientWebSocket.State == WebSocketState.Open)
        {
            long startConnect;
            long endConnect;

            startConnect = ScenarioTimer.Time.Elapsed.Ticks;
            await this.ClientWebSocket.CloseAsync(
                WebSocketCloseStatus.NormalClosure,
                "Close from web socket client tool",
                CancellationToken.None);
            endConnect = ScenarioTimer.Time.Elapsed.Ticks;

            if (this.Watcher is not null)
            {
                this.Watcher.SendMessage(
                    "WebSocketLogMessage.json",
                    $"{userName},,disconnect,{startConnect},{endConnect}",
                    typeof(WebSocketLogMessage));
            }
        }
    }

    // base
    public async ValueTask SendAsync(
        ReadOnlyMemory<byte> buffer,
        WebSocketMessageType messageType = WebSocketMessageType.Binary,
        bool endOfMessage = true,
        string userName = "",
        string label = "")
    {
        long startRequest;
        long finishRequest;

        startRequest = ScenarioTimer.Time.Elapsed.Ticks;
        await this.ClientWebSocket.SendAsync(buffer, messageType, endOfMessage, CancellationToken.None);
        finishRequest = ScenarioTimer.Time.Elapsed.Ticks;

        if (this.Watcher is not null)
        {
            this.Watcher.SendMessage(
                "WebSocketLogMessage.json",
                $"{userName},{label},send,{startRequest},{finishRequest}",
                typeof(WebSocketLogMessage));
        }
    }

    public async ValueTask<ValueWebSocketReceiveResult> ReceiveAsync(
        Memory<byte> buffer,
        string userName = "",
        string label = "")
    {
        long startReceive;
        long endReceive;

        startReceive = ScenarioTimer.Time.Elapsed.Ticks;
        var result = await this.ClientWebSocket.ReceiveAsync(buffer, CancellationToken.None);
        endReceive = ScenarioTimer.Time.Elapsed.Ticks;

        if (this.Watcher is not null)
        {
            this.Watcher.SendMessage(
                $"WebSocketLogMessage.json",
                $"{userName},{label},receive,{startReceive},{endReceive}",
                typeof(WebSocketLogMessage));
        }

        return result;
    }

    // message
    public async ValueTask SendMessageAsync(string message, string userName = "", string label = "")
    {
        var buffer = new ReadOnlyMemory<byte>(
            array: Encoding.UTF8.GetBytes(message),
            start: 0,
            length: message.Length);

        await this.SendAsync(
            buffer: buffer,
            messageType: WebSocketMessageType.Text,
            userName: userName,
            label: label);
    }

    public async ValueTask<string> ReceiveMessageAsync(string userName = "", string label = "")
    {
        var buffer = WebSocket.CreateClientBuffer(1024, 1024);
        var result = await this.ReceiveAsync(buffer, userName, label);
        var message = Encoding.UTF8.GetString(buffer.ToArray(), 0, result.Count);

        return message;
    }


    // bytes
    public async ValueTask SendBytesAsync(
        ReadOnlyMemory<byte> buffer,
        string userName = "",
        string label = "")
    {
        await this.SendAsync(
            buffer: buffer,
            messageType: WebSocketMessageType.Binary,
            userName: userName,
            label: label);
    }

    public async ValueTask<(Memory<byte> buffer, ValueWebSocketReceiveResult bufferInfo)> ReceiveBytesAsync(
        string userName = "",
        string label = "")
    {
        var buffer = WebSocket.CreateClientBuffer(this.ReceiveBufferSize, this.SendBufferSize);
        var result = await this.ReceiveAsync(buffer, userName, label);

        return (buffer, result);
    }

    // dispose
    public async ValueTask DisposeAsync()
    {
        await this.DisconnectAsync();
        this.ClientWebSocket.Dispose();
    }
}
