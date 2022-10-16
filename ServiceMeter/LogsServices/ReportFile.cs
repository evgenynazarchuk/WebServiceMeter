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

using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ServiceMeter.LogsServices;

public class ReportFile : Report
{
    private readonly StreamWriter _logWriter;

    public ReportFile(string logName)
        : base(logName)
    {
        this._logWriter = new StreamWriter(this.LogName, false, Encoding.UTF8, 65535);
    }
    
    public override Task StartProcessAsync()
    {
        var processWrite = Task.Run(async () =>
        {
            while (true)
            {
                if (this.Token.IsCancellationRequested && this.LogsQueue.IsEmpty)
                {
                    await this.StopProcessAsync();
                    break;
                }
                
                if (this.LogsQueue.TryDequeue(out var logMessage))
                {
                    await this._logWriter.WriteLineAsync(logMessage);
                }
            }
        });

        return processWrite;
    }

    private async Task StopProcessAsync()
    {
        await this._logWriter.FlushAsync();
        this._logWriter.Close();
    }
}