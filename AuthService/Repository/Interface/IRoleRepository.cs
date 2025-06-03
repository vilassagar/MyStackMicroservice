using AuthService.DomainModel;
using AuthService.GenereicRepository;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Repository.Interface
{
    public interface IRoleRepository : IGenericRepository<Role>
    {
        Task<Role?> GetByNameAsync(string name);
        Task<Role?> GetRoleWithPermissionsAsync(int roleId);
        Task<IEnumerable<Role>> GetRolesWithPermissionsAsync();
        Task<IEnumerable<Role>> GetUserRolesAsync(int userId);
        Task<IEnumerable<Role>> GetUsersByRoleIdAsync(int roleId);
        Task<bool> IsNameUniqueAsync(string name, int? excludeRoleId = null);
        public Task<IEnumerable<RolePermission>> GetRolePermissionsAsync(int roleId);
        
    }

}
