using AuthService.Dtos;

namespace AuthService.Services.Interface
{
    public interface ILicenceService
    {
        Task<IEnumerable<LicenceDto>> GetAllLicencesAsync();
        Task<LicenceDto?> GetLicenceByIdAsync(int id);
        Task<LicenceDto?> GetLicenceWithTenantsAsync(int id);
        Task<LicenceDto> CreateLicenceAsync(CreateLicenceDto createLicenceDto);
        Task<LicenceDto> UpdateLicenceAsync(int id, UpdateLicenceDto updateLicenceDto);
        Task<bool> DeleteLicenceAsync(int id);
        Task<bool> HasAvailableUserSlotsAsync(int licenceId, int requestedSlots);
    }
}
