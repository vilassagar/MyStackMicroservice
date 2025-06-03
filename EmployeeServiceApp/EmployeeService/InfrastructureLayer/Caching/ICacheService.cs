
namespace EmployeeService.InfrastructureLayer.Caching
{
    public interface ICacheService
    {

    }
    public class CacheService : IAsyncDisposable, ICacheService
    {
        public ValueTask DisposeAsync()
        {
            throw new NotImplementedException();
        }
    }
}
