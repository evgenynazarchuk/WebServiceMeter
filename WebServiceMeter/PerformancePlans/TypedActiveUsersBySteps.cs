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
using WebServiceMeter.PerformancePlans;

namespace WebServiceMeter;

public sealed class ActiveUsersBySteps<TData> : BasicActiveUsersBySteps
        where TData : class
{
    private readonly IDataReader<TData> dataReader;

    private readonly bool reuseDataInLoop;

    public ActiveUsersBySteps(
        ITypedUser<TData> user,
        int fromActiveUsersCount,
        int toActiveUsersCount,
        int usersStep,
        IDataReader<TData> dataReader,
        TimeSpan? stepPeriodDuration = null,
        TimeSpan? performancePlanDuration = null,
        int userLoopCount = 1,
        bool reuseDataInLoop = true)
        : base(user,
              fromActiveUsersCount,
              toActiveUsersCount,
              usersStep,
              stepPeriodDuration,
              performancePlanDuration,
              userLoopCount)
    {
        this.dataReader = dataReader;
        this.reuseDataInLoop = reuseDataInLoop;
    }

    protected override Task InvokeUserAsync()
    {
        return ((ITypedUser<TData>)this.User).InvokeAsync(this.dataReader, this.reuseDataInLoop, this.userLoopCount);
    }
}