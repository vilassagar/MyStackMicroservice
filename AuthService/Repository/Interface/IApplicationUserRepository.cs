using AuthService.DomainModel;
using AuthService.GenereicRepository;
using AuthService.Model;

namespace AuthService.Repository.Interface
{
    public interface IApplicationUserRepository : IGenericRepository<ApplicationUser>
    {
        Task<ApplicationUser?> GetByEmailAsync(string email);
        Task<ApplicationUser?> GetByUsernameAsync(string username);
        Task<ApplicationUser?> GetUserWithRolesAsync(int userId);
        Task<ApplicationUser?> GetUserWithPermissionsAsync(int userId);
        Task<ApplicationUser?> GetUserWithTenantAsync(int userId);
        Task<IEnumerable<ApplicationUser>> GetUsersByTenantAsync(int tenantId);
        Task<IEnumerable<ApplicationUser>> GetUsersByStatusAsync(UserStatus status);
        Task<IEnumerable<ApplicationUser>> GetUsersByRoleAsync(string roleName);
        Task<bool> IsEmailUniqueAsync(string email, int? excludeUserId = null);
        Task<bool> IsUsernameUniqueAsync(string username, int? excludeUserId = null);
    }

}
