using AuthService.Data;
using AuthService.DomainModel;
using AuthService.GenereicRepository;
using AuthService.Repository.Interface;

namespace AuthService.Repository
{
    public class UserRoleRepository:GenericRepository<UserRole>, IUserRoleRepository
    {
        public UserRoleRepository(AuthDbContext context) : base(context)
        {
        }
        // You can implement additional methods specific to UserRole here if needed
        // For example, methods to get roles by user ID, etc.
    }
    
}
