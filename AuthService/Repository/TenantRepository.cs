using AuthService.DomainModel;
using AuthService.GenereicRepository;
using AuthService.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Repository
{
    public class TenantRepository : GenericRepository<Tenant>, ITenantRepository
    {
        public TenantRepository(AuthDbContext context) : base(context) { }

        public async Task<IEnumerable<Tenant>> GetTenantsByLicenceAsync(int licenceId)
        {
            return await _dbSet
                .Where(t => t.LicenceId == licenceId)
                .Include(t => t.Users)
                .ToListAsync();
        }

        public async Task<Tenant?> GetTenantWithUsersAsync(int tenantId)
        {
            return await _dbSet
                .Include(t => t.Users)
                .FirstOrDefaultAsync(t => t.Id == tenantId);
        }

        public async Task<IEnumerable<Tenant>> GetTenantsWithUserCountAsync()
        {
            return await _dbSet
                .Include(t => t.Users)
                .ToListAsync();
        }

        public async Task<int> GetUserCountByTenantAsync(int tenantId)
        {
            return await _context.Set<ApplicationUser>()
                .CountAsync(u => u.TenantId == tenantId);
        }
    }

}
