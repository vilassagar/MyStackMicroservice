
using EmployeeService.Models.Dtos;

namespace EmployeeService.InfrastructureLayer.Caching
{
    public interface ICacheService
    {
        Task<T> GetAsync<T>(string cacheKey);
        Task SetAsync(string cacheKey, EmployeeDto employeeDto, TimeSpan timeSpan);
    }
    public class CacheService : IAsyncDisposable, ICacheService
    {
        public ValueTask DisposeAsync()
        {
            throw new NotImplementedException();
        }

        public Task<T> GetAsync<T>(string cacheKey)
        {
            throw new NotImplementedException();
        }

        public Task SetAsync(string cacheKey, EmployeeDto employeeDto, TimeSpan timeSpan)
        {
            throw new NotImplementedException();
        }
    }
}
