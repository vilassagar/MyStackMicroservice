using AuthService.DomainModel;
using AuthService.enums;
using AuthService.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Options;
using System.Reflection.Emit;

namespace AuthService.Data
{
    public class AuthDbContext : IdentityDbContext<ApplicationUser, Role, int>
    {
        IConfiguration _configuration;

        public AuthDbContext(DbContextOptions<AuthDbContext> options
            , IConfiguration databaseConfig)
            : base(options)
        {
            _configuration = databaseConfig;
        }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<UserRefreshToken> UserRefreshTokens { get; set; }
        public DbSet<UserPasswordHistory> UserPasswordHistories { get; set; }
        
        public DbSet<UserLoginHistory> UserLoginHistories { get; set; }
        public DbSet<UserSession> UserSessions { get; set; }
        public DbSet<UserClaim> UserClaims { get; set; }
        public DbSet<UserLogin> UserLogins { get; set; }
        public DbSet<UserToken> UserTokens { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<AuditLogDetail> AuditLogDetails { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<UserPermission> UserPermissions { get; set; }
        public DbSet<RolePermissionHistory> RolePermissionHistories { get; set; }
        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<Licence> Licences { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure on base type instead
            builder.Entity<IdentityUserClaim<int>>()
                .HasKey(uc => uc.Id);

            // Apply configurations
            builder.ApplyConfiguration(new LicenceConfiguration());
            builder.ApplyConfiguration(new TenantConfiguration());
            builder.ApplyConfiguration(new UserConfiguration());
            builder.ApplyConfiguration(new RoleConfiguration());
            builder.ApplyConfiguration(new UserRoleConfiguration());
            builder.ApplyConfiguration(new UserClaimConfiguration());
            builder.ApplyConfiguration(new UserLoginConfiguration());
            builder.ApplyConfiguration(new UserTokenConfiguration());
            builder.ApplyConfiguration(new UserPasswordHistoryConfiguration());
            builder.ApplyConfiguration(new UserRefreshTokenConfiguration());
            builder.ApplyConfiguration(new UserLoginHistoryConfiguration());
            builder.ApplyConfiguration(new UserSessionConfiguration());
            builder.ApplyConfiguration(new PermissionConfiguration());
            builder.ApplyConfiguration(new PermissionConfiguration());
            builder.ApplyConfiguration(new RolePermissionHistoryConfiguration());
            builder.ApplyConfiguration(new RolePermissionConfiguration());
        


        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.ConfigureWarnings(warnings =>
                warnings.Ignore(RelationalEventId.ForeignKeyPropertiesMappedToUnrelatedTables));
        }
      

    }
}
