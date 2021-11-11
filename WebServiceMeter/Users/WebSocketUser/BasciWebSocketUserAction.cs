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
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace WebServiceMeter.Users;

public abstract partial class BasicWebSocketUser : BasicUser
{
    public ValueTask SendMessage(
        WebSocketTool client,
        string message,
        string label = "")
    {
        return client.SendMessageAsync(message, this.UserName, label);
    }

    public ValueTask<string> ReceiveMessage(
        WebSocketTool client,
        string label = "")
    {
        return client.ReceiveMessageAsync(this.UserName, label);
    }

    public async ValueTask<List<string>> ReceiveMessage(
        WebSocketTool client,
        int readMilliseconds,
        string label = "")
    {
        var messages = new List<string>();

        var currentTime = DateTime.UtcNow;
        var endTime = currentTime.AddMilliseconds(readMilliseconds);
        while (currentTime < endTime)
        {
            var message = await client.ReceiveMessageAsync(this.UserName, label);
            messages.Add(message);
        }

        return messages;
    }

    public async ValueTask<List<string>> ReceiveMessage(
        WebSocketTool client,
        int messageCount,
        int readMilliseconds,
        string label = "")
    {
        var messages = new List<string>();

        var currentTime = DateTime.UtcNow;
        var endTime = currentTime.AddMilliseconds(readMilliseconds);

        while (messages.Count != messageCount && currentTime < endTime)
        {
            var message = await client.ReceiveMessageAsync(this.UserName, label);
            messages.Add(message);
        }

        return messages;
    }

    public ValueTask<ValueWebSocketReceiveResult> Receive(
        WebSocketTool client,
        Memory<byte> buffer,
        string label = "")
    {
        return client.ReceiveAsync(buffer, this.UserName, label);
    }

    public ValueTask<(Memory<byte> buffer, ValueWebSocketReceiveResult bufferInfo)> ReceiveBytes(
        WebSocketTool client,
        string label = "")
    {
        return client.ReceiveBytesAsync(this.UserName, label);
    }

    public ValueTask Send(
        WebSocketTool client,
        ReadOnlyMemory<byte> buffer,
        WebSocketMessageType messageType,
        bool endOfMessage = true,
        string label = "")
    {
        return client.SendAsync(buffer, messageType, endOfMessage, this.UserName, label);
    }

    public ValueTask SendBytes(
        WebSocketTool client,
        ReadOnlyMemory<byte> buffer,
        string label = "")
    {
        return client.SendBytesAsync(buffer, this.UserName, label);
    }
}
