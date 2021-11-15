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

using Microsoft.AspNetCore.Http;
using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using WebSocketWebApplication.Services;

namespace WebSocketWebApplication.Middleware;

public class WebSocketHandlerMiddleware
{
    private readonly RequestDelegate _next;

    private readonly IWebSocketHandler _webSocketHandler;

    public WebSocketHandlerMiddleware(RequestDelegate next, IWebSocketHandler webSocketHandler)
    {
        this._next = next;
        this._webSocketHandler = webSocketHandler;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.WebSockets.IsWebSocketRequest)
        {
            return;
        }

        using var socket = await context.WebSockets.AcceptWebSocketAsync();
        await this._webSocketHandler.OnConnectedAsync(socket);

        var buffer = new byte[1024];

        while (socket.State == WebSocketState.Open)
        {
            // ожидаем сообщение от клиента
            var result = await socket.ReceiveAsync(
                buffer: new ArraySegment<byte>(buffer),
                cancellationToken: CancellationToken.None);

            if (result.MessageType == WebSocketMessageType.Text)
            {
                // отправить всем клиентам полученное сообщение
                await _webSocketHandler.ReceiveAsync(socket, result, buffer);
            }
            else if (result.MessageType == WebSocketMessageType.Close)
            {
                // закрыть соединение с клиентом
                await _webSocketHandler.OnDisconnectedAsync(socket);
            }
        }

        // TODO
        //await this._next(context);
    }
}