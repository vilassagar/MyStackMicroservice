using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace AuthService.DomainModel
{
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

    //public class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    //{
    //    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    //    {
    //        // Each User can have many UserClaims
    //        builder.HasMany(e => e.Claims)
    //            .WithOne(e => e.User)
    //            .HasForeignKey(uc => uc.UserId)
    //            .IsRequired();

    //        // Each User can have many UserLogins
    //        builder.HasMany(e => e.UserLogins)
    //            .WithOne(e => e.User)
    //            .HasForeignKey(ul => ul.UserId)
    //            .IsRequired();

    //        // Each User can have many UserTokens
    //        builder.HasMany(e => e.UserTokens)
    //            .WithOne(e => e.User)
    //            .HasForeignKey(ut => ut.UserId)
    //            .IsRequired();

    //        // Each User can have many entries in the UserRole join table
    //        builder.HasMany(e => e.UserRoles)
    //            .WithOne(e => e.User)
    //            .HasForeignKey(ur => ur.UserId)
    //            .IsRequired();

    //        builder.HasOne(u => u.Tenant)
    //       .WithMany()
    //       .HasForeignKey(u => u.TenantId)
    //       .OnDelete(DeleteBehavior.Restrict);
    //    }
    //}
    public class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.ToTable("users"); // Add this line

            // Configure the Tenant relationship first
            builder.HasOne(u => u.Tenant)
                .WithMany(t => t.Users) // Change from WithMany() to WithMany(t => t.Users)
                .HasForeignKey(u => u.TenantId)
                .OnDelete(DeleteBehavior.Restrict);

            // Rest of your configurations...
            builder.HasMany(e => e.Claims)
                .WithOne(e => e.User)
                .HasForeignKey(uc => uc.UserId)
                .IsRequired();

            builder.HasMany(e => e.UserLogins)
                .WithOne(e => e.User)
                .HasForeignKey(ul => ul.UserId)
                .IsRequired();

            builder.HasMany(e => e.UserTokens)
                .WithOne(e => e.User)
                .HasForeignKey(ut => ut.UserId)
                .IsRequired();

            builder.HasMany(e => e.UserRoles)
                .WithOne(e => e.User)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();
        }
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

    public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.ToTable("user_roles");
            builder.HasKey(ur => ur.Id);

            // Also add a unique constraint on the combination of UserId and RoleId
            builder.HasIndex(ur => new { ur.UserId, ur.RoleId })
                .IsUnique();

            // Configure navigation properties
            builder.HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();

            builder.HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();
        }
    }
    public class UserClaimConfiguration : IEntityTypeConfiguration<UserClaim>
    {
        public void Configure(EntityTypeBuilder<UserClaim> builder)
        {
            builder.ToTable("user_claims");
           
            // Configure navigation properties
            builder.HasOne(uc => uc.User)
                .WithMany(u => u.Claims)
                .HasForeignKey(uc => uc.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction); // Prevent cascade delete
        }
    }

    public class UserLoginConfiguration : IEntityTypeConfiguration<UserLogin>
    {
        public void Configure(EntityTypeBuilder<UserLogin> builder)
        {
            builder.ToTable("user_logins");
           // builder.HasKey(ul => new { ul.LoginProvider, ul.ProviderKey, ul.UserId });
            // Configure navigation properties
            builder.HasOne(ul => ul.User)
                .WithMany(u => u.UserLogins)
                .HasForeignKey(ul => ul.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction); // Prevent cascade delete
        }
    }

    public class UserTokenConfiguration : IEntityTypeConfiguration<UserToken>
    {
        public void Configure(EntityTypeBuilder<UserToken> builder)
        {
            builder.ToTable("user_tokens");
           // builder.HasKey(ut => new { ut.UserId, ut.LoginProvider, ut.Name });
            // Configure navigation properties
            builder.HasOne(ut => ut.User)
                .WithMany(u => u.UserTokens)
                .HasForeignKey(ut => ut.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction); // Prevent cascade delete
        }
    }

    public class UserPasswordHistoryConfiguration : IEntityTypeConfiguration<UserPasswordHistory>
    {
        public void Configure(EntityTypeBuilder<UserPasswordHistory> builder)
        {
            builder.ToTable("user_password_history");
            builder.HasKey(uph => uph.Id);
            builder.Property(uph => uph.PasswordHash).IsRequired();
            builder.Property(uph => uph.CreatedAt).IsRequired();
            // Configure navigation properties
            builder.HasOne(uph => uph.User)
                .WithMany(u => u.UserPasswordHistories)
                .HasForeignKey(uph => uph.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade); // Allow cascade delete
        }
    }

    public class UserRefreshTokenConfiguration : IEntityTypeConfiguration<UserRefreshToken>
    {
        public void Configure(EntityTypeBuilder<UserRefreshToken> builder)
        {
            builder.ToTable("user_refresh_tokens");
            builder.HasKey(urt => urt.Id);
            builder.Property(urt => urt.Token).IsRequired().HasMaxLength(500);
            builder.Property(urt => urt.ExpiresAt).IsRequired();
            // Configure navigation properties
            builder.HasOne(urt => urt.User)
               .WithMany(u => u.UserRefreshTokens)
                .HasForeignKey(urt => urt.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade); // Allow cascade delete
        }
    }

    public class UserLoginHistoryConfiguration : IEntityTypeConfiguration<UserLoginHistory>
    {
        public void Configure(EntityTypeBuilder<UserLoginHistory> builder)
        {
            builder.ToTable("user_login_history");
            builder.HasKey(ulh => ulh.Id);
            // Configure navigation properties
            builder.HasOne(ulh => ulh.User)
                .WithMany(u => u.UserLoginHistories)
                .HasForeignKey(ulh => ulh.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade); // Allow cascade delete
        }
    }
    public class UserSessionConfiguration : IEntityTypeConfiguration<UserSession>
    {
        public void Configure(EntityTypeBuilder<UserSession> builder)
        {
            builder.ToTable("user_sessions");
            builder.HasKey(ulh => ulh.Id);
            // Configure navigation properties
            builder.HasOne(ulh => ulh.User)
                .WithMany(u => u.UserLoginSessions)
                .HasForeignKey(ulh => ulh.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade); // Allow cascade delete
        }

    }

    public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.ToTable("permissions");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.PermissionName).IsRequired().HasMaxLength(100);
            builder.Property(p => p.Resource).IsRequired().HasMaxLength(100); 
            builder.Property(p => p.Description).HasMaxLength(500);
            builder.Property(p=>p.IsDeleted).HasDefaultValue(true);
            builder.Property(p => p.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

            // Configure navigation properties if needed
        }
    }

    public class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
    {
        public void Configure(EntityTypeBuilder<RolePermission> builder)
        {
            builder.ToTable("role_permissions");

            // Use the Id as primary key (since RolePermission has an Id property)
            builder.HasKey(rp => rp.Id);

            // Add unique constraint for the combination instead
            builder.HasIndex(rp => new { rp.RoleId, rp.PermissionId })
                .IsUnique();

            // Configure navigation properties
            builder.HasOne(rp => rp.Role)
                .WithMany(r => r.RolePermissions)
                .HasForeignKey(rp => rp.RoleId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(rp => rp.Permission)
                .WithMany(p => p.RolePermissions) // Change from WithMany() to WithMany(p => p.RolePermissions)
                .HasForeignKey(rp => rp.PermissionId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

    //public class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
    //{
    //    public void Configure(EntityTypeBuilder<RolePermission> builder)
    //    {
    //        builder.ToTable("role_permissions");
    //        builder.HasKey(rp => new { rp.RoleId, rp.PermissionId });
    //        // Configure navigation properties
    //        builder.HasOne(rp => rp.Role)
    //            .WithMany(r => r.RolePermissions)
    //            .HasForeignKey(rp => rp.RoleId)
    //            .IsRequired()
    //            .OnDelete(DeleteBehavior.Cascade); // Allow cascade delete
    //        builder.HasOne(rp => rp.Permission)
    //            .WithMany()
    //            .HasForeignKey(rp => rp.PermissionId)
    //            .IsRequired()
    //            .OnDelete(DeleteBehavior.Cascade); // Allow cascade delete
    //    }
    //}

    public class UserPermissionConfiguration : IEntityTypeConfiguration<UserPermission>
    {
        public void Configure(EntityTypeBuilder<UserPermission> builder)
        {
            builder.ToTable("user_permissions");

            // Use the Id as primary key
            builder.HasKey(up => up.Id);

            // Add unique constraint
            builder.HasIndex(up => new { up.UserId, up.PermissionId })
                .IsUnique();

            // Configure navigation properties
            builder.HasOne(up => up.User)
                .WithMany(u => u.UserPermissions)
                .HasForeignKey(up => up.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(up => up.Permission)
                .WithMany(p => p.UserPermissions) // Change from WithMany() to WithMany(p => p.UserPermissions)
                .HasForeignKey(up => up.PermissionId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
    public class RolePermissionHistoryConfiguration : IEntityTypeConfiguration<RolePermissionHistory>
    {
        public void Configure(EntityTypeBuilder<RolePermissionHistory> builder)
        {
            builder.ToTable("role_permission_history");
            builder.HasKey(rph => rph.Id);
            builder.Property(rph => rph.GrantedAt).IsRequired();
            builder.Property(rph => rph.GrantedBy).HasMaxLength(100);
            // Configure navigation properties
            builder.HasOne(rph => rph.Role)
               .WithMany(u => u.RolePermissionHistories)
                .HasForeignKey(rph => rph.RoleId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade); // Allow cascade delete
            builder.HasOne(rph => rph.Permission)
               .WithMany(u => u.RolePermissionHistories)
                .HasForeignKey(rph => rph.PermissionId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade); // Allow cascade delete
        }
    }

}
