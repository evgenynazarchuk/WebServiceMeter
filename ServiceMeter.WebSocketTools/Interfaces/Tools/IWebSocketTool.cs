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
using System.Threading.Tasks;

namespace ServiceMeter.HttpTools.Tools.HttpTool.Interfaces;

public interface IWebSocketTool : ITool
{
    ValueTask ConnectAsync(string userName = "");

    ValueTask DisconnectAsync(string userName = "");

    ValueTask SendMessageAsync(string message, string userName = "", string label = "");

    ValueTask<string> ReceiveMessageAsync(string userName = "", string label = "");

    ValueTask<ValueWebSocketReceiveResult> ReceiveAsync(Memory<byte> buffer, string userName = "", string label = "");

    ValueTask<(Memory<byte> buffer, ValueWebSocketReceiveResult bufferInfo)> ReceiveBytesAsync(string userName = "", string label = "");

    ValueTask SendAsync(ReadOnlyMemory<byte> buffer, WebSocketMessageType messageType, bool endOfMessage = true, string userName = "", string label = "");

    ValueTask SendBytesAsync(ReadOnlyMemory<byte> buffer, string userName = "", string label = "");
}
