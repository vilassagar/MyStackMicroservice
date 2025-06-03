using AuthService.Data.UnitofWorkPattern;
using AuthService.DomainModel;
using AuthService.Dtos;
using AuthService.Services.Interface;

namespace AuthService.Services
{
    public class UserService: IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<bool> AssignRolesToUserAsync(int userId, List<string> roleNames)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ChangeUserStatusAsync(int userId, UserStatus status)
        {
            throw new NotImplementedException();
        }

        public Task<ApplicationUserDto> CreateUserAsync(CreateUserDto createUserDto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteUserAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ApplicationUserDto>> GetAllUsersAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ApplicationUserDto?> GetUserByEmailAsync(string email)
        {
            throw new NotImplementedException();
        }

        public Task<ApplicationUserDto?> GetUserByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ApplicationUserDto?> GetUserByUsernameAsync(string username)
        {
            throw new NotImplementedException();
        }

        public Task<List<string>> GetUserPermissionsAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ApplicationUserDto>> GetUsersByRoleAsync(string roleName)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ApplicationUserDto>> GetUsersByStatusAsync(UserStatus status)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ApplicationUserDto>> GetUsersByTenantAsync(int tenantId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveRolesFromUserAsync(int userId, List<string> roleNames)
        {
            throw new NotImplementedException();
        }

        public Task<ApplicationUserDto> UpdateUserAsync(int id, UpdateUserDto updateUserDto)
        {
            throw new NotImplementedException();
        }
        // Implement IUserService methods here
    }
    
}
