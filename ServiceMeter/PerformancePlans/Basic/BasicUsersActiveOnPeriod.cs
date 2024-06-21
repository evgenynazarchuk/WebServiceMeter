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
using ServiceMeter.Interfaces;
using ServiceMeter.Support;

namespace ServiceMeter.PerformancePlans.Basic;

public abstract class BasicUsersActiveOnPeriod : PerformancePlan
{
    private readonly int _activeUsersCount;

    private readonly TimeSpan _performancePlanDuration;

    private readonly Task[] _activeUsers;
    
    protected BasicUsersActiveOnPeriod(
        IUser user,
        int activeUsersCount,
        TimeSpan performancePlanDuration)
        : base(user)
    {
        this._activeUsersCount = activeUsersCount;
        this._activeUsers = new Task[this._activeUsersCount];
        this._performancePlanDuration = performancePlanDuration;
    }

    public override async Task StartAsync()
    {
        var endTime = ScenarioTimer.Time.Elapsed.TotalSeconds + this._performancePlanDuration.TotalSeconds;

        while (ScenarioTimer.Time.Elapsed.TotalSeconds < endTime)
        {
            for (var i = 0; i < this._activeUsersCount; i++)
            {
                if (this._activeUsers[i] is null || this._activeUsers[i].IsCompleted)
                {
                    this._activeUsers[i] = this.StartUserAsync();
                }
            }
        }

        await this.WaitFinishUserAsync();
    }

    private async Task WaitFinishUserAsync()
    {
        foreach (var user in this._activeUsers)
        {
            if (user is not null)
            {
                await user;
            }
        }
    }
}