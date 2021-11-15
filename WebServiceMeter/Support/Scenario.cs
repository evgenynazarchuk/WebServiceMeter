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

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebServiceMeter.Reports;
using WebServiceMeter.Users;
using WebServiceMeter.Support;

namespace WebServiceMeter;

public sealed class Scenario
{
    public Scenario(string projectName = "", string testRunId = "")
    {
        this._projectName = projectName;
        this._testRunId = testRunId;
        this._acts = new();
        this._reports = new();
    }

    public Scenario AddParallelPlans(params UsersPerformancePlan[] performancePlan)
    {
        this.AddActs(ActType.Parallel, performancePlan);

        return this;
    }

    public Scenario AddSequentialPlans(params UsersPerformancePlan[] performancePlan)
    {
        this.AddActs(ActType.Sequential, performancePlan);

        return this;
    }

    public async Task Start()
    {
        this.StartWatcher();

        ScenarioTimer.Time.Start();

        foreach (var (launchType, plans) in this._acts)
        {
            switch (launchType)
            {
                case ActType.Parallel:

                    var tasks = new List<Task>();

                    foreach (var plan in plans)
                    {
                        tasks.Add(Task.Run(async () =>
                        {
                            await plan.StartAsync();
                        }));
                    }

                    await Task.WhenAll(tasks.ToArray());

                    break;

                case ActType.Sequential:

                    foreach (var plan in plans)
                    {
                        await plan.StartAsync();
                    }

                    break;
            }
        }

        ScenarioTimer.Time.Stop();

        await this.StopAndWaitWatcher();
    }

    private Scenario AddActs(ActType launchType, params UsersPerformancePlan[] performancePlan)
    {
        this._acts.Add(new(launchType, performancePlan));

        return this;
    }

    private async Task StopAndWaitWatcher()
    {
        Console.WriteLine($"Info: Stop Reports");

        foreach (var (_, plans) in this._acts)
        {
            foreach (var plan in plans)
            {
                plan.User.Watcher.Stop();
            }
        }

        Console.WriteLine($"Info: Wait Reports");

        await Task.WhenAll(this._reports.ToArray());
    }

    private void InitDefaultReport()
    {
        foreach (var (_, plans) in this._acts)
        {
            foreach (var plan in plans)
            {
                if (plan.User is BasicHttpUser)
                    plan.User.Watcher.AddReport(HttpReportFileSingleton.GetInstance(this._projectName, this._testRunId));

                if (plan.User is BasicGrpcUser)
                    plan.User.Watcher.AddReport(GrpcReportFileSingleton.GetInstance(this._projectName, this._testRunId));

                if (plan.User is BasicWebSocketUser)
                    plan.User.Watcher.AddReport(WebSocketReportFileSingleton.GetInstance(this._projectName, this._testRunId));

                //if (plan.User is BasicChromiumUser)
                //    plan.User.Watcher.AddReport(ChromiumReportFileSingleton.GetInstance(this._projectName, this._testRunId));
            }
        }
    }

    private void StartWatcher()
    {
        this.InitDefaultReport();

        foreach (var (_, plans) in this._acts)
        {
            foreach (var plan in plans)
            {
                Console.WriteLine($"Info: Start Reports");

                var task = plan.User.Watcher.StartAsync();
                this._reports.AddRange(task);
            }
        }
    }

    private readonly string _projectName;

    private readonly string _testRunId;

    private readonly List<KeyValuePair<ActType, UsersPerformancePlan[]>> _acts;

    private readonly List<Task> _reports;
}
