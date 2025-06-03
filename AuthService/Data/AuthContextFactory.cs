using AuthService.DomainModel;
using AuthService.enums;
using AuthService.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Data
{
    public static class AuthContextFactory
    {
        public static void ConfigureDbContext(IServiceCollection services, IConfiguration configuration, ExecutionEnvironment environment)
        {
            var databaseConfig = GetDatabaseConfiguration(configuration, environment);

            services.AddSingleton(databaseConfig);

            services.AddDbContext<AuthDbContext>(options =>
            {
                options.UseSqlServer(databaseConfig.ConnectionString);

                if (databaseConfig.EnableSensitiveDataLogging)
                {
                    options.EnableSensitiveDataLogging();
                }
            });

            ConfigureIdentity(services, environment);
        }

        private static DatabaseConfiguration GetDatabaseConfiguration(IConfiguration configuration, ExecutionEnvironment environment)
        {
            return environment switch
            {
                ExecutionEnvironment.Testing => new DatabaseConfiguration
                {
                    ConnectionString = configuration.GetConnectionString("TestConnection") ??
                                     configuration.GetConnectionString("DefaultConnection"),
                    Environment = ExecutionEnvironment.Testing,
                    EnableSensitiveDataLogging = true,
                    PreventCommitInTesting = true
                },
                ExecutionEnvironment.Development => new DatabaseConfiguration
                {
                    ConnectionString = configuration.GetConnectionString("DefaultConnection"),
                    Environment = ExecutionEnvironment.Development,
                    EnableSensitiveDataLogging = true,
                    PreventCommitInTesting = false
                },
                ExecutionEnvironment.Production => new DatabaseConfiguration
                {
                    ConnectionString = configuration.GetConnectionString("ProductionConnection"),
                    Environment = ExecutionEnvironment.Production,
                    EnableSensitiveDataLogging = false,
                    PreventCommitInTesting = false
                },
                ExecutionEnvironment.Staging => new DatabaseConfiguration
                {
                    ConnectionString = configuration.GetConnectionString("StagingConnection"),
                    Environment = ExecutionEnvironment.Staging,
                    EnableSensitiveDataLogging = false,
                    PreventCommitInTesting = false
                },
                _ => throw new ArgumentException($"Unsupported environment: {environment}")
            };
        }
        private static void ConfigureIdentity(IServiceCollection services, ExecutionEnvironment environment)
        {
            if (environment == ExecutionEnvironment.Testing)
            {
                // Relaxed password requirements for testing
                services.AddIdentity<ApplicationUser, Role>(options =>
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequiredLength = 3;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireLowercase = false;
                    options.User.RequireUniqueEmail = true;
                })
                .AddEntityFrameworkStores<AuthDbContext>()
                .AddDefaultTokenProviders();
            }
            else
            {
                // Production password requirements
                services.AddIdentity<ApplicationUser, Role>(options =>
                {
                    options.Password.RequireDigit = true;
                    options.Password.RequiredLength = 8;
                    options.Password.RequireNonAlphanumeric = true;
                    options.Password.RequireUppercase = true;
                    options.Password.RequireLowercase = true;
                    options.User.RequireUniqueEmail = true;
                })
                .AddEntityFrameworkStores<AuthDbContext>()
                .AddDefaultTokenProviders();
            }
        }
    }
}
