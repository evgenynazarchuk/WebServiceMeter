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

using System.Text.Json;
using ServiceMeter.DataReader;

namespace ServiceMeter;

public sealed class JsonReader<TData> : DataReader<TData>
        where TData : class
{
    public JsonReader(string filePath, bool cyclicalData = false, JsonSerializerOptions? options = null)
        : base(filePath, cyclicalData)
    {
        this._jsonOptions = options ?? new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };

        string? line;
        while ((line = this.reader.ReadLine()) != null)
        {
            var data = JsonSerializer.Deserialize<TData?>(line, this._jsonOptions);

            if (data is null)
            {
                continue;
            }

            this.queue.Enqueue(data);
        }
    }

    private readonly JsonSerializerOptions _jsonOptions;
}
