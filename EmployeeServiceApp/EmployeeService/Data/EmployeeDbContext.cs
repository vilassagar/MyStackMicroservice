using EmployeeService.Models.DomainModel;
using Microsoft.EntityFrameworkCore;

namespace EmployeeService.Data
{
    public class EmployeeDbContext : DbContext
    {
        public EmployeeDbContext(DbContextOptions<EmployeeDbContext> options) : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Ensure the correct namespace and extension methods are available
            modelBuilder.Entity<Employee>().ToTable("Employees"); // Fix: Ensure the correct namespace is imported
            modelBuilder.Entity<Department>().ToTable("Departments");

            // Configure relationships, indexes, etc. as needed
            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Department)
                .WithMany(d => d.Employees)
                .HasForeignKey(e => e.DepartmentId);
        }
    }
