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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace WebSocketWebApplication.Services;

public class ConnectionHandler : IConnectionHandler
{
    private readonly ConcurrentDictionary<Guid, WebSocket> _sockets;

    public ConnectionHandler()
    {
        this._sockets = new();
    }

    public IEnumerable<WebSocket> GetAllSocket()
    {
        return this._sockets.Values;
    }

    public Guid GetSocketId(WebSocket socket)
    {
        return this._sockets.FirstOrDefault(x => x.Value == socket).Key;
    }

    public WebSocket GetSocketById(Guid id)
    {
        return this._sockets.FirstOrDefault(x => x.Key == id).Value;
    }

    public void AddSocket(WebSocket socket)
    {
        this._sockets.TryAdd(Guid.NewGuid(), socket);
    }

    public Task RemoveSocketAsync(Guid id)
    {
        if (this._sockets.TryRemove(id, out WebSocket socket))
        {
            return socket.CloseAsync(closeStatus: WebSocketCloseStatus.NormalClosure,
                                statusDescription: "Closed by the Connection Manager",
                                cancellationToken: CancellationToken.None);
        }

        return Task.CompletedTask;
    }
}