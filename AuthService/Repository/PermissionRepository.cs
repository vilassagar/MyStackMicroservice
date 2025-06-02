using AuthService.DomainModel;
using AuthService.GenereicRepository;
using AuthService.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Repository
{
    public class PermissionRepository : GenericRepository<Permission>, IPermissionRepository
    {
        public PermissionRepository(AuthDbContext context) : base(context) { }

        public async Task<IEnumerable<Permission>> GetPermissionsByRoleAsync(int roleId)
        {
            return await _dbSet
                .Where(p => p.RolePermissions.Any(rp => rp.RoleId == roleId) && p.IsActive)
                .ToListAsync();
        }

        public async Task<IEnumerable<Permission>> GetPermissionsByUserAsync(int userId)
        {
            return await _dbSet
                .Where(p => p.UserPermissions.Any(up => up.UserId == userId) && p.IsActive)
                .ToListAsync();
        }

        public async Task<IEnumerable<Permission>> GetPermissionsByResourceAsync(string resource)
        {
            return await _dbSet
                .Where(p => p.Resource == resource && p.IsActive && !p.IsDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<Permission>> GetPermissionsByActionAsync(PermissionAction action)
        {
            return await _dbSet
                .Where(p => p.Action == action && p.IsActive && !p.IsDeleted)
                .ToListAsync();
        }

        public async Task<Permission?> GetByNameAsync(string permissionName)
        {
            return await _dbSet
                .FirstOrDefaultAsync(p => p.PermissionName == permissionName && p.IsActive && !p.IsDeleted);
        }

        public async Task<bool> UserHasPermissionAsync(int userId, string permissionName)
        {
            // Check direct user permissions
            var hasDirectPermission = await _dbSet
                .AnyAsync(p => p.PermissionName == permissionName &&
                              p.UserPermissions.Any(up => up.UserId == userId) &&
                              p.IsActive && !p.IsDeleted);

            if (hasDirectPermission) return true;

            // Check role-based permissions
            var hasRolePermission = await _context.Set<ApplicationUser>()
                .Where(u => u.Id == userId)
                .SelectMany(u => u.UserRoles)
                .Select(ur => ur.Role)
                .SelectMany(r => r.RolePermissions)
                .Select(rp => rp.Permission)
                .AnyAsync(p => p.PermissionName == permissionName && p.IsActive && !p.IsDeleted);

            return hasRolePermission;
        }

        public async Task<bool> RoleHasPermissionAsync(int roleId, string permissionName)
        {
            return await _dbSet
                .AnyAsync(p => p.PermissionName == permissionName &&
                              p.RolePermissions.Any(rp => rp.RoleId == roleId) &&
                              p.IsActive && !p.IsDeleted);
        }
    }

}
