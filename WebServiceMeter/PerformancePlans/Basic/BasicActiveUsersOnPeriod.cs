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
using System.Threading.Tasks;
using WebServiceMeter.Interfaces;
using WebServiceMeter.Support;

namespace WebServiceMeter.PerformancePlans;

public abstract class BasicActiveUsersOnPeriod : UsersPerformancePlan
{
    public BasicActiveUsersOnPeriod(
        IBasicUser user,
        int activeUsersCount,
        TimeSpan performancePlanDuration,
        int userLoopCount = 1)
        : base(user)
    {
        this.activeUsersCount = activeUsersCount;
        this.activeUsers = new Task[this.activeUsersCount];
        this.performancePlanDuration = performancePlanDuration;
        this.userLoopCount = userLoopCount;
    }

    public override async Task StartAsync()
    {
        double endTime = ScenarioTimer.Time.Elapsed.TotalSeconds + this.performancePlanDuration.TotalSeconds;

        while (ScenarioTimer.Time.Elapsed.TotalSeconds < endTime)
        {
            for (int i = 0; i < this.activeUsersCount; i++)
            {
                if (this.activeUsers[i] is null || this.activeUsers[i].IsCompleted)
                {
                    this.activeUsers[i] = this.InvokeUserAsync();
                }
            }
        }

        await this.WaitUserTerminationAsync();
    }

    protected abstract Task InvokeUserAsync();

    private async Task WaitUserTerminationAsync()
    {
        foreach (var user in this.activeUsers)
        {
            if (user is not null)
            {
                await user;
            }
        }
    }

    protected readonly int activeUsersCount;

    protected readonly TimeSpan performancePlanDuration;

    protected readonly Task[] activeUsers;

    protected readonly int userLoopCount;
}