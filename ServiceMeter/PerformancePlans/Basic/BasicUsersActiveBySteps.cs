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

public abstract class BasicUsersActiveBySteps : PerformancePlan
{
    private readonly int _fromActiveUsersCount;

    private readonly int _toActiveUsersCount;

    private readonly int _usersStep;

    private readonly TimeSpan _stepPeriodDuration;

    private readonly Task[] _activeUsers;

    private readonly int _periodsCount;
    
    protected BasicUsersActiveBySteps(
        IUser user,
        int fromActiveUsersCount,
        int toActiveUsersCount,
        int usersStep,
        TimeSpan? stepPeriodDuration = null,
        TimeSpan? performancePlanDuration = null)
        : base(user)
    {
        BasicUsersActiveBySteps.UsersCountValidation(fromActiveUsersCount, toActiveUsersCount);
        
        BasicUsersActiveBySteps.UsersStepValidation(usersStep, toActiveUsersCount);
        
        BasicUsersActiveBySteps.DurationTimeValidation(stepPeriodDuration, performancePlanDuration);

        var maximumActiveUsersCount = Math.Max(fromActiveUsersCount, toActiveUsersCount);
        
        var minimumActiveUsersCount = Math.Min(fromActiveUsersCount, toActiveUsersCount);

        this._fromActiveUsersCount = fromActiveUsersCount;
        
        this._toActiveUsersCount = toActiveUsersCount;
        
        this._usersStep = usersStep;
        
        this._periodsCount = ((maximumActiveUsersCount - minimumActiveUsersCount) / usersStep) + 1;
        
        this._activeUsers = new Task[maximumActiveUsersCount];
        
        this._stepPeriodDuration = BasicUsersActiveBySteps.CalculateStepPeriodDuration(stepPeriodDuration, performancePlanDuration, this._periodsCount);
    }

    public override async Task StartAsync()
    {
        var currentMaximumActiveUsersCountPerPeriod = this._fromActiveUsersCount;

        for (var i = 1; i <= this._periodsCount; i++)
        {
            var endTime = ScenarioTimer.Time.Elapsed.TotalSeconds + this._stepPeriodDuration.TotalSeconds;

            while (ScenarioTimer.Time.Elapsed.TotalSeconds < endTime)
            {
                for (var currentUser = 0; currentUser < currentMaximumActiveUsersCountPerPeriod; currentUser++)
                {
                    if (this._activeUsers[currentUser] is null || this._activeUsers[currentUser].IsCompleted)
                    {
                        this._activeUsers[currentUser] = this.StartUserAsync();
                    }
                }
            }

            if (this._fromActiveUsersCount <= this._toActiveUsersCount)
                currentMaximumActiveUsersCountPerPeriod += this._usersStep;
            else
                currentMaximumActiveUsersCountPerPeriod -= this._usersStep;
        }

        await Task.WhenAll(this._activeUsers);
    }

    private static void UsersStepValidation(int step, int end)
    {
        if (step < 1)
            throw new ApplicationException("StepValueMustBeGreaterThanZero");

        if (step > end)
            throw new ApplicationException("StepValueMustBeLessOrEqualEndUserCount");
    }

    private static void DurationTimeValidation(
        TimeSpan? stepPeriodDuration = null,
        TimeSpan? performancePlanDuration = null)
    {
        if (stepPeriodDuration is not null && performancePlanDuration is not null)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(@"
Error!
Do not set both options:
stepPeriodDuration
performancePlanDuration
");
            Console.ResetColor();
            
            throw new ApplicationException("InitManyDuration");
        }

        if (stepPeriodDuration is null && performancePlanDuration is null)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(@"
Error!
Need to set some parameters:
stepPeriodDuration
performancePlanDuration
");
            Console.ResetColor();
            
            throw new ApplicationException("DurationIsNotSet");
        }
    }

    private static void UsersCountValidation(int fromActiveUsersCount, int toActiveUsersCount)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(@"
Error!
Incorrect fromActiveUsersCount and toActiveUsersCount
");
        Console.ResetColor();
        
        if (fromActiveUsersCount < 0 || toActiveUsersCount < 0)
            throw new ApplicationException("ErrorUsersCount");
    }

    private static TimeSpan CalculateStepPeriodDuration(
        TimeSpan? stepPeriodDuration,
        TimeSpan? performancePlanDuration,
        int periodsCount)
    {
        if (stepPeriodDuration is not null)
        {
            return stepPeriodDuration.Value;
        }
        else if (stepPeriodDuration is null && performancePlanDuration is not null)
        {
            return performancePlanDuration.Value / periodsCount;
        }
        else throw new ApplicationException("DurationIsNotSet");
    }
}