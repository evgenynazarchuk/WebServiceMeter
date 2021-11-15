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

namespace WebSocketWebApplication.Services;
public abstract class WebSocketHandler : IWebSocketHandler
{
    protected readonly IConnectionHandler connectionHandler;

    public WebSocketHandler(IConnectionHandler connectionHandler)
    {
        this.connectionHandler = connectionHandler;
    }

    public virtual Task OnConnectedAsync(WebSocket socket)
    {
        this.connectionHandler.AddSocket(socket);
        return Task.CompletedTask;
    }

    public virtual Task OnDisconnectedAsync(WebSocket socket)
    {
        var socketId = this.connectionHandler.GetSocketId(socket);
        return this.connectionHandler.RemoveSocketAsync(socketId);
    }

    public Task SendMessageAsync(WebSocket socket, string message)
    {
        return socket.SendAsync(
            buffer: new ArraySegment<byte>(
                array: Encoding.UTF8.GetBytes(message),
                offset: 0,
                count: message.Length),
            messageType: WebSocketMessageType.Text,
            endOfMessage: true,
            cancellationToken: CancellationToken.None);
    }

    public async Task SendMessageToAllAsync(string message)
    {
        foreach (var toWebSocket in this.connectionHandler.GetAllSocket())
        {
            if (toWebSocket.State == WebSocketState.Open)
            {
                try
                {
                    await this.SendMessageAsync(toWebSocket, message);
                }
                catch
                {
                    Console.WriteLine($"Error send message, to {toWebSocket.State.ToString()}");
                }
            }
        }
    }

    public abstract Task ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, byte[] buffer);
}
