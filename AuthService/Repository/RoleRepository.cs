using AuthService.Data;
using AuthService.DomainModel;
using AuthService.GenereicRepository;
using AuthService.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Repository
{
    public class RoleRepository : GenericRepository<Role>, IRoleRepository
    {
        public RoleRepository(AuthDbContext context) : base(context) { }

        public async Task<Role?> GetByNameAsync(string name)
        {
            return await _dbSet.FirstOrDefaultAsync(r => r.Name == name);
        }

        public async Task<Role?> GetRoleWithPermissionsAsync(int roleId)
        {
            return await _dbSet
                .Include(r => r.RolePermissions)
                .ThenInclude(rp => rp.Permission)
                .FirstOrDefaultAsync(r => r.Id == roleId);
        }

        public async Task<IEnumerable<Role>> GetRolesWithPermissionsAsync()
        {
            return await _dbSet
                .Include(r => r.RolePermissions)
                .ThenInclude(rp => rp.Permission)
                .ToListAsync();
        }

        public async Task<IEnumerable<Role>> GetUserRolesAsync(int userId)
        {
            return await _dbSet
                .Where(r => r.UserRoles.Any(ur => ur.UserId == userId))
                .Include(r => r.RolePermissions)
                .ThenInclude(rp => rp.Permission)
                .ToListAsync();
        }

        public async Task<bool> IsNameUniqueAsync(string name, int? excludeRoleId = null)
        {
            var query = _dbSet.Where(r => r.Name == name);
            if (excludeRoleId.HasValue)
            {
                query = query.Where(r => r.Id != excludeRoleId.Value);
            }
            return !await query.AnyAsync();
        }

        public async Task<IEnumerable<Role>> GetUsersByRoleIdAsync(int roleId)
        {

            return await _dbSet
                .Where(u => u.UserRoles.Any(ur => ur.Role.Id == roleId))
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.User)
                .ToListAsync();

        }
        public async Task<IEnumerable<RolePermission>> GetRolePermissionsAsync(int roleId)
        {
            try
            {
                return await _context.RolePermissions
                    .Where(rp => rp.RoleId == roleId)
                    .Include(rp => rp.Permission) // Include permission details if needed
                    .ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

}
