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
using System.Timers;
using ServiceMeter.Interfaces;

namespace ServiceMeter.PerformancePlans;

public abstract class BasicUsersPerPeriod : UsersPerformancePlan
{
    public BasicUsersPerPeriod(
        IBasicUser user,
        int usersCountPerPeriod,
        TimeSpan performancePlanDuration,
        TimeSpan? perPeriod = null,
        int sizePeriodBuffer = 60,
        int userLoopCount = 1)
        : base(user)
    {
        this.totalUsersPerPeriod = usersCountPerPeriod;
        this.performancePlanDuration = performancePlanDuration;
        this.sizePeriodBuffer = sizePeriodBuffer;
        this.currentPeriod = 0;
        this.invokedUsers = new Task[sizePeriodBuffer, usersCountPerPeriod];
        this.perPeriod = perPeriod is null ? 1.Seconds() : perPeriod.Value;

        this.runner = new Timer(this.perPeriod.TotalMilliseconds);
        this.runner.Elapsed += (sender, e) => this.InvokeUsers();

        this.userLoopCount = userLoopCount;
    }

    public override async Task StartAsync()
    {
        this.runner.Start();
        await this.WaitTerminationPerformancePlanAsync();
        this.runner.Stop();
        this.runner.Close();
        await this.WaitUserTerminationAsync();
    }

    protected abstract Task StartUserAsync();

    private void InvokeUsers()
    {
        for (var i = 0; i < this.totalUsersPerPeriod; i++)
        {
            this.invokedUsers[this.currentPeriod, i] = this.StartUserAsync();
        }

        this.IncrementPeriod();
    }

    private async Task WaitUserTerminationAsync()
    {
        await this.invokedUsers.Wait(this.sizePeriodBuffer, this.totalUsersPerPeriod);
    }

    private async Task WaitTerminationPerformancePlanAsync()
    {
        await Task.Delay(this.performancePlanDuration + 900.Milliseconds());
    }

    private void IncrementPeriod()
    {
        this.currentPeriod++;

        if (this.currentPeriod == this.sizePeriodBuffer)
        {
            this.currentPeriod = 0;
        }
    }

    protected readonly int totalUsersPerPeriod;

    protected readonly TimeSpan perPeriod;

    protected readonly TimeSpan performancePlanDuration;

    protected readonly Task[,] invokedUsers;

    protected readonly int sizePeriodBuffer;

    protected readonly Timer runner;

    protected int currentPeriod;

    protected readonly int userLoopCount;
}