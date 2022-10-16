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

using System.Text;
using System.Text.Json;
using System.Collections.Generic;
using System;
using System.IO;

namespace ServiceMeter.LogsServices;

public abstract class HtmlGenerator<TLog>
        where TLog : class
{
    private readonly StreamReader _reader;

    private readonly StreamWriter _writer;
    
    protected HtmlGenerator(
        string sourceJsonFile,
        string resultHtmlFile)
    {
        this._reader = new StreamReader(sourceJsonFile, Encoding.UTF8, false, 65535);
        this._writer = new StreamWriter(resultHtmlFile, false, Encoding.UTF8, 65355);
    }

    public void BuildHtml()
    {
        var logMessageObjects = this.ReadLogObjectsFromFile();
        
        var html = this.GenerateHtml(logMessageObjects);
        
        this.SaveHtml(html);
    }

    protected abstract string GenerateHtml(List<TLog> logMessageObjects);

    private void SaveHtml(string html)
    {
        this._writer.WriteLine(html);
        
        this._writer.Flush();
        
        this._writer.Close();
    }

    private List<TLog> ReadLogObjectsFromFile()
    {
        List<TLog> logs = new();
        
        while (true)
        {
            var logMessageJson = this._reader.ReadLine();
            
            if (logMessageJson is null) break;
            
            var logMessageObject = JsonSerializer.Deserialize<TLog>(logMessageJson);

            if (logMessageObject is null)
            {
                throw new ApplicationException("Error convert");
            }

            logs.Add(logMessageObject);
        }

        this._reader.Close();

        return logs;
    }
}
