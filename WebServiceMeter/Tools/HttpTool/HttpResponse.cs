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

namespace WebServiceMeter;

public sealed class HttpResponse
{
    public HttpResponse(
        int statusCode,
        byte[] content,
        string? filename)
    {
        this.StatusCode = statusCode;
        this.Content = content;
        this.Filename = filename;
    }

    public readonly int StatusCode;

    public readonly byte[] Content;

    public readonly string? Filename = null;

    public string ContentAsUTF8 => Encoding.UTF8.GetString(this.Content);

    public string ContentAsASCII => Encoding.ASCII.GetString(this.Content);

    public string ContentAsUnicode => Encoding.Unicode.GetString(this.Content);

    public string ContentAsUTF32 => Encoding.UTF32.GetString(this.Content);
}
