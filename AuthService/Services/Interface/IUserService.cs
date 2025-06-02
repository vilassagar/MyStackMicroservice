using AuthService.DomainModel;

namespace AuthService.Services.Interface
{

    public interface IUserService
    {
        Task<IEnumerable<ApplicationUserDto>> GetAllUsersAsync();
        Task<ApplicationUserDto?> GetUserByIdAsync(int id);
        Task<ApplicationUserDto?> GetUserByEmailAsync(string email);
        Task<ApplicationUserDto?> GetUserByUsernameAsync(string username);
        Task<IEnumerable<ApplicationUserDto>> GetUsersByTenantAsync(int tenantId);
        Task<IEnumerable<ApplicationUserDto>> GetUsersByRoleAsync(string roleName);
        Task<IEnumerable<ApplicationUserDto>> GetUsersByStatusAsync(UserStatus status);
        Task<ApplicationUserDto> CreateUserAsync(CreateUserDto createUserDto);
        Task<ApplicationUserDto> UpdateUserAsync(int id, UpdateUserDto updateUserDto);
        Task<bool> DeleteUserAsync(int id);
        Task<bool> ChangeUserStatusAsync(int userId, UserStatus status);
        Task<bool> AssignRolesToUserAsync(int userId, List<string> roleNames);
        Task<bool> RemoveRolesFromUserAsync(int userId, List<string> roleNames);
        Task<List<string>> GetUserPermissionsAsync(int userId);
    }

}
