using AuthService.DomainModel;
using AuthService.enums;
using AuthService.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Data
{
    public class AuthDbContext : IdentityDbContext<ApplicationUser, Role, int>
    {
        private readonly DatabaseConfiguration _databaseConfig;
        private readonly List<object> _pendingChanges = new List<object>();
        public AuthDbContext(DbContextOptions<AuthDbContext> options, DatabaseConfiguration databaseConfig)
            : base(options)
        {
            _databaseConfig = databaseConfig;
        }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<UserRefreshToken> UserRefreshTokens { get; set; }
        public DbSet<UserPasswordHistory> UserPasswordHistories { get; set; }
        public DbSet<UserTwoFactor> UserTwoFactors { get; set; }
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


            base.OnModelCreating(builder);

            // Apply configurations
            builder.ApplyConfiguration(new LicenceConfiguration());
            builder.ApplyConfiguration(new TenantConfiguration());
            builder.ApplyConfiguration(new UserConfiguration());
            builder.ApplyConfiguration(new RoleConfiguration());
            builder.ApplyConfiguration(new UserRoleConfiguration());

            // Configure RolePermission
            builder.Entity<RolePermission>(entity =>
            {
                entity.HasKey(rp => new { rp.RoleId, rp.PermissionId });
                entity.ToTable("role_permission");

                entity.HasOne(rp => rp.Role)
                      .WithMany(r => r.RolePermissions)
                      .HasForeignKey(rp => rp.RoleId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(rp => rp.Permission)
                      .WithMany(p => p.RolePermissions)
                      .HasForeignKey(rp => rp.PermissionId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure UserPermission
            builder.Entity<UserPermission>(entity =>
            {
                entity.HasKey(up => new { up.UserId, up.PermissionId });
                entity.ToTable("user_permission");

                entity.HasOne(up => up.User)
                      .WithMany()
                      .HasForeignKey(up => up.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(up => up.Permission)
                      .WithMany(p => p.UserPermissions)
                      .HasForeignKey(up => up.PermissionId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure other entities
            builder.Entity<UserRefreshToken>(entity =>
            {
                entity.ToTable("user_refresh_tokens");
                entity.HasOne(urt => urt.User)
                      .WithMany()
                      .HasForeignKey(urt => urt.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<UserPasswordHistory>(entity =>
            {
                entity.ToTable("user_password_history");
                entity.HasOne(uph => uph.User)
                      .WithMany()
                      .HasForeignKey(uph => uph.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<UserTwoFactor>(entity =>
            {
                entity.ToTable("user_two_factor");
                entity.HasOne(utf => utf.User)
                      .WithMany()
                      .HasForeignKey(utf => utf.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<UserLoginHistory>(entity =>
            {
                entity.ToTable("user_login_history");
                entity.HasOne(ulh => ulh.User)
                      .WithMany()
                      .HasForeignKey(ulh => ulh.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<UserSession>(entity =>
            {
                entity.ToTable("user_sessions");
                entity.HasOne(us => us.User)
                      .WithMany()
                      .HasForeignKey(us => us.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure RolePermissionHistory
            builder.Entity<RolePermissionHistory>(entity =>
            {
                entity.HasKey(rph => new { rph.RoleId, rph.PermissionId, rph.GrantedAt });
                entity.ToTable("role_permission_history");
            });
            ConfigureAdditionalEntities(builder);
        }
       
        private void ConfigureAdditionalEntities(ModelBuilder builder)
        {
            builder.Entity<UserRefreshToken>(entity =>
            {
                entity.ToTable("user_refresh_tokens");
                entity.HasOne(urt => urt.User)
                      .WithMany()
                      .HasForeignKey(urt => urt.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<UserPasswordHistory>(entity =>
            {
                entity.ToTable("user_password_history");
                entity.HasOne(uph => uph.User)
                      .WithMany()
                      .HasForeignKey(uph => uph.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<UserTwoFactor>(entity =>
            {
                entity.ToTable("user_two_factor");
                entity.HasOne(utf => utf.User)
                      .WithMany()
                      .HasForeignKey(utf => utf.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<UserLoginHistory>(entity =>
            {
                entity.ToTable("user_login_history");
                entity.HasOne(ulh => ulh.User)
                      .WithMany()
                      .HasForeignKey(ulh => ulh.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<UserSession>(entity =>
            {
                entity.ToTable("user_sessions");
                entity.HasOne(us => us.User)
                      .WithMany()
                      .HasForeignKey(us => us.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }

        // Override SaveChanges to prevent commits in testing
        public override int SaveChanges()
        {
            if (_databaseConfig?.Environment == ExecutionEnvironment.Testing && _databaseConfig.PreventCommitInTesting)
            {
                return SimulateSaveChanges();
            }

            return base.SaveChanges();
        }
        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            if (_databaseConfig?.Environment == ExecutionEnvironment.Testing && _databaseConfig.PreventCommitInTesting)
            {
                return SimulateSaveChanges();
            }

            return base.SaveChanges(acceptAllChangesOnSuccess);
        }
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            if (_databaseConfig?.Environment == ExecutionEnvironment.Testing && _databaseConfig.PreventCommitInTesting)
            {
                return await SimulateSaveChangesAsync();
            }

            return await base.SaveChangesAsync(cancellationToken);
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            if (_databaseConfig?.Environment == ExecutionEnvironment.Testing && _databaseConfig.PreventCommitInTesting)
            {
                return await SimulateSaveChangesAsync();
            }

            return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        // Method to force save even in testing (for setup data)
        public int ForceSaveChanges()
        {
            return base.SaveChanges();
        }

        public async Task<int> ForceSaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await base.SaveChangesAsync(cancellationToken);
        }

        private int SimulateSaveChanges()
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added ||
                           e.State == EntityState.Modified ||
                           e.State == EntityState.Deleted)
                .ToList();

            int affectedRows = 0;

            foreach (var entry in entries)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        // Simulate auto-generated ID for new entities
                        AssignTemporaryId(entry);
                        entry.State = EntityState.Unchanged; // Mark as if saved
                        affectedRows++;
                        break;

                    case EntityState.Modified:
                        entry.State = EntityState.Unchanged; // Mark as if saved
                        affectedRows++;
                        break;

                    case EntityState.Deleted:
                        entry.State = EntityState.Detached; // Remove from context
                        affectedRows++;
                        break;
                }
            }

            return affectedRows;
        }

        private async Task<int> SimulateSaveChangesAsync()
        {
            // For async, we can simulate some delay if needed
            await Task.Delay(1); // Minimal delay to simulate async operation
            return SimulateSaveChanges();
        }

        private void AssignTemporaryId(Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry entry)
        {
            // Generate temporary IDs for testing
            var idProperty = entry.Properties.FirstOrDefault(p => p.Metadata.IsPrimaryKey() && p.Metadata.ClrType == typeof(int));
            if (idProperty != null && (int)idProperty.CurrentValue <= 0)
            {
                // Generate a temporary ID for testing (negative numbers to avoid conflicts)
                var tempId = -Random.Shared.Next(1000, 999999);
                idProperty.CurrentValue = tempId;
            }
        }
    }
}
