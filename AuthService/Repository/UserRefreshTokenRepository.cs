using AuthService.Data;
using AuthService.DomainModel;
using AuthService.GenereicRepository;
using AuthService.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Repository
{
    public class UserRefreshTokenRepository : GenericRepository<UserRefreshToken>, IUserRefreshTokenRepository
    {
        public UserRefreshTokenRepository(AuthDbContext context) : base(context) { }

        public async Task<UserRefreshToken?> GetByTokenAsync(string token)
        {
            return await _dbSet
                .Include(rt => rt.User)
                .FirstOrDefaultAsync(rt => rt.Token == token);
        }

        public async Task<IEnumerable<UserRefreshToken>> GetByUserIdAsync(int userId)
        {
            return await _dbSet
                .Where(rt => rt.UserId == userId)
                .OrderByDescending(rt => rt.ExpirationDate)
                .ToListAsync();
        }

        public async Task<bool> IsTokenValidAsync(string token)
        {
            return await _dbSet
                .AnyAsync(rt => rt.Token == token && rt.ExpirationDate > DateTime.UtcNow);
        }

        public async Task RevokeTokenAsync(string token)
        {
            var refreshToken = await _dbSet.FirstOrDefaultAsync(rt => rt.Token == token);
            if (refreshToken != null)
            {
                _dbSet.Remove(refreshToken);
            }
        }

        public async Task RevokeAllUserTokensAsync(int userId)
        {
            var tokens = await _dbSet.Where(rt => rt.UserId == userId).ToListAsync();
            _dbSet.RemoveRange(tokens);
        }

        public async Task CleanupExpiredTokensAsync()
        {
            var expiredTokens = await _dbSet
                .Where(rt => rt.ExpirationDate <= DateTime.UtcNow)
                .ToListAsync();

            _dbSet.RemoveRange(expiredTokens);
        }
    }

}
