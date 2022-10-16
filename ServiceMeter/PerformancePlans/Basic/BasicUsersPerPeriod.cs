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
using ServiceMeter.Extensions;

namespace ServiceMeter.PerformancePlans.Basic;

public abstract class BasicUsersPerPeriod : PerformancePlan
{
    private readonly int _totalUsersPerPeriod;

    private readonly TimeSpan _performancePlanDuration;

    private readonly Task[,] _invokedUsers;

    private readonly int _sizePeriodBuffer;

    private readonly Timer _timer;

    private int _currentPeriod;
    
    protected BasicUsersPerPeriod(
        IUser user,
        int usersCountPerPeriod,
        TimeSpan performancePlanDuration,
        TimeSpan? perPeriod = null,
        int sizePeriodBuffer = 60)
        : base(user)
    {
        this._totalUsersPerPeriod = usersCountPerPeriod;
        
        this._performancePlanDuration = performancePlanDuration;
        
        this._sizePeriodBuffer = sizePeriodBuffer;
        
        this._currentPeriod = 0;
        
        this._invokedUsers = new Task[sizePeriodBuffer, usersCountPerPeriod];

        var period = perPeriod ?? 1.Seconds();

        this._timer = new Timer(period.TotalMilliseconds);
        
        this._timer.Elapsed += (sender, e) => this.InvokeUsers();
    }
    
    public override async Task StartAsync()
    {
        this._timer.Start();
        
        await this.WaitFinishPerformancePlan();
        
        this._timer.Stop();
        
        this._timer.Close();
        
        await this.WaitFinishUser();
    }

    private void InvokeUsers()
    {
        for (var i = 0; i < this._totalUsersPerPeriod; i++)
        {
            this._invokedUsers[this._currentPeriod, i] = this.StartUserAsync();
        }

        this.IncrementPeriod();
    }

    private async Task WaitFinishUser()
    {
        await this._invokedUsers.Wait(this._sizePeriodBuffer, this._totalUsersPerPeriod);
    }

    private async Task WaitFinishPerformancePlan()
    {
        await Task.Delay(this._performancePlanDuration + 900.Milliseconds());
    }

    private void IncrementPeriod()
    {
        this._currentPeriod++;

        if (this._currentPeriod == this._sizePeriodBuffer)
        {
            this._currentPeriod = 0;
        }
    }
}