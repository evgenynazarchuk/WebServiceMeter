using System;
using System.Threading.Tasks;
using WebServiceMeter.Interfaces;
using WebServiceMeter.PerformancePlans;

namespace WebServiceMeter
{
    public sealed class UsersPerPeriod : BasicUsersPerPeriod
    {
        public UsersPerPeriod(
            ISimpleUser user,
            int usersCountPerPeriod,
            TimeSpan performancePlanDuration,
            TimeSpan? perPeriod = null,
            int sizePeriodBuffer = 60,
            int userLoopCount = 1)
            : base(user,
                  usersCountPerPeriod,
                  performancePlanDuration,
                  perPeriod,
                  sizePeriodBuffer,
                  userLoopCount)
        { }

        protected override Task StartUserAsync()
        {
            return ((ISimpleUser)this.User).InvokeAsync(this.userLoopCount);
        }
    }
}