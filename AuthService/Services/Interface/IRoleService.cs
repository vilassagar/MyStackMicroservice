using AuthService.Dtos;

namespace AuthService.Services.Interface
{
    public interface IRoleService
    {
        Task<IEnumerable<RoleDto>> GetAllRolesAsync();
        Task<RoleDto?> GetRoleByIdAsync(int id);
        Task<RoleDto?> GetRoleByNameAsync(string name);
        Task<RoleDto?> GetRoleWithPermissionsAsync(int id);
        Task<IEnumerable<RoleDto>> GetUserRolesAsync(int userId);
        Task<RoleDto> CreateRoleAsync(CreateRoleDto createRoleDto);
        Task<RoleDto> UpdateRoleAsync(int id, UpdateRoleDto updateRoleDto);
        Task<bool> DeleteRoleAsync(int id);
        Task<bool> AssignPermissionsToRoleAsync(int roleId, List<int> permissionIds);
        Task<bool> RemovePermissionsFromRoleAsync(int roleId, List<int> permissionIds);
    }

}
