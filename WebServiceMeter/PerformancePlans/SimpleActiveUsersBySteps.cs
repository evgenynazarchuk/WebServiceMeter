using System;
using System.Threading.Tasks;
using WebServiceMeter.Interfaces;
using WebServiceMeter.PerformancePlans;

namespace WebServiceMeter
{
    public sealed class ActiveUsersBySteps : BasicActiveUsersBySteps
    {
        public ActiveUsersBySteps(
            ISimpleUser user,
            int fromActiveUsersCount,
            int toActiveUsersCount,
            int usersStep,
            TimeSpan? stepPeriodDuration = null,
            TimeSpan? performancePlanDuration = null,
            int userLoopCount = 1)
            : base(user,
                  fromActiveUsersCount,
                  toActiveUsersCount,
                  usersStep,
                  stepPeriodDuration,
                  performancePlanDuration,
                  userLoopCount)
        { }

        protected override Task InvokeUserAsync()
        {
            return ((ISimpleUser)this.User).InvokeAsync(this.userLoopCount);
        }
    }
}