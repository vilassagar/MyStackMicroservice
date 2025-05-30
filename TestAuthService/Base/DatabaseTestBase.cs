using AuthService.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace TestAuthService.Base
{
    public abstract class DatabaseTestBase : IDisposable
    {
        protected readonly AuthContext _context;
        protected readonly IServiceProvider _serviceProvider;
        protected readonly DatabaseConfiguration _databaseConfig;

        protected DatabaseTestBase()
        {
            // Setup test services
            var services = new ServiceCollection();
            var configuration = BuildTestConfiguration();

            // Configure for testing environment with SQL Server
            AuthContextFactory.ConfigureDbContext(services, configuration, ExecutionEnvironment.Testing);

            // Register additional services
            ConfigureTestServices(services);

            _serviceProvider = services.BuildServiceProvider();
            _context = _serviceProvider.GetRequiredService<AuthContext>();
            _databaseConfig = _serviceProvider.GetRequiredService<DatabaseConfiguration>();

            // Ensure database exists and seed test data
            InitializeDatabase();
        }

        private IConfiguration BuildTestConfiguration()
        {
            return new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                {"ConnectionStrings:TestConnection", "Server=(localdb)\\mssqllocaldb;Database=AuthServiceTest;Trusted_Connection=true;MultipleActiveResultSets=true"},
                {"ConnectionStrings:DefaultConnection", "Server=(localdb)\\mssqllocaldb;Database=AuthServiceDev;Trusted_Connection=true;MultipleActiveResultSets=true"}
                })
                .Build();
        }

        private void ConfigureTestServices(IServiceCollection services)
        {
            // Register AutoMapper
            services.AddAutoMapper(typeof(UserMappingProfile));

            // Register repositories
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUserRepository, UserRepository>();

            // Register services
            services.AddScoped<IPermissionService, PermissionService>();
            services.AddScoped<IUserService, UserService>();

            // Add logging for testing
            services.AddLogging(builder => builder.AddConsole());
        }

        private void InitializeDatabase()
        {
            // Ensure database exists
            _context.Database.EnsureCreated();

            // Clear existing test data
            CleanupTestData();

            // Seed fresh test data using ForceSaveChanges
            SeedTestData();
        }

        private void CleanupTestData()
        {
            // Remove any existing test data
            var testUsers = _context.ApplicationUsers
                .Where(u => u.Email.Contains("@test.com"))
                .ToList();

            if (testUsers.Any())
            {
                _context.ApplicationUsers.RemoveRange(testUsers);
                _context.ForceSaveChanges(); // Force commit cleanup
            }
        }

        protected virtual void SeedTestData()
        {
            var userManager = _serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = _serviceProvider.GetRequiredService<RoleManager<Role>>();

            // Ensure roles exist
            EnsureRolesExist(roleManager);

            // Ensure permissions exist
            EnsurePermissionsExist();

            // Create test users
            CreateTestUsers(userManager);
        }

        private void EnsureRolesExist(RoleManager<Role> roleManager)
        {
            var roles = new[] { "Admin", "User", "Manager" };

            foreach (var roleName in roles)
            {
                if (!roleManager.RoleExistsAsync(roleName).Result)
                {
                    var role = new Role
                    {
                        Name = roleName,
                        Description = $"{roleName} Role"
                    };
                    roleManager.CreateAsync(role).Wait();
                }
            }
        }

        private void EnsurePermissionsExist()
        {
            if (!_context.Permissions.Any())
            {
                var permissions = new List<Permission>
            {
                new Permission { PermissionName = "User Management", Resource = "User", Action = PermissionAction.Read, IsActive = true },
                new Permission { PermissionName = "User Management", Resource = "User", Action = PermissionAction.Write, IsActive = true },
                new Permission { PermissionName = "User Management", Resource = "User", Action = PermissionAction.Update, IsActive = true },
                new Permission { PermissionName = "User Management", Resource = "User", Action = PermissionAction.Delete, IsActive = true },
                new Permission { PermissionName = "Order Management", Resource = "Order", Action = PermissionAction.Read, IsActive = true },
                new Permission { PermissionName = "Order Management", Resource = "Order", Action = PermissionAction.Write, IsActive = true }
            };

                _context.Permissions.AddRange(permissions);
                _context.ForceSaveChanges(); // Force commit permissions
            }
        }

        private void CreateTestUsers(UserManager<ApplicationUser> userManager)
        {
            var testUsers = new List<(ApplicationUser user, string password, List<string> roles)>
        {
            (new ApplicationUser
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@test.com",
                UserName = "john.doe",
                Address = "123 Test St",
                EmailConfirmed = true
            }, "TestPass123!", new List<string> { "Admin" }),

            (new ApplicationUser
            {
                FirstName = "Jane",
                LastName = "Smith",
                Email = "jane.smith@test.com",
                UserName = "jane.smith",
                Address = "456 Test Ave",
                EmailConfirmed = true
            }, "TestPass123!", new List<string> { "User" }),

            (new ApplicationUser
            {
                FirstName = "Bob",
                LastName = "Manager",
                Email = "bob.manager@test.com",
                UserName = "bob.manager",
                Address = "789 Manager Blvd",
                EmailConfirmed = true
            }, "TestPass123!", new List<string> { "Manager" })
        };

            foreach (var (user, password, roles) in testUsers)
            {
                // Check if user already exists
                var existingUser = userManager.FindByEmailAsync(user.Email).Result;
                if (existingUser == null)
                {
                    var result = userManager.CreateAsync(user, password).Result;
                    if (result.Succeeded)
                    {
                        userManager.AddToRolesAsync(user, roles).Wait();
                    }
                }
            }
        }

        public void Dispose()
        {
            // No need for transaction rollback since SaveChanges is overridden
            // Just clean up test data
            CleanupTestData();

            _context?.Dispose();

            if (_serviceProvider is IDisposable disposableServiceProvider)
            {
                disposableServiceProvider.Dispose();
            }
        }
    }
}
