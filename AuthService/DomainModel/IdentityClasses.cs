using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

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
        public ICollection<UserPasswordHistory> UserPasswordHistories { get; set; }
        public ICollection<UserLoginHistory> UserLoginHistories { get; set; }
        public ICollection<UserSession> UserLoginSessions { get; set; }
        public ICollection<UserRefreshToken> UserRefreshTokens { get; set; } = new List<UserRefreshToken>();
        public ICollection<UserPermission> UserPermissions { get; set; } = new List<UserPermission>();
    }

    [Table("roles")]
    public class Role : IdentityRole<int>, IAuditableEntity
    {
        public string Description { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
        public ICollection<RolePermission> RolePermissions { get; set; }=new List<RolePermission>();
        public ICollection<RolePermissionHistory> RolePermissionHistories { get; set; } = new List<RolePermissionHistory>();

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
  

    [Table("user_roles")]
    public class UserRole : IEntity
    {
        [Key]
        public  int Id { get; set; }
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public ApplicationUser User { get; set; }
        public Role Role { get; set; }
    }

   

    [Table("user_claims")]
    public class UserClaim : IdentityUserClaim<int>, IEntity
    {
        public override int UserId { get; set; }
        public ApplicationUser User { get; set; }
    }

 
    [Table("user_logins")]
    public class UserLogin :IdentityUserLogin<int> ,IEntity
    {      
        public  int Id { get; set; }
        public override int UserId { get; set; }
        public ApplicationUser User { get; set; }
    }


    [Table("user_tokens")]
    public class UserToken : IdentityUserToken<int>, IEntity
    {
       
        public  int Id { get; set; }
        public override int UserId { get; set; }
        public ApplicationUser User { get; set; }
    }

    [Table("user_refresh_tokens")]
    public class UserRefreshToken: IEntity
    {
        
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Token { get; set; }
        public DateTime ExpiresAt { get; set; }
        public ApplicationUser User { get; set; }
    }

    [Table("user_password_history")]
    public class UserPasswordHistory: IEntity
    {
        
        public int Id { get; set; }
        public int UserId { get; set; }
        public string PasswordHash { get; set; }
        public DateTime CreatedAt { get; set; }
        public ApplicationUser User { get; set; }
    }

    [Table("user_login_history")]
    public class UserLoginHistory: IEntity
    {
      
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime LoginTime { get; set; }
        public string IpAddress { get; set; }
        public ApplicationUser User { get; set; }
    }

    [Table("user_sessions")]
    public class UserSession: IEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string SessionToken { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
        public ApplicationUser User { get; set; }
    }

    [Table("permission")]
    public class Permission : BaseAuditableEntity, ISoftDeletable
    {
      
        public string PermissionName { get; set; } // e.g., "Users", "Orders", "Reports"
        public string Resource { get; set; } // e.g., "User", "Order", "Report" 
        public PermissionAction Action { get; set; } // Read, Write, Update, Delete
        public string? Description { get; set; }        
     
        public virtual ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
        public virtual ICollection<UserPermission> UserPermissions { get; set; } = new List<UserPermission>();
        public ICollection<RolePermissionHistory> RolePermissionHistories { get; set; } = new List<RolePermissionHistory>();
    }

    [Table("role_permission")]
    public class RolePermission: IAuditableEntity, ISoftDeletable
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public int PermissionId { get; set; }
        public DateTime GrantedAt { get; set; } = DateTime.UtcNow;
        public string? GrantedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsDeleted { get; set; } = false;
        // Navigation properties
        public virtual Role Role { get; set; }
        public virtual Permission Permission { get; set; }
    }

    [Table("role_permission_history")]
    public class RolePermissionHistory: IEntity
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public int PermissionId { get; set; }
        public DateTime GrantedAt { get; set; } = DateTime.UtcNow;
        public string? GrantedBy { get; set; }

        // Navigation properties
        public virtual Role Role { get; set; }
        public virtual Permission Permission { get; set; }
        
    }

    [Table("user_permission")]
    public class UserPermission: IEntity, ISoftDeletable
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int PermissionId { get; set; }
        public DateTime GrantedAt { get; set; } = DateTime.UtcNow;
        public string? GrantedBy { get; set; }
        public bool IsDeleted { get; set; } = false;
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
