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

using System.Reflection;
using WebServiceMeter.Runner;

namespace WebServiceMeter;

public class TestRunner
{
    private readonly string[] _args;

    private readonly Assembly _assembly;

    public TestRunner(string[] args, Assembly assembly)
    {
        this._args = args;
        this._assembly = assembly;
    }

    public async Task StartAsync()
    {
        if (this._args.Length == 0)
        {
            var runner = new ConsoleRunner(this._assembly);
            await runner.StartAsync();
        }
        else
        {
            int port = 0;
            string? loggerAddress = null;

            for (int i = 0; i < this._args.Length; i++)
            {
                if (this._args[i] == "-p")
                {
                    port = int.Parse(this._args[i + 1]);
                    break;
                }
            }

            for (int i = 0; i < this._args.Length; i++)
            {
                if (this._args[i] == "-l")
                {
                    loggerAddress = this._args[i + 1];
                    break;
                }
            }

            var config = new WebServiceConfigDto
            {
                TestRunnerPort = port,
                LogServiceAddress = loggerAddress
            };

            WebRunner.Start(this._assembly, config);
        }
    }
}
