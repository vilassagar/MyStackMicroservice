using AuthService.Data;
using AuthService.DomainModel;
using AuthService.GenereicRepository;
using AuthService.Model;
using AuthService.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Repository
{
    public class UserRepository:  GenericRepository<ApplicationUser>, Interface.IApplicationUserRepository
    {
        public UserRepository(AuthContext context) : base(context)
        {
        }

        public async Task<PagedResult<ApplicationUser>> GetUsersByRoleAsync(string roleName, PaginationParameters parameters)
        {
            var query = _context.ApplicationUsers
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .Where(u => u.UserRoles.Any(ur => ur.Role.Name == roleName));

            if (!string.IsNullOrWhiteSpace(parameters.SearchTerm))
            {
                query = query.Where(u =>
                    u.FirstName.Contains(parameters.SearchTerm) ||
                    u.LastName.Contains(parameters.SearchTerm) ||
                    u.Email.Contains(parameters.SearchTerm));
            }

            var totalCount = await query.CountAsync();

            var users = await query
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();

            return new PagedResult<ApplicationUser>(users, totalCount, parameters.PageNumber, parameters.PageSize);
        }

        public async Task<PagedResult<ApplicationUser>> SearchUsersAsync(string searchTerm, PaginationParameters parameters)
        {
            var query = _context.ApplicationUsers.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(u =>
                    u.FirstName.Contains(searchTerm) ||
                    u.LastName.Contains(searchTerm) ||
                    u.Email.Contains(searchTerm) ||
                    u.UserName.Contains(searchTerm));
            }

            var totalCount = await query.CountAsync();

            var users = await query
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();

            return new PagedResult<ApplicationUser>(users, totalCount, parameters.PageNumber, parameters.PageSize);
        }

        protected override IQueryable<ApplicationUser> ApplySearch(IQueryable<ApplicationUser> query, string searchTerm)
        {
            return query.Where(u =>
                u.FirstName.Contains(searchTerm) ||
                u.LastName.Contains(searchTerm) ||
                u.Email.Contains(searchTerm) ||
                u.UserName.Contains(searchTerm));
        }
    }
}
