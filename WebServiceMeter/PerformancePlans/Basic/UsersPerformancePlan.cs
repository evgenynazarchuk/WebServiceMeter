using System.Threading.Tasks;
using WebServiceMeter.Interfaces;
using WebServiceMeter.Users;

namespace WebServiceMeter
{
    public abstract class UsersPerformancePlan
    {
        public UsersPerformancePlan(IBasicUser user)
        {
            this.User = (BasicUser)user;
        }

        public abstract Task StartAsync();

        public readonly BasicUser User;
    }
}
