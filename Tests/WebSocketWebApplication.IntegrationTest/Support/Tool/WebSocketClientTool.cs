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

namespace WebSocketWebApplication.IntegrationTest.Support.Tool;

public class WebSocketClientTool : IWebSocketClientTool, IAsyncDisposable
{
    public readonly ClientWebSocket ClientWebSocket;

    public readonly Uri Uri;

    public readonly int ReceiveBufferSize;

    public readonly int SendBufferSize;

    public WebSocketClientTool(Uri uri, int receiveBufferSize = 1024, int sendBufferSize = 1024)
    {
        this.ClientWebSocket = new ClientWebSocket();
        this.Uri = uri;
        this.ReceiveBufferSize = receiveBufferSize;
        this.SendBufferSize = sendBufferSize;
    }

    public Task ConnectAsync()
    {
        return this.ClientWebSocket.ConnectAsync(this.Uri, CancellationToken.None);
    }

    public async ValueTask DisconnectAsync()
    {
        if (this.ClientWebSocket.State == WebSocketState.Open)
        {
            try
            {
                await this.ClientWebSocket.CloseAsync(
                WebSocketCloseStatus.NormalClosure,
                "Close from web socket client tool",
                CancellationToken.None);
            }
            catch
            {
                return;
            }


            //await this.ClientWebSocket.CloseOutputAsync(
            //    WebSocketCloseStatus.NormalClosure, 
            //    "closing websocket", 
            //    CancellationToken.None);
        }
    }

    // read
    public ValueTask<ValueWebSocketReceiveResult> ReceiveBytesAsync(Memory<byte> buffer)
    {
        return this.ClientWebSocket.ReceiveAsync(buffer, CancellationToken.None);
    }

    public async ValueTask<(Memory<byte> buffer, ValueWebSocketReceiveResult bufferInfo)> ReceiveBytesAsync()
    {
        var buffer = WebSocket.CreateClientBuffer(this.ReceiveBufferSize, this.SendBufferSize);
        var result = await this.ReceiveBytesAsync(buffer);

        return (buffer: buffer, bufferInfo: result);
    }

    public async ValueTask<string> ReceiveMessageAsync()
    {
        var buffer = WebSocket.CreateClientBuffer(1024, 1024);
        var result = await this.ReceiveBytesAsync(buffer);
        var message = Encoding.UTF8.GetString(buffer.ToArray(), 0, result.Count);

        return message;
    }

    // send
    public ValueTask SendAsync(
        ReadOnlyMemory<byte> buffer,
        WebSocketMessageType messageType = WebSocketMessageType.Binary,
        bool endOfMessage = true)
    {
        return this.ClientWebSocket.SendAsync(buffer, messageType, endOfMessage, CancellationToken.None);
    }

    public ValueTask SendMessageAsync(string message)
    {
        var buffer = new ReadOnlyMemory<byte>(
            array: Encoding.UTF8.GetBytes(message),
            start: 0,
            length: message.Length);

        return this.SendAsync(buffer: buffer, messageType: WebSocketMessageType.Text);
    }

    public ValueTask SendBytesAsync(ReadOnlyMemory<byte> buffer)
    {
        return this.SendAsync(buffer: buffer, messageType: WebSocketMessageType.Binary);
    }

    // TODO
    // wait and receive for time
    // wait for n message
    // 

    // dispose
    public async ValueTask DisposeAsync()
    {
        await this.DisconnectAsync();
        this.ClientWebSocket.Dispose();
    }
}