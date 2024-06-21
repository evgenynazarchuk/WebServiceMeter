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

using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using ServiceMeter.PerformancePlans.Basic;
using ServiceMeter.LogsServices;

namespace ServiceMeter.Support;

public sealed class Scenario
{
    private readonly List<KeyValuePair<ActType, List<PerformancePlan>>> _acts;

    private readonly List<Watcher> _watchers = new();
    
    public Scenario()
    {
        this._acts = new List<KeyValuePair<ActType, List<PerformancePlan>>>();
    }

    public Scenario AddWatchers(params Watcher[] watchers)
    {
        this._watchers.AddRange(watchers);
        return this;
    }

    private void StartAllReports()
    {
        foreach (var watcher in this._watchers)
        {
            watcher.StartAllReportsProcesses();
        }
    }

    private void StopAllReports()
    {
        foreach (var watcher in this._watchers)
        {
            watcher.StopAllReportsProcesses();
        }
    }

    private Scenario AddActs(ActType launchType, params PerformancePlan[] performancePlan)
    {
        this._acts.Add(new KeyValuePair<ActType, List<PerformancePlan>>(launchType, performancePlan.ToList()));
        return this;
    }

    public Scenario AddParallelPlans(params PerformancePlan[] performancePlan)
    {
        this.AddActs(ActType.Parallel, performancePlan);
        return this;
    }

    public Scenario AddSequentialPlans(params PerformancePlan[] performancePlan)
    {
        this.AddActs(ActType.Sequential, performancePlan);
        return this;
    }

    public async Task StartAsync()
    {
        this.StartAllReports();
            
        ScenarioTimer.Time.Start();

        foreach (var (launchActType, performancePlans) in this._acts)
        {
            if (launchActType == ActType.Parallel)
            {
                var tasks = new List<Task>();
                    
                performancePlans.ForEach(plan =>
                {
                    tasks.Add(Task.Run(async () =>
                    {
                        await plan.StartAsync();
                    }));
                });
                
                await Task.WhenAll(tasks.ToArray());
            }
            
            if (launchActType == ActType.Sequential)
            {
                foreach (var plan in performancePlans)
                {
                    await plan.StartAsync();
                }
            }
        }

        ScenarioTimer.Time.Stop();
        
        this.StopAllReports();
    }
}
