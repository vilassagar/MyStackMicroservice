using AuthService.DomainModel;
using AuthService.GenereicRepository;

namespace AuthService.Repository.Interface
{
    public interface IUserRefreshTokenRepository : IGenericRepository<UserRefreshToken>
    {
        Task<UserRefreshToken?> GetByTokenAsync(string token);
        Task<IEnumerable<UserRefreshToken>> GetByUserIdAsync(int userId);
        Task<bool> IsTokenValidAsync(string token);
        Task RevokeTokenAsync(string token);
        Task RevokeAllUserTokensAsync(int userId);
        Task CleanupExpiredTokensAsync();
    }
}
