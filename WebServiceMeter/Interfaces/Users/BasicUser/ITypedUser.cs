using System.Threading.Tasks;

namespace WebServiceMeter.Interfaces
{
    public interface ITypedUser<TEntity> : IBasicUser
        where TEntity : class
    {
        Task InvokeAsync(IDataReader<TEntity> dataReader, bool reuseDataInLoop = false, int userLoopCount = 1);
    }
}
