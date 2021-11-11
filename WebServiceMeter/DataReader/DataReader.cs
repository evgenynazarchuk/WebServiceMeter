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
using System.IO;
using System.Text;
using WebServiceMeter.Interfaces;

namespace WebServiceMeter.DataReader;

public abstract class DataReader<TData> : IDataReader<TData>
        where TData : class
{
    public DataReader(string filePath, bool cyclicalData = false)
    {
        this.reader = new StreamReader(filePath, Encoding.UTF8, true, 65535);
        this.queue = new();
        this.cyclicalData = cyclicalData;
    }

    public TData? GetData()
    {
        if (this.queue is null)
        {
            return null;
        }

        this.queue.TryDequeue(out TData? result);

        // put again
        if (this.cyclicalData && result is not null)
        {
            this.queue.Enqueue(result);
        }

        return result;
    }

    protected readonly bool cyclicalData;

    protected readonly StreamReader reader;

    protected readonly ConcurrentQueue<TData> queue;
}
