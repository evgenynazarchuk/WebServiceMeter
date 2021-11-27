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

namespace ServiceMeter.Users;

public abstract partial class BasicWebSocketUser : BasicUser
{
    public BasicWebSocketUser(
        string host,
        int port,
        string path,
        string? userName = null)
        : base(userName ?? typeof(BasicWebSocketUser).Name)
    {
        this.host = host;
        this.port = port;
        this.path = path;
    }

    public void SetClientBuffer(
        int receiveBufferSize = 1024,
        int sendBufferSize = 1024)
    {
        this.sendBufferSize = sendBufferSize;
        this.receiveBufferSize = receiveBufferSize;
    }

    protected readonly string host;

    protected readonly int port;

    protected readonly string path;

    protected int receiveBufferSize = 1024;

    protected int sendBufferSize = 1024;
}
