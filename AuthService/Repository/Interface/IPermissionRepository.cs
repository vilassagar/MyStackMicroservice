using AuthService.DomainModel;
using AuthService.GenereicRepository;

namespace AuthService.Repository.Interface
{
    public interface IPermissionRepository : IGenericRepository<Permission>
    {
        Task<IEnumerable<Permission>> GetPermissionsByRoleAsync(int roleId);
        Task<IEnumerable<Permission>> GetPermissionsByUserAsync(int userId);
        Task<IEnumerable<Permission>> GetPermissionsByResourceAsync(string resource);
        Task<IEnumerable<Permission>> GetPermissionsByActionAsync(PermissionAction action);
        Task<Permission?> GetByNameAsync(string permissionName);
        Task<bool> UserHasPermissionAsync(int userId, string permissionName);
        Task<bool> RoleHasPermissionAsync(int roleId, string permissionName);
    }
}
