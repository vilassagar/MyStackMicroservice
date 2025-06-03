using AuthService.DomainModel;
using AuthService.GenereicRepository;

namespace AuthService.Repository.Interface
{
    public interface IUserRoleRepository:IGenericRepository<UserRole>
    {
        // Define any additional methods specific to UserRole if needed
        // For example, methods to get roles by user ID, etc.

    }
   
}
