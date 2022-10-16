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

using System.Threading.Tasks;
using ServiceMeter.HttpService.Tools.HttpTool;

namespace WebSocketWebApplication.PerformanceTest.Tests;

[PerformanceTestClass]
public class DelaySendMessageTests
{
    [PerformanceTest(10, 200)]
    public async Task SendMessageHelloWorldTest(int seconds, int usersCountPerPeriod)
    {
        var user = new WebSocketUserTest("localhost", 5000, "ws");
        var plan = new UsersPerPeriod(user, usersCountPerPeriod, seconds.Seconds());

        await new Scenario()
            .AddSequentialPlans(plan)
            .Start();
    }

    [PerformanceTest(10, 200)]
    public async Task ActiveUsersSendMessageHelloWorldTest(int seconds, int activeUsersCount)
    {
        var user = new WebSocketUserTest("localhost", 5000, "ws");
        var plan = new ActiveUsersOnPeriod(user, activeUsersCount, seconds.Seconds());

        await new Scenario()
            .AddSequentialPlans(plan)
            .Start();
    }

    public class WebSocketUserTest : WebSocketUser
    {
        public WebSocketUserTest(string host, int port, string path) : base(host, port, path) { }

        protected override async Task PerformanceAsync(WebSocketTool client)
        {
            await Task.Delay(500);
            await SendMessage(client, "Hello world");
        }
    }
}