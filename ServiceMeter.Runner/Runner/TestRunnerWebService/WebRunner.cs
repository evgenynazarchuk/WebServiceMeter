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

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;
using ServiceMeter.HttpTools.Tools.HttpTool.Runner;
using ServiceMeter.Runner.Runner.TestRunnerWebService.DTOs;
using ServiceMeter.Runner.Runner.TestRunnerWebService.Services;

namespace ServiceMeter.Runner.Runner.TestRunnerWebService;

public class WebRunner
{
    public static void Start(Assembly assembly, WebServiceConfigDto config)
    {
        Host.CreateDefaultBuilder()
           .ConfigureWebHostDefaults(webBuilder =>
           {
               webBuilder.UseStartup<TestRunnerStartup>();

               webBuilder.ConfigureServices(services =>
               {
                   services.AddSingleton<TestRunnerService>(x => new TestRunnerService(assembly));
               });

               webBuilder.UseUrls($"http://*:{config.TestRunnerPort}");
           })
           .Build()
           .Run();
    }
}
