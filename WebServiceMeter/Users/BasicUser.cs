using WebServiceMeter.Interfaces;
using WebServiceMeter.Reports;

namespace WebServiceMeter.Users
{
    public abstract class BasicUser : IBasicUser
    {
        public readonly Watcher Watcher;

        public readonly string UserName;

        public BasicUser(string userName)
        {
            this.Watcher = new Watcher();
            this.UserName = userName;
        }
    }
}