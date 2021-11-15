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
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using WebServiceMeter.DataReader;
using WebServiceMeter.Interfaces;

namespace WebServiceMeter.Reports;

public abstract class Report : IReport
{
    public Report(string projectName, string testRunId)
    {
        this.ProjectName = projectName;
        this.TestRunId = testRunId;
        this.LogQueue = new();
        this.tokenSource = new();
        this.token = this.tokenSource.Token;
        this._lock = new();
        this._processStart = false;
    }

    public Task StartAsync()
    {
        lock (this._lock)
        {
            if (this._processStart is true)
            {
                return Task.CompletedTask;
            }
            else
            {
                _processStart = true;
            }
        }

        return this.ProcessAsync();
    }

    public void Stop()
    {
        this.tokenSource.Cancel();
    }

    public virtual void SendLogMessage(string logName, string logMessage, Type logMessageType)
    {
        //
        //Console.WriteLine($"log message: {logName} {logMessage}");

        this.LogQueue.Enqueue((logName, logMessage, logMessageType));
    }

    protected virtual Task PostProcessingAsync(string logName)
    {
        return Task.CompletedTask;
    }

    // from csv line to object
    protected virtual object? FromCsvLineToObject(string logMessageCsvLine, Type logMessageType)
    {
        var logMessageObject = CsvConverter.GetObjectFromCsvLine(logMessageCsvLine, logMessageType);

        return logMessageObject;
    }

    // from csv line to json string
    protected virtual string FromCsvLineToJsonString(string logMessageCsvLine, Type logMessageType)
    {
        var obj = this.FromCsvLineToObject(logMessageCsvLine, logMessageType);
        var jsonString = this.FromObjectToJsonString(obj, logMessageType);

        return jsonString;
    }

    // from object to json string
    protected virtual string FromObjectToJsonString(object? logMessageObject, Type logMessageType)
    {
        var jsonString = JsonSerializer.Serialize(logMessageObject, logMessageType);
        return jsonString;
    }

    protected abstract Task ProcessAsync();

    public readonly string ProjectName;

    public readonly string TestRunId;

    protected readonly CancellationToken token;

    protected readonly CancellationTokenSource tokenSource;

    protected readonly ConcurrentQueue<(string logName, string logMessage, Type logType)> LogQueue;

    private readonly object _lock;

    private bool _processStart;
}
