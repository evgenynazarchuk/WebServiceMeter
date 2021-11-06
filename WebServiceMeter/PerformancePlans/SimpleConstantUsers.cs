using System.Threading.Tasks;
using WebServiceMeter.Interfaces;
using WebServiceMeter.PerformancePlans;

namespace WebServiceMeter
{
    public sealed class ConstantUsers : BasicConstantUsers
    {
        public ConstantUsers(
            ISimpleUser user,
            int usersCount,
            int userLoopCount = 1)
            : base(user, usersCount, userLoopCount) { }

        public override Task InvokeUserAsync()
        {
            return ((ISimpleUser)this.User).InvokeAsync(this.userLoopCount);
        }
    }
}
