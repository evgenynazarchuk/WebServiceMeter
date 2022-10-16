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

namespace ServiceMeter.HttpService.Models;

public class HttpLogMessage
{
    public string UserName { get; set; }

    public string RequestLabel { get; set; }
    
    public string RequestMethod { get; set; } = string.Empty;

    public string RequestUri { get; set; } = string.Empty;

    public int StatusCode { get; set; }

    /* Время начало запроса */
    public long StartSendRequestTime { get; set; }

    /* Время начала ожидания данных */
    public long StartWaitResponseTime { get; set; }

    /* Время начала получения данных */
    public long StartResponseTime { get; set; }

    /* Время окончания запроса */
    public long EndResponseTime { get; set; }

    public long SendBytes { get; set; }

    public int ReceiveBytes { get; set; }
}
