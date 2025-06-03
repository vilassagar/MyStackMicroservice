using AuthService.DomainModel;
using AuthService.GenereicRepository;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Repository.Interface
{
    public interface IRolePermissionRepository: IGenericRepository<RolePermission>
    {
        // Inside your RolePermission Repository class
        public Task<IEnumerable<RolePermission>> GetRolePermissionsByIdsAsync(int roleId, List<int> permissionIds);
        
    }
}
