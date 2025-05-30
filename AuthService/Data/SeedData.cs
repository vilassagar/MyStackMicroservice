using AuthService.DomainModel;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Data
{
    public class SeedData
    {
        public void SeedTestData(ModelBuilder builder)
        {
            // Seed test permissions
            var permissions = new List<Permission>
        {
            new Permission { Id = 1, PermissionName = "User Management", Resource = "User", Action = PermissionAction.Read, IsActive = true },
            new Permission { Id = 2, PermissionName = "User Management", Resource = "User", Action = PermissionAction.Write, IsActive = true },
            new Permission { Id = 3, PermissionName = "User Management", Resource = "User", Action = PermissionAction.Update, IsActive = true },
            new Permission { Id = 4, PermissionName = "User Management", Resource = "User", Action = PermissionAction.Delete, IsActive = true },
            new Permission { Id = 5, PermissionName = "Order Management", Resource = "Order", Action = PermissionAction.Read, IsActive = true },
            new Permission { Id = 6, PermissionName = "Order Management", Resource = "Order", Action = PermissionAction.Write, IsActive = true }
        };
            builder.Entity<Permission>().HasData(permissions);

            // Seed test roles
            builder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "Admin", NormalizedName = "ADMIN", Description = "Administrator Role", ConcurrencyStamp = Guid.NewGuid().ToString() },
                new Role { Id = 2, Name = "User", NormalizedName = "USER", Description = "Regular User Role", ConcurrencyStamp = Guid.NewGuid().ToString() },
                new Role { Id = 3, Name = "Manager", NormalizedName = "MANAGER", Description = "Manager Role", ConcurrencyStamp = Guid.NewGuid().ToString() }
            );

            // Seed role permissions
            builder.Entity<RolePermission>().HasData(
                new RolePermission { RoleId = 1, PermissionId = 1, GrantedAt = DateTime.UtcNow, GrantedBy = "System" },
                new RolePermission { RoleId = 1, PermissionId = 2, GrantedAt = DateTime.UtcNow, GrantedBy = "System" },
                new RolePermission { RoleId = 1, PermissionId = 3, GrantedAt = DateTime.UtcNow, GrantedBy = "System" },
                new RolePermission { RoleId = 1, PermissionId = 4, GrantedAt = DateTime.UtcNow, GrantedBy = "System" },
                new RolePermission { RoleId = 2, PermissionId = 1, GrantedAt = DateTime.UtcNow, GrantedBy = "System" },
                new RolePermission { RoleId = 3, PermissionId = 1, GrantedAt = DateTime.UtcNow, GrantedBy = "System" },
                new RolePermission { RoleId = 3, PermissionId = 2, GrantedAt = DateTime.UtcNow, GrantedBy = "System" },
                new RolePermission { RoleId = 3, PermissionId = 3, GrantedAt = DateTime.UtcNow, GrantedBy = "System" }
            );
        }
    }

}
