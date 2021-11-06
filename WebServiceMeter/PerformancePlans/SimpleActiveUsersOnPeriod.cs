using System;
using System.Threading.Tasks;
using WebServiceMeter.Interfaces;
using WebServiceMeter.PerformancePlans;

namespace WebServiceMeter
{
    public sealed class ActiveUsersOnPeriod : BasicActiveUsersOnPeriod
    {
        public ActiveUsersOnPeriod(
            ISimpleUser user,
            int activeUsersCount,
            TimeSpan performancePlanDuration,
            int userLoopCount = 1)
            : base(user,
                  activeUsersCount,
                  performancePlanDuration,
                  userLoopCount)
        { }

        protected override Task InvokeUserAsync()
        {
            return ((ISimpleUser)this.User).InvokeAsync(this.userLoopCount);
        }
    }
}