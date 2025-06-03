using AuthService.DomainModel;
using AuthService.GenereicRepository;
using Microsoft.EntityFrameworkCore;

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
        // Alternative implementation with additional filtering (e.g., only active permissions)
        public Task<IEnumerable<Permission>> GetByIdsAsync(List<int> permissionIds, bool includeInactive = false);
        

    }
}
