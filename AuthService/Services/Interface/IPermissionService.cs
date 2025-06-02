namespace AuthService.Services.Interface
{
    public interface IPermissionService
    {
        Task<IEnumerable<PermissionDto>> GetAllPermissionsAsync();
        Task<PermissionDto?> GetPermissionByIdAsync(int id);
        Task<PermissionDto?> GetPermissionByNameAsync(string name);
        Task<IEnumerable<PermissionDto>> GetPermissionsByRoleAsync(int roleId);
        Task<IEnumerable<PermissionDto>> GetPermissionsByUserAsync(int userId);
        Task<IEnumerable<PermissionDto>> GetPermissionsByResourceAsync(string resource);
        Task<PermissionDto> CreatePermissionAsync(CreatePermissionDto createPermissionDto);
        Task<PermissionDto> UpdatePermissionAsync(int id, UpdatePermissionDto updatePermissionDto);
        Task<bool> DeletePermissionAsync(int id);
        Task<bool> UserHasPermissionAsync(int userId, string permissionName);
        Task<bool> RoleHasPermissionAsync(int roleId, string permissionName);
    }

}
