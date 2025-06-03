using AuthService.Data;
using AuthService.DomainModel;
using AuthService.GenereicRepository;
using AuthService.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Repository
{
    public class RolePermissionRepository:GenericRepository<RolePermission>, IRolePermissionRepository
    {
        public RolePermissionRepository(AuthDbContext context) : base(context) { }
      
        public async Task<IEnumerable<RolePermission>> GetRolePermissionsByIdsAsync(int roleId, List<int> permissionIds)
        {
            try
            {
                if (permissionIds == null || !permissionIds.Any())
                {
                    return new List<RolePermission>();
                }

                return await _context.RolePermissions
                    .Where(rp => rp.RoleId == roleId && permissionIds.Contains(rp.PermissionId))
                    .Include(rp => rp.Role)        // Include related Role if needed
                    .Include(rp => rp.Permission)  // Include related Permission if needed
                    .ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        
    }
   
   
}
