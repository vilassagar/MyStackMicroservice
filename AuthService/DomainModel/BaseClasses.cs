using AuthService.DomainModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AuthService.DomainModel
{
    public interface IEntity
    {
        int Id { get; set; }
    }

    public interface IAuditableEntity : IEntity
    {
        DateTime CreatedAt { get; set; }
        DateTime UpdatedAt { get; set; }
    }
    public interface ISoftDeletable
    {
        bool IsDeleted { get; set; }
    }

}








   

   

   



// 5. Specific Repository Interfaces






public interface IUserRefreshTokenRepository : IGenericRepository<UserRefreshToken>
{
    Task<UserRefreshToken?> GetByTokenAsync(string token);
    Task<IEnumerable<UserRefreshToken>> GetByUserIdAsync(int userId);
    Task<bool> IsTokenValidAsync(string token);
    Task RevokeTokenAsync(string token);
    Task RevokeAllUserTokensAsync(int userId);
    Task CleanupExpiredTokensAsync();
}

// 6. Specific Repository Implementations
public class LicenceRepository : GenericRepository<Licence>, ILicenceRepository
{
    public LicenceRepository(AuthDbContext context) : base(context) { }

    public async Task<IEnumerable<Licence>> GetLicencesWithTenantsAsync()
    {
        return await _dbSet
            .Include(l => l.Tenant)
            .ToListAsync();
    }

    public async Task<Licence?> GetLicenceWithTenantsAsync(int licenceId)
    {
        return await _dbSet
            .Include(l => l.Tenant)
            .FirstOrDefaultAsync(l => l.Id == licenceId);
    }

    public async Task<bool> HasAvailableUserSlotsAsync(int licenceId, int requestedSlots)
    {
        var licence = await _dbSet
            .Include(l => l.Tenant)
            .ThenInclude(t => t.Users)
            .FirstOrDefaultAsync(l => l.Id == licenceId);

        if (licence == null) return false;

        var currentUserCount = licence.Tenant.Sum(t => t.Users.Count);
        return (currentUserCount + requestedSlots) <= licence.NumberOfUser;
    }
}

public class TenantRepository : GenericRepository<Tenant>, ITenantRepository
{
    public TenantRepository(AuthDbContext context) : base(context) { }

    public async Task<IEnumerable<Tenant>> GetTenantsByLicenceAsync(int licenceId)
    {
        return await _dbSet
            .Where(t => t.LicenceId == licenceId)
            .Include(t => t.Users)
            .ToListAsync();
    }

    public async Task<Tenant?> GetTenantWithUsersAsync(int tenantId)
    {
        return await _dbSet
            .Include(t => t.Users)
            .FirstOrDefaultAsync(t => t.Id == tenantId);
    }

    public async Task<IEnumerable<Tenant>> GetTenantsWithUserCountAsync()
    {
        return await _dbSet
            .Include(t => t.Users)
            .ToListAsync();
    }

    public async Task<int> GetUserCountByTenantAsync(int tenantId)
    {
        return await _context.Set<ApplicationUser>()
            .CountAsync(u => u.TenantId == tenantId);
    }
}

public class ApplicationUserRepository : GenericRepository<ApplicationUser>, IApplicationUserRepository
{
    public ApplicationUserRepository(AuthDbContext context) : base(context) { }

    public async Task<ApplicationUser?> GetByEmailAsync(string email)
    {
        return await _dbSet.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<ApplicationUser?> GetByUsernameAsync(string username)
    {
        return await _dbSet.FirstOrDefaultAsync(u => u.UserName == username);
    }

    public async Task<ApplicationUser?> GetUserWithRolesAsync(int userId)
    {
        return await _dbSet
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Id == userId);
    }

    public async Task<ApplicationUser?> GetUserWithPermissionsAsync(int userId)
    {
        return await _dbSet
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .ThenInclude(r => r.RolePermissions)
            .ThenInclude(rp => rp.Permission)
            .FirstOrDefaultAsync(u => u.Id == userId);
    }

    public async Task<ApplicationUser?> GetUserWithTenantAsync(int userId)
    {
        return await _dbSet
            .Include(u => u.Tenant)
            .FirstOrDefaultAsync(u => u.Id == userId);
    }

    public async Task<IEnumerable<ApplicationUser>> GetUsersByTenantAsync(int tenantId)
    {
        return await _dbSet
            .Where(u => u.TenantId == tenantId)
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .ToListAsync();
    }

    public async Task<IEnumerable<ApplicationUser>> GetUsersByStatusAsync(UserStatus status)
    {
        return await _dbSet
            .Where(u => u.Status == status)
            .Include(u => u.Tenant)
            .ToListAsync();
    }

    public async Task<IEnumerable<ApplicationUser>> GetUsersByRoleAsync(string roleName)
    {
        return await _dbSet
            .Where(u => u.UserRoles.Any(ur => ur.Role.Name == roleName))
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .ToListAsync();
    }

    public async Task<bool> IsEmailUniqueAsync(string email, int? excludeUserId = null)
    {
        var query = _dbSet.Where(u => u.Email == email);
        if (excludeUserId.HasValue)
        {
            query = query.Where(u => u.Id != excludeUserId.Value);
        }
        return !await query.AnyAsync();
    }

    public async Task<bool> IsUsernameUniqueAsync(string username, int? excludeUserId = null)
    {
        var query = _dbSet.Where(u => u.UserName == username);
        if (excludeUserId.HasValue)
        {
            query = query.Where(u => u.Id != excludeUserId.Value);
        }
        return !await query.AnyAsync();
    }
}

public class RoleRepository : GenericRepository<Role>, IRoleRepository
{
    public RoleRepository(AuthDbContext context) : base(context) { }

    public async Task<Role?> GetByNameAsync(string name)
    {
        return await _dbSet.FirstOrDefaultAsync(r => r.Name == name);
    }

    public async Task<Role?> GetRoleWithPermissionsAsync(int roleId)
    {
        return await _dbSet
            .Include(r => r.RolePermissions)
            .ThenInclude(rp => rp.Permission)
            .FirstOrDefaultAsync(r => r.Id == roleId);
    }

    public async Task<IEnumerable<Role>> GetRolesWithPermissionsAsync()
    {
        return await _dbSet
            .Include(r => r.RolePermissions)
            .ThenInclude(rp => rp.Permission)
            .ToListAsync();
    }

    public async Task<IEnumerable<Role>> GetUserRolesAsync(int userId)
    {
        return await _dbSet
            .Where(r => r.UserRoles.Any(ur => ur.UserId == userId))
            .Include(r => r.RolePermissions)
            .ThenInclude(rp => rp.Permission)
            .ToListAsync();
    }

    public async Task<bool> IsNameUniqueAsync(string name, int? excludeRoleId = null)
    {
        var query = _dbSet.Where(r => r.Name == name);
        if (excludeRoleId.HasValue)
        {
            query = query.Where(r => r.Id != excludeRoleId.Value);
        }
        return !await query.AnyAsync();
    }
}

public class PermissionRepository : GenericRepository<Permission>, IPermissionRepository
{
    public PermissionRepository(AuthDbContext context) : base(context) { }

    public async Task<IEnumerable<Permission>> GetPermissionsByRoleAsync(int roleId)
    {
        return await _dbSet
            .Where(p => p.RolePermissions.Any(rp => rp.RoleId == roleId) && p.IsActive)
            .ToListAsync();
    }

    public async Task<IEnumerable<Permission>> GetPermissionsByUserAsync(int userId)
    {
        return await _dbSet
            .Where(p => p.UserPermissions.Any(up => up.UserId == userId) && p.IsActive)
            .ToListAsync();
    }

    public async Task<IEnumerable<Permission>> GetPermissionsByResourceAsync(string resource)
    {
        return await _dbSet
            .Where(p => p.Resource == resource && p.IsActive && !p.IsDeleted)
            .ToListAsync();
    }

    public async Task<IEnumerable<Permission>> GetPermissionsByActionAsync(PermissionAction action)
    {
        return await _dbSet
            .Where(p => p.Action == action && p.IsActive && !p.IsDeleted)
            .ToListAsync();
    }

    public async Task<Permission?> GetByNameAsync(string permissionName)
    {
        return await _dbSet
            .FirstOrDefaultAsync(p => p.PermissionName == permissionName && p.IsActive && !p.IsDeleted);
    }

    public async Task<bool> UserHasPermissionAsync(int userId, string permissionName)
    {
        // Check direct user permissions
        var hasDirectPermission = await _dbSet
            .AnyAsync(p => p.PermissionName == permissionName &&
                          p.UserPermissions.Any(up => up.UserId == userId) &&
                          p.IsActive && !p.IsDeleted);

        if (hasDirectPermission) return true;

        // Check role-based permissions
        var hasRolePermission = await _context.Set<ApplicationUser>()
            .Where(u => u.Id == userId)
            .SelectMany(u => u.UserRoles)
            .Select(ur => ur.Role)
            .SelectMany(r => r.RolePermissions)
            .Select(rp => rp.Permission)
            .AnyAsync(p => p.PermissionName == permissionName && p.IsActive && !p.IsDeleted);

        return hasRolePermission;
    }

    public async Task<bool> RoleHasPermissionAsync(int roleId, string permissionName)
    {
        return await _dbSet
            .AnyAsync(p => p.PermissionName == permissionName &&
                          p.RolePermissions.Any(rp => rp.RoleId == roleId) &&
                          p.IsActive && !p.IsDeleted);
    }
}

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

// 7. Unit of Work Interface
public interface IUnitOfWork : IDisposable
{
    ILicenceRepository Licences { get; }
    ITenantRepository Tenants { get; }
    IApplicationUserRepository Users { get; }
    IRoleRepository Roles { get; }
    IPermissionRepository Permissions { get; }
    IUserRefreshTokenRepository RefreshTokens { get; }
    IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class, IEntity;

    Task<int> SaveChangesAsync();
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}

// 8. Unit of Work Implementation
public class UnitOfWork : IUnitOfWork
{
    private readonly AuthDbContext _context;
    private readonly Dictionary<Type, object> _repositories;
    private IDbContextTransaction? _transaction;

    public UnitOfWork(AuthDbContext context)
    {
        _context = context;
        _repositories = new Dictionary<Type, object>();

        // Initialize specific repositories
        Licences = new LicenceRepository(_context);
        Tenants = new TenantRepository(_context);
        Users = new ApplicationUserRepository(_context);
        Roles = new RoleRepository(_context);
        Permissions = new PermissionRepository(_context);
        RefreshTokens = new UserRefreshTokenRepository(_context);
    }

    public ILicenceRepository Licences { get; }
    public ITenantRepository Tenants { get; }
    public IApplicationUserRepository Users { get; }
    public IRoleRepository Roles { get; }
    public IPermissionRepository Permissions { get; }
    public IUserRefreshTokenRepository RefreshTokens { get; }

    public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class, IEntity
    {
        var type = typeof(TEntity);

        if (!_repositories.ContainsKey(type))
        {
            _repositories[type] = new GenericRepository<TEntity>(_context);
        }

        return (IGenericRepository<TEntity>)_repositories[type];
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task BeginTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.CommitAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task RollbackTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }
}

// 9. DTOs (Data Transfer Objects)
public class LicenceDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int NumberOfUser { get; set; }
    public int CurrentUserCount { get; set; }
    public int AvailableSlots { get; set; }
    public List<TenantDto> Tenants { get; set; } = new();
}

public class TenantDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int LicenceId { get; set; }
    public string LicenceName { get; set; } = string.Empty;
    public int UserCount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public List<ApplicationUserDto> Users { get; set; } = new();
}

public class ApplicationUserDto
{
    public int Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName => $"{FirstName} {LastName}".Trim();
    public int TenantId { get; set; }
    public string TenantName { get; set; } = string.Empty;
    public UserStatus Status { get; set; }
    public bool EmailConfirmed { get; set; }
    public bool PhoneNumberConfirmed { get; set; }
    public bool TwoFactorEnabled { get; set; }
    public List<string> Roles { get; set; } = new();
    public List<string> Permissions { get; set; } = new();
}

public class RoleDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<PermissionDto> Permissions { get; set; } = new();
    public int UserCount { get; set; }
}

public class PermissionDto
{
    public int Id { get; set; }
    public string PermissionName { get; set; } = string.Empty;
    public string Resource { get; set; } = string.Empty;
    public PermissionAction Action { get; set; }
    public string ActionName => Action.ToString();
    public string? Description { get; set; }
    public bool IsActive { get; set; }
}

// Create DTOs
public class CreateLicenceDto
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [StringLength(500)]
    public string Description { get; set; } = string.Empty;

    [Required]
    [Range(1, int.MaxValue)]
    public int NumberOfUser { get; set; }
}

public class CreateTenantDto
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [StringLength(500)]
    public string Description { get; set; } = string.Empty;

    [Required]
    public int LicenceId { get; set; }
}

public class CreateUserDto
{
    [Required]
    [StringLength(100)]
    public string UserName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string LastName { get; set; } = string.Empty;

    [Required]
    public int TenantId { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 6)]
    public string Password { get; set; } = string.Empty;

    public List<string> Roles { get; set; } = new();
}

public class CreateRoleDto
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [StringLength(500)]
    public string Description { get; set; } = string.Empty;

    public List<int> PermissionIds { get; set; } = new();
}

public class CreatePermissionDto
{
    [Required]
    [StringLength(100)]
    public string PermissionName { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Resource { get; set; } = string.Empty;

    [Required]
    public PermissionAction Action { get; set; }

    [StringLength(500)]
    public string? Description { get; set; }
}

// Update DTOs
public class UpdateLicenceDto
{
    [StringLength(100)]
    public string? Name { get; set; }

    [StringLength(500)]
    public string? Description { get; set; }

    [Range(1, int.MaxValue)]
    public int? NumberOfUser { get; set; }
}

public class UpdateTenantDto
{
    [StringLength(100)]
    public string? Name { get; set; }

    [StringLength(500)]
    public string? Description { get; set; }

    public int? LicenceId { get; set; }
}

public class UpdateUserDto
{
    [StringLength(100)]
    public string? UserName { get; set; }

    [EmailAddress]
    public string? Email { get; set; }

    [StringLength(100)]
    public string? FirstName { get; set; }

    [StringLength(100)]
    public string? LastName { get; set; }

    public int? TenantId { get; set; }

    public UserStatus? Status { get; set; }

    public List<string>? Roles { get; set; }
}

public class UpdateRoleDto
{
    [StringLength(100)]
    public string? Name { get; set; }

    [StringLength(500)]
    public string? Description { get; set; }

    public List<int>? PermissionIds { get; set; }
}

public class UpdatePermissionDto
{
    [StringLength(100)]
    public string? PermissionName { get; set; }

    [StringLength(100)]
    public string? Resource { get; set; }

    public PermissionAction? Action { get; set; }

    [StringLength(500)]
    public string? Description { get; set; }

    public bool? IsActive { get; set; }
}

// 10. AutoMapper Profiles
public class AuthMappingProfile : Profile
{
    public AuthMappingProfile()
    {
        // Licence mappings
        CreateMap<Licence, LicenceDto>()
            .ForMember(dest => dest.CurrentUserCount, opt => opt.MapFrom(src => src.Tenant.Sum(t => t.Users.Count)))
            .ForMember(dest => dest.AvailableSlots, opt => opt.MapFrom(src => src.NumberOfUser - src.Tenant.Sum(t => t.Users.Count)));

        CreateMap<CreateLicenceDto, Licence>();
        CreateMap<UpdateLicenceDto, Licence>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        // Tenant mappings
        CreateMap<Tenant, TenantDto>()
            .ForMember(dest => dest.UserCount, opt => opt.MapFrom(src => src.Users.Count))
            .ForMember(dest => dest.LicenceName, opt => opt.Ignore()); // Will be set in service if needed

        CreateMap<CreateTenantDto, Tenant>();
        CreateMap<UpdateTenantDto, Tenant>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        // User mappings
        CreateMap<ApplicationUser, ApplicationUserDto>()
            .ForMember(dest => dest.TenantName, opt => opt.MapFrom(src => src.Tenant.Name))
            .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.UserRoles.Select(ur => ur.Role.Name)))
            .ForMember(dest => dest.Permissions, opt => opt.Ignore()); // Will be calculated in service

        CreateMap<CreateUserDto, ApplicationUser>()
            .ForMember(dest => dest.UserRoles, opt => opt.Ignore())
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => UserStatus.Pending));

        CreateMap<UpdateUserDto, ApplicationUser>()
            .ForMember(dest => dest.UserRoles, opt => opt.Ignore())
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        // Role mappings
        CreateMap<Role, RoleDto>()
            .ForMember(dest => dest.UserCount, opt => opt.MapFrom(src => src.UserRoles.Count));

        CreateMap<CreateRoleDto, Role>()
            .ForMember(dest => dest.RolePermissions, opt => opt.Ignore());

        CreateMap<UpdateRoleDto, Role>()
            .ForMember(dest => dest.RolePermissions, opt => opt.Ignore())
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        // Permission mappings
        CreateMap<Permission, PermissionDto>();
        CreateMap<CreatePermissionDto, Permission>();
        CreateMap<UpdatePermissionDto, Permission>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}

// 11. Service Interfaces
public interface ILicenceService
{
    Task<IEnumerable<LicenceDto>> GetAllLicencesAsync();
    Task<LicenceDto?> GetLicenceByIdAsync(int id);
    Task<LicenceDto?> GetLicenceWithTenantsAsync(int id);
    Task<LicenceDto> CreateLicenceAsync(CreateLicenceDto createLicenceDto);
    Task<LicenceDto> UpdateLicenceAsync(int id, UpdateLicenceDto updateLicenceDto);
    Task<bool> DeleteLicenceAsync(int id);
    Task<bool> HasAvailableUserSlotsAsync(int licenceId, int requestedSlots);
}

public interface ITenantService
{
    Task<IEnumerable<TenantDto>> GetAllTenantsAsync();
    Task<TenantDto?> GetTenantByIdAsync(int id);
    Task<TenantDto?> GetTenantWithUsersAsync(int id);
    Task<IEnumerable<TenantDto>> GetTenantsByLicenceAsync(int licenceId);
    Task<TenantDto> CreateTenantAsync(CreateTenantDto createTenantDto);
    Task<TenantDto> UpdateTenantAsync(int id, UpdateTenantDto updateTenantDto);
    Task<bool> DeleteTenantAsync(int id);
    Task<int> GetUserCountByTenantAsync(int tenantId);
}

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

// 12. Service Implementations
public class LicenceService : ILicenceService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<LicenceService> _logger;

    public LicenceService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<LicenceService> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<LicenceDto>> GetAllLicencesAsync()
    {
        try
        {
            var licences = await _unitOfWork.Licences.GetLicencesWithTenantsAsync();
            return _mapper.Map<IEnumerable<LicenceDto>>(licences);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting all licences");
            throw;
        }
    }

    public async Task<LicenceDto?> GetLicenceByIdAsync(int id)
    {
        try
        {
            var licence = await _unitOfWork.Licences.GetByIdAsync(id);
            return licence != null ? _mapper.Map<LicenceDto>(licence) : null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting licence with ID {LicenceId}", id);
            throw;
        }
    }

    public async Task<LicenceDto?> GetLicenceWithTenantsAsync(int id)
    {
        try
        {
            var licence = await _unitOfWork.Licences.GetLicenceWithTenantsAsync(id);
            return licence != null ? _mapper.Map<LicenceDto>(licence) : null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting licence with tenants for ID {LicenceId}", id);
            throw;
        }
    }

    public async Task<LicenceDto> CreateLicenceAsync(CreateLicenceDto createLicenceDto)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync();

            var licence = _mapper.Map<Licence>(createLicenceDto);
            await _unitOfWork.Licences.AddAsync(licence);
            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitTransactionAsync();

            _logger.LogInformation("Licence created with ID {LicenceId}", licence.Id);
            return _mapper.Map<LicenceDto>(licence);
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync();
            _logger.LogError(ex, "Error occurred while creating licence");
            throw;
        }
    }

    public async Task<LicenceDto> UpdateLicenceAsync(int id, UpdateLicenceDto updateLicenceDto)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync();

            var existingLicence = await _unitOfWork.Licences.GetByIdAsync(id);
            if (existingLicence == null)
            {
                throw new NotFoundException($"Licence with ID {id} not found");
            }

            // Validate NumberOfUser decrease doesn't violate current usage
            if (updateLicenceDto.NumberOfUser.HasValue)
            {
                var currentUsage = await GetCurrentUserCountAsync(id);
                if (updateLicenceDto.NumberOfUser.Value < currentUsage)
                {
                    throw new InvalidOperationException($"Cannot reduce user limit below current usage ({currentUsage})");
                }
            }

            _mapper.Map(updateLicenceDto, existingLicence);
            await _unitOfWork.Licences.UpdateAsync(existingLicence);
            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitTransactionAsync();

            _logger.LogInformation("Licence updated with ID {LicenceId}", id);
            return _mapper.Map<LicenceDto>(existingLicence);
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync();
            _logger.LogError(ex, "Error occurred while updating licence with ID {LicenceId}", id);
            throw;
        }
    }

    public async Task<bool> DeleteLicenceAsync(int id)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync();

            var licence = await _unitOfWork.Licences.GetLicenceWithTenantsAsync(id);
            if (licence == null)
            {
                return false;
            }

            // Check if licence has tenants
            if (licence.Tenant.Any())
            {
                throw new InvalidOperationException("Cannot delete licence that has tenants");
            }

            await _unitOfWork.Licences.DeleteAsync(licence);
            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitTransactionAsync();

            _logger.LogInformation("Licence deleted with ID {LicenceId}", id);
            return true;
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync();
            _logger.LogError(ex, "Error occurred while deleting licence with ID {LicenceId}", id);
            throw;
        }
    }

    public async Task<bool> HasAvailableUserSlotsAsync(int licenceId, int requestedSlots)
    {
        try
        {
            return await _unitOfWork.Licences.HasAvailableUserSlotsAsync(licenceId, requestedSlots);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while checking available slots for licence {LicenceId}", licenceId);
            throw;
        }
    }

    private async Task<int> GetCurrentUserCountAsync(int licenceId)
    {
        var tenants = await _unitOfWork.Tenants.GetTenantsByLicenceAsync(licenceId);
        return tenants.Sum(t => t.Users.Count);
    }
}

public class TenantService : ITenantService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<TenantService> _logger;

    public TenantService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<TenantService> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<TenantDto>> GetAllTenantsAsync()
    {
        try
        {
            var tenants = await _unitOfWork.Tenants.GetTenantsWithUserCountAsync();
            return _mapper.Map<IEnumerable<TenantDto>>(tenants);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting all tenants");
            throw;
        }
    }

    public async Task<TenantDto?> GetTenantByIdAsync(int id)
    {
        try
        {
            var tenant = await _unitOfWork.Tenants.GetByIdAsync(id);
            return tenant != null ? _mapper.Map<TenantDto>(tenant) : null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting tenant with ID {TenantId}", id);
            throw;
        }
    }

    public async Task<TenantDto?> GetTenantWithUsersAsync(int id)
    {
        try
        {
            var tenant = await _unitOfWork.Tenants.GetTenantWithUsersAsync(id);
            return tenant != null ? _mapper.Map<TenantDto>(tenant) : null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting tenant with users for ID {TenantId}", id);
            throw;
        }
    }

    public async Task<IEnumerable<TenantDto>> GetTenantsByLicenceAsync(int licenceId)
    {
        try
        {
            var tenants = await _unitOfWork.Tenants.GetTenantsByLicenceAsync(licenceId);
            return _mapper.Map<IEnumerable<TenantDto>>(tenants);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting tenants for licence {LicenceId}", licenceId);
            throw;
        }
    }

    public async Task<TenantDto> CreateTenantAsync(CreateTenantDto createTenantDto)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync();

            // Validate licence exists
            var licenceExists = await _unitOfWork.Licences.AnyAsync(l => l.Id == createTenantDto.LicenceId);
            if (!licenceExists)
            {
                throw new NotFoundException($"Licence with ID {createTenantDto.LicenceId} not found");
            }

            var tenant = _mapper.Map<Tenant>(createTenantDto);
            await _unitOfWork.Tenants.AddAsync(tenant);
            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitTransactionAsync();

            _logger.LogInformation("Tenant created with ID {TenantId}", tenant.Id);
            return _mapper.Map<TenantDto>(tenant);
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync();
            _logger.LogError(ex, "Error occurred while creating tenant");
            throw;
        }
    }

    public async Task<TenantDto> UpdateTenantAsync(int id, UpdateTenantDto updateTenantDto)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync();

            var existingTenant = await _unitOfWork.Tenants.GetByIdAsync(id);
            if (existingTenant == null)
            {
                throw new NotFoundException($"Tenant with ID {id} not found");
            }

            // Validate licence if provided
            if (updateTenantDto.LicenceId.HasValue)
            {
                var licenceExists = await _unitOfWork.Licences.AnyAsync(l => l.Id == updateTenantDto.LicenceId.Value);
                if (!licenceExists)
                {
                    throw new NotFoundException($"Licence with ID {updateTenantDto.LicenceId} not found");
                }
            }

            _mapper.Map(updateTenantDto, existingTenant);
            await _unitOfWork.Tenants.UpdateAsync(existingTenant);
            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitTransactionAsync();

            _logger.LogInformation("Tenant updated with ID {TenantId}", id);
            return _mapper.Map<TenantDto>(existingTenant);
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync();
            _logger.LogError(ex, "Error occurred while updating tenant with ID {TenantId}", id);
            throw;
        }
    }

    public async Task<bool> DeleteTenantAsync(int id)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync();

            var tenant = await _unitOfWork.Tenants.GetTenantWithUsersAsync(id);
            if (tenant == null)
            {
                return false;
            }

            // Check if tenant has users
            if (tenant.Users.Any())
            {
                throw new InvalidOperationException("Cannot delete tenant that has users");
            }

            await _unitOfWork.Tenants.DeleteAsync(tenant);
            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitTransactionAsync();

            _logger.LogInformation("Tenant deleted with ID {TenantId}", id);
            return true;
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync();
            _logger.LogError(ex, "Error occurred while deleting tenant with ID {TenantId}", id);
            throw;
        }
    }

    public async Task<int> GetUserCountByTenantAsync(int tenantId)
    {
        try
        {
            return await _unitOfWork.Tenants.GetUserCountByTenantAsync(tenantId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting user count for tenant {TenantId}", tenantId);
            throw;
        }
    }
}

// 13. Custom Exceptions
public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message) { }
}

public class ValidationException : Exception
{
    public ValidationException(string message) : base(message) { }
}

// 14. DbContext (Enhanced version of your existing context)
public class AuthDbContext : IdentityDbContext<ApplicationUser, Role, int, UserClaim, UserRole, UserLogin, IdentityRoleClaim<int>, UserToken>
{
    public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options) { }

    public DbSet<Licence> Licences { get; set; }
    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<Permission> Permissions { get; set; }
    public DbSet<RolePermission> RolePermissions { get; set; }
    public DbSet<UserPermission> UserPermissions { get; set; }
    public DbSet<RolePermissionHistory> RolePermissionHistory { get; set; }
    public DbSet<UserRefreshToken> UserRefreshTokens { get; set; }
    public DbSet<UserPasswordHistory> UserPasswordHistory { get; set; }
    public DbSet<UserTwoFactor> UserTwoFactor { get; set; }
    public DbSet<UserLoginHistory> UserLoginHistory { get; set; }
    public DbSet<UserSession> UserSessions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply all your existing configurations
        modelBuilder.ApplyConfiguration(new LicenceConfiguration());
        modelBuilder.ApplyConfiguration(new TenantConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new RoleConfiguration());
        modelBuilder.ApplyConfiguration(new UserRoleConfiguration());

        // Additional configurations for new entities
        modelBuilder.Entity<RolePermission>(entity =>
        {
            entity.HasKey(rp => new { rp.RoleId, rp.PermissionId });
            entity.Property(rp => rp.GrantedAt).IsRequired();
            entity.Property(rp => rp.GrantedBy).HasMaxLength(100);

            entity.HasOne(rp => rp.Role)
                  .WithMany(r => r.RolePermissions)
                  .HasForeignKey(rp => rp.RoleId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(rp => rp.Permission)
                  .WithMany(p => p.RolePermissions)
                  .HasForeignKey(rp => rp.PermissionId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<UserPermission>(entity =>
        {
            entity.HasKey(up => new { up.UserId, up.PermissionId });
            entity.Property(up => up.GrantedAt).IsRequired();
            entity.Property(up => up.GrantedBy).HasMaxLength(100);

            entity.HasOne(up => up.User)
                  .WithMany()
                  .HasForeignKey(up => up.UserId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(up => up.Permission)
                  .WithMany(p => p.UserPermissions)
                  .HasForeignKey(up => up.PermissionId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<UserRefreshToken>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Token).IsRequired().HasMaxLength(500);
            entity.Property(e => e.ExpirationDate).IsRequired();

            entity.HasOne(e => e.User)
                  .WithMany()
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Additional entity configurations...
        modelBuilder.Entity<UserPasswordHistory>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.PasswordHash).IsRequired().HasMaxLength(500);
            entity.Property(e => e.CreatedAt).IsRequired();

            entity.HasOne(e => e.User)
                  .WithMany()
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<UserLoginHistory>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.LoginTime).IsRequired();
            entity.Property(e => e.IpAddress).HasMaxLength(45);

            entity.HasOne(e => e.User)
                  .WithMany()
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Automatically set timestamps for auditable entities
        var entries = ChangeTracker.Entries<IAuditableEntity>();

        foreach (var entry in entries)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    break;
                case EntityState.Modified:
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    break;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}

// 15. Controllers
[ApiController]
[Route("api/[controller]")]
public class LicencesController : ControllerBase
{
    private readonly ILicenceService _licenceService;

    public LicencesController(ILicenceService licenceService)
    {
        _licenceService = licenceService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<LicenceDto>>> GetLicences()
    {
        var licences = await _licenceService.GetAllLicencesAsync();
        return Ok(licences);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<LicenceDto>> GetLicence(int id)
    {
        var licence = await _licenceService.GetLicenceByIdAsync(id);
        if (licence == null)
            return NotFound();

        return Ok(licence);
    }

    [HttpGet("{id}/with-tenants")]
    public async Task<ActionResult<LicenceDto>> GetLicenceWithTenants(int id)
    {
        var licence = await _licenceService.GetLicenceWithTenantsAsync(id);
        if (licence == null)
            return NotFound();

        return Ok(licence);
    }

    [HttpGet("{id}/available-slots/{requestedSlots}")]
    public async Task<ActionResult<bool>> CheckAvailableSlots(int id, int requestedSlots)
    {
        var hasSlots = await _licenceService.HasAvailableUserSlotsAsync(id, requestedSlots);
        return Ok(hasSlots);
    }

    [HttpPost]
    public async Task<ActionResult<LicenceDto>> CreateLicence(CreateLicenceDto createLicenceDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var licence = await _licenceService.CreateLicenceAsync(createLicenceDto);
        return CreatedAtAction(nameof(GetLicence), new { id = licence.Id }, licence);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<LicenceDto>> UpdateLicence(int id, UpdateLicenceDto updateLicenceDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var licence = await _licenceService.UpdateLicenceAsync(id, updateLicenceDto);
            return Ok(licence);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteLicence(int id)
    {
        try
        {
            var result = await _licenceService.DeleteLicenceAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}

[ApiController]
[Route("api/[controller]")]
public class TenantsController : ControllerBase
{
    private readonly ITenantService _tenantService;

    public TenantsController(ITenantService tenantService)
    {
        _tenantService = tenantService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TenantDto>>> GetTenants()
    {
        var tenants = await _tenantService.GetAllTenantsAsync();
        return Ok(tenants);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TenantDto>> GetTenant(int id)
    {
        var tenant = await _tenantService.GetTenantByIdAsync(id);
        if (tenant == null)
            return NotFound();

        return Ok(tenant);
    }

    [HttpGet("{id}/with-users")]
    public async Task<ActionResult<TenantDto>> GetTenantWithUsers(int id)
    {
        var tenant = await _tenantService.GetTenantWithUsersAsync(id);
        if (tenant == null)
            return NotFound();

        return Ok(tenant);
    }

    [HttpGet("by-licence/{licenceId}")]
    public async Task<ActionResult<IEnumerable<TenantDto>>> GetTenantsByLicence(int licenceId)
    {
        var tenants = await _tenantService.GetTenantsByLicenceAsync(licenceId);
        return Ok(tenants);
    }

    [HttpGet("{id}/user-count")]
    public async Task<ActionResult<int>> GetUserCountByTenant(int id)
    {
        var count = await _tenantService.GetUserCountByTenantAsync(id);
        return Ok(count);
    }

    [HttpPost]
    public async Task<ActionResult<TenantDto>> CreateTenant(CreateTenantDto createTenantDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var tenant = await _tenantService.CreateTenantAsync(createTenantDto);
            return CreatedAtAction(nameof(GetTenant), new { id = tenant.Id }, tenant);
        }
        catch (NotFoundException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<TenantDto>> UpdateTenant(int id, UpdateTenantDto updateTenantDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var tenant = await _tenantService.UpdateTenantAsync(id, updateTenantDto);
            return Ok(tenant);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteTenant(int id)
    {
        try
        {
            var result = await _tenantService.DeleteTenantAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}

// 16. Dependency Injection Configuration (Program.cs)
/*
// Database
builder.Services.AddDbContext<AuthDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Identity
builder.Services.AddIdentity<ApplicationUser, Role>(options =>
{
    // Password settings
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;

    // Lockout settings
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // User settings
    options.User.AllowedUserNameCharacters =
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<AuthDbContext>()
.AddDefaultTokenProviders();

// AutoMapper
builder.Services.AddAutoMapper(typeof(AuthMappingProfile));

// Unit of Work
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Repositories
builder.Services.AddScoped<ILicenceRepository, LicenceRepository>();
builder.Services.AddScoped<ITenantRepository, TenantRepository>();
builder.Services.AddScoped<IApplicationUserRepository, ApplicationUserRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IPermissionRepository, PermissionRepository>();
builder.Services.AddScoped<IUserRefreshTokenRepository, UserRefreshTokenRepository>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

// Services
builder.Services.AddScoped<ILicenceService, LicenceService>();
builder.Services.AddScoped<ITenantService, TenantService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IPermissionService, PermissionService>();

// Logging
builder.Services.AddLogging();

// Controllers
builder.Services.AddControllers();

// JWT Authentication (if needed)
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

// Authorization
builder.Services.AddAuthorization();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
*/

// 17. Example Usage Patterns

// Multi-tenant aware service method example
public class MultiTenantOrderService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public MultiTenantOrderService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<List<Order>> GetOrdersForCurrentTenantAsync()
    {
        var currentTenantId = GetCurrentTenantId();

        // Use the generic repository for entities not in specific repositories
        var orders = await _unitOfWork.Repository<Order>()
            .FindAsync(o => o.TenantId == currentTenantId);

        return orders.ToList();
    }

    private int GetCurrentTenantId()
    {
        // Extract tenant ID from current user claims
        var tenantIdClaim = _httpContextAccessor.HttpContext?.User
            .FindFirst("TenantId")?.Value;

        return int.Parse(tenantIdClaim ?? "0");
    }
}

// Permission-based authorization service
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
        return await _unitOfWork.Permissions.UserHasPermissionAsync(userId, permissionName);
    }

    public async Task<List<string>> GetUserPermissionsAsync(int userId)
    {
        // Get direct user permissions
        var directPermissions = await _unitOfWork.Permissions.GetPermissionsByUserAsync(userId);

        // Get role-based permissions
        var userRoles = await _unitOfWork.Roles.GetUserRolesAsync(userId);
        var rolePermissions = new List<Permission>();

        foreach (var role in userRoles)
        {
            var permissions = await _unitOfWork.Permissions.GetPermissionsByRoleAsync(role.Id);
            rolePermissions.AddRange(permissions);
        }

        // Combine and deduplicate
        var allPermissions = directPermissions.Concat(rolePermissions)
            .Distinct()
            .Select(p => p.PermissionName)
            .ToList();

        return allPermissions;
    }
}< Permission > (entity =>
{
    entity.HasKey(e => e.Id);
    entity.Property(e => e.PermissionName).IsRequired().HasMaxLength(100);
    entity.Property(e => e.Resource).IsRequired().HasMaxLength(100);
    entity.Property(e => e.Action).IsRequired();
    entity.Property(e => e.Description).HasMaxLength(500);
    entity.Property(e => e.IsActive).HasDefaultValue(true);
    entity.Property(e => e.IsDeleted).HasDefaultValue(false);

    // Global query filter for soft delete
    entity.HasQueryFilter(p => !p.IsDeleted);
});

modelBuilder.Entity