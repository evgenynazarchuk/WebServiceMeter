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
using System.Threading.Tasks;
using ServiceMeter.Interfaces;

namespace ServiceMeter;

public class Watcher
{
    protected readonly List<IReport> reports;

    public Watcher()
    {
        this.reports = new List<IReport>();
    }

    public void AddReport(IReport report)
    {
        this.reports.Add(report);
    }

    public void ClearReports()
    {
        this.reports.Clear();
    }

    public void SendMessage(string logName, string logMessage, Type logMessageType)
    {
        foreach (var logger in this.reports)
        {
            logger.SendLogMessage(logName, logMessage, logMessageType);
        }
    }

    public List<Task> StartAsync()
    {
        var tasks = new List<Task>();

        foreach (var report in this.reports)
        {
            tasks.Add(report.StartAsync());
        }

        return tasks;
    }

    public void Stop()
    {
        foreach (var report in this.reports)
        {
            report.Stop();
        }
    }
}
