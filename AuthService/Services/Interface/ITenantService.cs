namespace AuthService.Services.Interface
{
    public interface ITenantService
    {
        Task<IEnumerable<TenantDto>> GetAllTenantsAsync();
        Task<TenantDto?> GetTenantByIdAsync(int id);
        Task<TenantDto?> GetTenantWithUsersAsync(int id);
        Task<IEnumerable<TenantDto>> GetTenantsByLicenceAsync(int licenceId);
        Task<TenantDto> CreateTenantAsync(CreateTenantDto createTenantDto);
        Task<TenantDto> UpdateTenantAsync(int id, UpdateTenantDto updateTenantDto);
        Task<bool> DeleteTenantAsync(int id);
        Task<int> GetUserCountByTenantAsync(int tenantId);
    }

}
