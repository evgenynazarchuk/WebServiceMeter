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
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ServiceMeter.Reports;

public abstract class ReportFile : Report
{
    public ReportFile(string projectName, string testRunId)
        : base(projectName, testRunId)
    {
        this.writers = new();

        ////long reportNumber = DateTime.UtcNow.Ticks;
        ////string targetFolder = $"Logs//{reportNumber}";
        ////
        ////if (!Directory.Exists("Logs"))
        ////{
        ////    Directory.CreateDirectory("Logs");
        ////}
        ////
        ////if (!Directory.Exists(targetFolder))
        ////{
        ////    Directory.CreateDirectory(targetFolder);
        ////}
    }

    protected readonly ConcurrentDictionary<string, StreamWriter> writers;

    protected override Task ProcessAsync()
    {
        var task = Task.Run(async () =>
        {
            while (true)
            {
                if (this.LogQueue.IsEmpty && this.token.IsCancellationRequested)
                {
                    await this.FinishAsync();

                    break;
                }

                if (this.LogQueue.TryDequeue(out (string logName, string logMessage, Type logMessageType) log))
                {
                    if (!this.writers.TryGetValue(log.logName, out StreamWriter? logWriter))
                    {
                        if (logWriter is null)
                        {
                            logWriter = new StreamWriter(log.logName, false, Encoding.UTF8, 65535);
                        }

                        this.writers.TryAdd(log.logName, logWriter);
                    }
                    else
                    {
                        var jsonLogMessage = this.FromCsvLineToJsonString(log.logMessage, log.logMessageType);

                        //
                        //Console.WriteLine(jsonLogMessage);

                        logWriter.WriteLine(jsonLogMessage);
                    }
                }
            }
        });

        return task;
    }

    private async Task FinishAsync()
    {
        foreach ((var logName, var fileWriter) in this.writers)
        {
            fileWriter.Flush();
            fileWriter.Close();

            await this.PostProcessingAsync(logName);
        }
    }
}
