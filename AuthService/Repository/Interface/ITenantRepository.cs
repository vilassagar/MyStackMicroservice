using AuthService.DomainModel;
using AuthService.GenereicRepository;

namespace AuthService.Repository.Interface
{
    public interface ITenantRepository : IGenericRepository<Tenant>
    {
        Task<IEnumerable<Tenant>> GetTenantsByLicenceAsync(int licenceId);
        Task<Tenant?> GetTenantWithUsersAsync(int tenantId);
        Task<IEnumerable<Tenant>> GetTenantsWithUserCountAsync();
        Task<int> GetUserCountByTenantAsync(int tenantId);
    }

}
