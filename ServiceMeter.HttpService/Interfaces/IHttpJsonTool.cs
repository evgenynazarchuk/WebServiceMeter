﻿/*
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

using ServiceMeter.Interfaces;

namespace ServiceMeter.HttpService.Interfaces;

public interface IHttpJsonTool : ITool
{
    Task<TResponseObject?> RequestAsJsonAsync<TResponseObject, TRequestObject>(
        HttpMethod httpMethod,
        string requestUri,
        TRequestObject requestObject,
        Dictionary<string, string>? requestHeaders = null,
        string userName = "",
        string requestLabel = "")
        where TResponseObject : class, new()
        where TRequestObject : class, new();


    Task<string> RequestAsJsonAsync<TRequestObject>(
        HttpMethod httpMethod,
        string requestUri,
        TRequestObject requestObject,
        Dictionary<string, string>? requestHeaders = null,
        string userName = "",
        string requestLabel = "")
        where TRequestObject : class, new();

    Task<TResponseObject?> RequestAsJsonAsync<TResponseObject>(
        HttpMethod httpMethod,
        string requestUri,
        Dictionary<string, string>? requestHeaders = null,
        string user = "",
        string requestLabel = "")
        where TResponseObject : class, new();
}