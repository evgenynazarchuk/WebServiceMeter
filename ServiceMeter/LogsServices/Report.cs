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

using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using ServiceMeter.Interfaces;

namespace ServiceMeter.LogsServices;

public abstract class Report : IReport
{
    protected readonly CancellationToken Token;

    protected readonly ConcurrentQueue<string> LogsQueue;

    protected readonly string LogName;
    
    private readonly CancellationTokenSource _tokenSource;
    
    public abstract Task StartProcessAsync();
    
    protected Report(string logName)
    {
        this.LogName = logName;
        this.LogsQueue =  new ConcurrentQueue<string>();
        this._tokenSource = new CancellationTokenSource();
        this.Token = this._tokenSource.Token;
    }

    public virtual void StopProcess()
    {
        this._tokenSource.Cancel();
    }

    public virtual void SendMessage(string logMessage)
    {
        this.LogsQueue.Enqueue(logMessage);
    }
}