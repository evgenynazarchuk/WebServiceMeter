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
using System.IO;
using System.Text;
using System.Text.Json;

namespace ServiceMeter.Reports;

public abstract class HtmlBuilder<TLogMessage>
        where TLogMessage : class
{
    public HtmlBuilder(
        string sourceJsonFilePath,
        string destinationHtmlFilePath)
    {
        this.logs = new();
        this._reader = new(sourceJsonFilePath, Encoding.UTF8, false, 65535);
        this._writer = new(destinationHtmlFilePath, false, Encoding.UTF8, 65355);
    }

    public void Build()
    {
        this.ReadJsonObject();
        var htmlReport = this.GenerateHtml();
        this.SaveHtml(htmlReport);
    }

    protected abstract string GenerateHtml();

    private void SaveHtml(string htmlReport)
    {
        _writer.WriteLine(htmlReport);
        _writer.Flush();
        _writer.Close();
    }

    private void ReadJsonObject()
    {
        string? line;
        TLogMessage? httpLogMessage;

        while ((line = this._reader.ReadLine()) != null)
        {
            httpLogMessage = JsonSerializer.Deserialize<TLogMessage>(line);

            if (httpLogMessage is null)
            {
                throw new ApplicationException("Error convertation");
            }

            this.logs.Add(httpLogMessage);
        }

        this._reader.Close();
    }

    protected readonly List<TLogMessage> logs;

    private readonly StreamReader _reader;

    private readonly StreamWriter _writer;
}
