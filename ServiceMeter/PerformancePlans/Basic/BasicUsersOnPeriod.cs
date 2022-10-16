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

public abstract class BasicUsersOnPeriod : PerformancePlan
{
    private readonly int _totalUsers;

    private readonly TimeSpan _userPerformancePlanDuration;

    private readonly Task[] _invokedUsers;

    private readonly int _usersCount;

    private readonly int _interval;

    private readonly Timer _timer;

    private readonly TimeSpan _minimalInvokePeriod;

    private int _currentInvoke;
    
    protected BasicUsersOnPeriod(
        IUser user,
        int totalUsers,
        TimeSpan performancePlanDuration,
        TimeSpan? minimalInvokePeriod = null)
        : base(user)
    {
        this._totalUsers = totalUsers;
        
        this._userPerformancePlanDuration = performancePlanDuration;
        
        this._invokedUsers = new Task[this._totalUsers];
        
        this._currentInvoke = 0;
        
        this._minimalInvokePeriod = minimalInvokePeriod ?? 1000.Milliseconds();
        
        this.CalculateUserCountOnInterval(ref this._usersCount, ref this._interval);

        this._timer = new Timer(this._interval);
        
        this._timer.Elapsed += (sender, e) => this.InvokeUsers();
    }

    public override async Task StartAsync()
    {
        this._timer.Start();
        
        await this.WaitFinishPerformancePlan();
        
        this._timer.Stop();
        
        this._timer.Close();

        await this.WaitUserTerminationAsync();
    }

    private void InvokeUsers()
    {
        if (this._currentInvoke == this._totalUsers)
        {
            return;
        }

        for (var i = 0; i < this._usersCount; i++)
        {
            this._invokedUsers[this._currentInvoke] = this.StartUserAsync();
            this._currentInvoke++;
        }
    }

    private void CalculateUserCountOnInterval(ref int userCount, ref int interval)
    {
        interval = 1;
        userCount = 1;

        if (this._userPerformancePlanDuration.TotalMilliseconds > this._totalUsers)
        {
            interval = (int)this._userPerformancePlanDuration.TotalMilliseconds / _totalUsers;
        }
        else
        {
            userCount = this._totalUsers / (int)this._userPerformancePlanDuration.TotalMilliseconds;
        }

        if (interval < this._minimalInvokePeriod.TotalMilliseconds)
        {
            userCount *= (int)this._minimalInvokePeriod.TotalMilliseconds / this._interval;
            interval = (int)this._minimalInvokePeriod.TotalMilliseconds;
        }
    }

    private async Task WaitUserTerminationAsync()
    {
        foreach (var user in this._invokedUsers)
        {
            if (user is not null)
            {
                await user;
            }
        }

        // ???
        //Task.WaitAll(_invokedUsers);
        //await Task.WhenAll(this._invokedUsers);
    }

    private async Task WaitFinishPerformancePlan()
    {
        await Task.Delay(this._userPerformancePlanDuration + 500.Milliseconds());
    }
}