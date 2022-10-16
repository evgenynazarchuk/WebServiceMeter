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

using GrpcWebApplication.PerformanceTests.Users;
using System.Threading.Tasks;
using ServiceMeter.HttpService.Tools.HttpTool;

namespace GrpcWebApplication.PerformanceTests.Tests;

[PerformanceTestClass]
public class Demo1
{
    private const string ADDRESS = "https://localhost:5001";

    [PerformanceTest(10, 120)]
    public async Task UnaryCallByActiveUsersTest(int activeUsers, int seconds)
    {
        var user = new UnaryGrpcUser(ADDRESS);
        var plan = new ActiveUsersOnPeriod(user, activeUsers, seconds.Seconds());

        await new Scenario()
            .AddSequentialPlans(plan)
            .Start();
    }

    [PerformanceTest(500, 20 * 60)]
    public async Task UnaryCallByUsersPerPeriodTest(int users, int seconds)
    {
        var user = new UnaryGrpcUser(ADDRESS);
        var plan = new UsersPerPeriod(user, users, seconds.Seconds());

        await new Scenario()
            .AddSequentialPlans(plan)
            .Start();
    }

    [PerformanceTest(5, 60)]
    public async Task ClientStreamTest(int users, int seconds)
    {
        var user = new ClientStreamUser(ADDRESS);
        var plan = new ActiveUsersOnPeriod(user, users, seconds.Seconds());

        await new Scenario()
            .AddSequentialPlans(plan)
            .Start();
    }
}
