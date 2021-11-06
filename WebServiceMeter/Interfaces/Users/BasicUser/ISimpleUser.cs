using System.Threading.Tasks;

namespace WebServiceMeter.Interfaces
{
    public interface ISimpleUser : IBasicUser
    {
        Task InvokeAsync(int userLoopCount);

        //Task PerformanceAsync();
    }
}
