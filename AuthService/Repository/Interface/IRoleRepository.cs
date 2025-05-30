using AuthService.DomainModel;
using AuthService.GenereicRepository;

namespace AuthService.Repository.Interface
{
    public interface IRoleRepository : IGenericRepository<Role>
    {
        Task<Role?> GetByNameAsync(string name);
        Task<Role?> GetRoleWithPermissionsAsync(int roleId);
        Task<IEnumerable<Role>> GetRolesWithPermissionsAsync();
        Task<IEnumerable<Role>> GetUserRolesAsync(int userId);
        Task<bool> IsNameUniqueAsync(string name, int? excludeRoleId = null);
    }

}
