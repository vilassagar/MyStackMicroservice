using AuthService.DomainModel;
using AuthService.GenereicRepository;

namespace AuthService.Repository.Interface
{
    public interface ILicenceRepository : IGenericRepository<Licence>
    {
        Task<IEnumerable<Licence>> GetLicencesWithTenantsAsync();
        Task<Licence?> GetLicenceWithTenantsAsync(int licenceId);
        Task<bool> HasAvailableUserSlotsAsync(int licenceId, int requestedSlots);
    }

}
