using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthService.DomainModel
{
    public class Licence : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int NumberOfUser { get; set; }
        public ICollection<Tenant> Tenant { get; set; } = new HashSet<Tenant>();
    }
    public class LicenceConfiguration : IEntityTypeConfiguration<Licence>
    {
        public void Configure(EntityTypeBuilder<Licence> builder)
        {
            builder.ToTable("licences");
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Name).IsRequired().HasMaxLength(100);
            builder.Property(t => t.Description).HasMaxLength(500);
            builder.Property(t => t.NumberOfUser).IsRequired();
            builder.HasMany(t => t.Tenant)
                .WithOne()
                .HasForeignKey(u => u.LicenceId)
                .IsRequired(false); // Optional relationship
        }

    }
    public class Tenant : IEntity, IAuditableEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int LicenceId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();
    }
    public class TenantConfiguration : IEntityTypeConfiguration<Tenant>
    {
        public void Configure(EntityTypeBuilder<Tenant> builder)
        {
            builder.ToTable("tenants");
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Name).IsRequired().HasMaxLength(100);
            builder.Property(t => t.Description).HasMaxLength(500);
            builder.HasMany(t => t.Users)
                .WithOne()
                .HasForeignKey(u => u.TenantId)
                .IsRequired(false); // Optional relationship
        }
    }


    [Table("users")]
    public class ApplicationUser : IdentityUser<int>, IEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int TenantId { get; set; }
        public UserStatus Status { get; set; } = UserStatus.Pending;
        public virtual Tenant Tenant { get; set; }

        public ICollection<UserClaim> Claims { get; set; }

        public ICollection<UserRole> UserRoles { get; set; }

        public ICollection<UserLogin> UserLogins { get; set; }

        public ICollection<UserToken> UserTokens { get; set; }
    }

    public class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            // Each User can have many UserClaims
            builder.HasMany(e => e.Claims)
                .WithOne(e => e.User)
                .HasForeignKey(uc => uc.UserId)
                .IsRequired();

            // Each User can have many UserLogins
            builder.HasMany(e => e.UserLogins)
                .WithOne(e => e.User)
                .HasForeignKey(ul => ul.UserId)
                .IsRequired();

            // Each User can have many UserTokens
            builder.HasMany(e => e.UserTokens)
                .WithOne(e => e.User)
                .HasForeignKey(ut => ut.UserId)
                .IsRequired();

            // Each User can have many entries in the UserRole join table
            builder.HasMany(e => e.UserRoles)
                .WithOne(e => e.User)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();
        }
    }

    [Table("roles")]
    public class Role : IdentityRole<int>
    {
        public string Description { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
        public ICollection<RolePermission> RolePermissions { get; set; }
    }
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("roles");

            // Each Role can have many entries in the UserRole join table
            builder.HasMany(e => e.UserRoles)
                .WithOne(e => e.Role)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();

            // Each Role can have many associated RoleClaims
            builder.HasMany(e => e.RolePermissions)
                .WithOne(e => e.Role)
                .HasForeignKey(rc => rc.RoleId)
                .IsRequired();
        }
    }

    [Table("user_roles")]
    public class UserRole : IdentityUserRole<int>
    {

        public ApplicationUser User { get; set; }
        public Role Role { get; set; }
    }

    public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.HasKey(x => new { x.UserId, x.RoleId });
        }
    }

    [Table("user_claims")]
    public class UserClaim : IdentityUserClaim<int>
    {
        public ApplicationUser User { get; set; }
    }

    [Table("user_logins")]
    public class UserLogin : IdentityUserLogin<int>
    {
        public ApplicationUser User { get; set; }
    }

    [Table("user_tokens")]
    public class UserToken : IdentityUserToken<int>
    {
        public ApplicationUser User { get; set; }
    }

    [Table("user_refresh_tokens")]
    public class UserRefreshToken
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Token { get; set; }
        public DateTime ExpirationDate { get; set; }
        public ApplicationUser User { get; set; }
    }

    [Table("user_password_history")]
    public class UserPasswordHistory
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string PasswordHash { get; set; }
        public DateTime CreatedAt { get; set; }
        public ApplicationUser User { get; set; }
    }

    [Table("user_two_factor")]
    public class UserTwoFactor
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string TwoFactorProvider { get; set; }
        public string TwoFactorKey { get; set; }
        public ApplicationUser User { get; set; }
    }

    [Table("user_login_history")]
    public class UserLoginHistory
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime LoginTime { get; set; }
        public string IpAddress { get; set; }
        public ApplicationUser User { get; set; }
    }

    [Table("user_sessions")]
    public class UserSession
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string SessionToken { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
        public ApplicationUser User { get; set; }
    }

    [Table("permission")]
    public class Permission : IEntity, ISoftDeletable
    {
        public int Id { get; set; }
        public string PermissionName { get; set; } // e.g., "Users", "Orders", "Reports"
        public string Resource { get; set; } // e.g., "User", "Order", "Report" 
        public PermissionAction Action { get; set; } // Read, Write, Update, Delete
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        // Fix: Should be ICollection, not single property
        public virtual ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
        public virtual ICollection<UserPermission> UserPermissions { get; set; } = new List<UserPermission>();
    }

    [Table("role_permission")]
    public class RolePermission
    {
        public int RoleId { get; set; }
        public int PermissionId { get; set; }
        public DateTime GrantedAt { get; set; } = DateTime.UtcNow;
        public string? GrantedBy { get; set; }

        // Navigation properties
        public virtual Role Role { get; set; }
        public virtual Permission Permission { get; set; }
    }

    [Table("role_permission_history")]
    public class RolePermissionHistory
    {
        public int RoleId { get; set; }
        public int PermissionId { get; set; }
        public DateTime GrantedAt { get; set; } = DateTime.UtcNow;
        public string? GrantedBy { get; set; }

        // Navigation properties
        public virtual Role Role { get; set; }
        public virtual Permission Permission { get; set; }
    }

    [Table("user_permission")]
    public class UserPermission
    {
        public int UserId { get; set; }
        public int PermissionId { get; set; }
        public DateTime GrantedAt { get; set; } = DateTime.UtcNow;
        public string? GrantedBy { get; set; }
        // Navigation properties
        public virtual ApplicationUser User { get; set; }
        public virtual Permission Permission { get; set; }
    }

    public enum PermissionType
    {
        UI,
        API,
        Data
    }
    public enum PermissionAction
    {
        Read = 1,
        Write = 2,
        Update = 4,
        Delete = 8,
        All = Read | Write | Update | Delete
    }
    public enum UserStatus
    {
        Inactive = 0,
        Active = 1,
        Suspended = 2,
        Deleted = 3,
        Pending = 4,
        Locked = 5,
        Unlocked = 6,
        Banned = 7,
        Verified = 8,
        Unverified = 9,
        PendingVerification = 10,
        PasswordResetRequired = 11,
        TwoFactorAuthenticationRequired = 12,
        EmailChangeRequired = 13,
        PhoneNumberChangeRequired = 14,
        AccountMergeRequired = 15,
        AccountSplitRequired = 16,
        AccountRecoveryRequired = 17,
        AccountDeactivationRequested = 18,
        AccountReactivationRequested = 19,
        AccountSuspensionRequested = 20,
        AccountDeletionRequested = 21,
        AccountRestorationRequested = 22,
        AccountLockRequested = 23,
        AccountUnlockRequested = 24,



    }


}
