using AuthService.Data;
using AuthService.DomainModel;
using AuthService.GenereicRepository;
using AuthService.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Repository
{
    public class LicenceRepository : GenericRepository<Licence>, ILicenceRepository
    {
        public LicenceRepository(AuthDbContext context) : base(context) { }

        public async Task<IEnumerable<Licence>> GetLicencesWithTenantsAsync()
        {
            return await _dbSet
                .Include(l => l.Tenant)
                .ToListAsync();
        }

        public async Task<Licence?> GetLicenceWithTenantsAsync(int licenceId)
        {
            return await _dbSet
                .Include(l => l.Tenant)
                .FirstOrDefaultAsync(l => l.Id == licenceId);
        }

        public async Task<bool> HasAvailableUserSlotsAsync(int licenceId, int requestedSlots)
        {
            var licence = await _dbSet
                .Include(l => l.Tenant)
                .ThenInclude(t => t.Users)
                .FirstOrDefaultAsync(l => l.Id == licenceId);

            if (licence == null) return false;

            var currentUserCount = licence.Tenant.Sum(t => t.Users.Count);
            return (currentUserCount + requestedSlots) <= licence.NumberOfUser;
        }
    }

}
