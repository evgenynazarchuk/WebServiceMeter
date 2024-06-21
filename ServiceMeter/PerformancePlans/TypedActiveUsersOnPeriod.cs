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
using ServiceMeter.PerformancePlans.Basic;
using ServiceMeter.Interfaces;

namespace ServiceMeter.PerformancePlans;

public sealed class UsersActiveOnPeriod<TData> : BasicUsersActiveOnPeriod
    where TData : class
{
    private readonly IDataReader<TData> _dataReader;

    public UsersActiveOnPeriod(
        ITypedUser<TData> user,
        int activeUsersCount,
        TimeSpan performancePlanDuration,
        IDataReader<TData> dataReader)
        : base(user,
              activeUsersCount,
              performancePlanDuration)
    {
        this._dataReader = dataReader;
    }

    protected override Task StartUserAsync()
    {
        var data = this._dataReader.GetData();

        if (data is null)
        {
            return Task.CompletedTask;
        }
        
        return ((ITypedUser<TData>)this.User).StartAsync(data);
    }
}