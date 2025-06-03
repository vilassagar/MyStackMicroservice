using AuthService.Data.UnitofWorkPattern;
using AuthService.DomainModel;

namespace AuthService.Services
{
    public class AuthorizationService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AuthorizationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> UserHasPermissionAsync(int userId, string resource, PermissionAction action)
        {
            var permissionName = $"{resource}.{action}";
            return await _unitOfWork.PermissionRepository.UserHasPermissionAsync(userId, permissionName);
        }
        public async Task<List<string>> GetUserPermissionsAsync(int userId)
        {
            var directPermissions = await _unitOfWork.PermissionRepository.GetPermissionsByUserAsync(userId);
            // Get role-based permissions
            var userRoles = await _unitOfWork.RoleRepository.GetUserRolesAsync(userId);
            var rolePermissions = new List<Permission>();
            foreach (var role in userRoles)
            {
                var permissions = await _unitOfWork.PermissionRepository.GetPermissionsByRoleAsync(role.Id);
                rolePermissions.AddRange(permissions);
            }
            // Combine and deduplicate permissions
            var allPermissions = directPermissions.Concat(rolePermissions).Distinct().ToList();
            return allPermissions.Select(p => p.PermissionName).ToList();
        }

    }
}
