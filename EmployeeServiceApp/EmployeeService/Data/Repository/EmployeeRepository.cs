using EmployeeService.Data.GenereicRepository;
using EmployeeService.Data.Repository.Interface;
using EmployeeService.Models.DomainModel;
using EmployeeService.Models.Requests;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EmployeeService.Data.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly EmployeeDbContext _context;
        private readonly ILogger<EmployeeRepository> _logger;

        public EmployeeRepository(EmployeeDbContext context, ILogger<EmployeeRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Employee?> GetByIdAsync(int id)
        {
            return await _context.Employees
                .Include(e => e.Department)
                .Include(e => e.Position)
                .Include(e => e.Manager)
                .FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted);
        }

        public async Task<Employee?> GetWithAllDetailsAsync(int id)
        {
            return await _context.Employees
                .Include(e => e.Department)
                .Include(e => e.Position)
                .Include(e => e.Manager)
                .Include(e => e.Subordinates.Where(s => !s.IsDeleted))
                .Include(e => e.Addresses)
                .FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted);
        }

        public async Task<Employee?> GetByEmployeeCodeAsync(string employeeCode)
        {
            return await _context.Employees
                .Include(e => e.Department)
                .Include(e => e.Position)
                .Include(e => e.Manager)
                .FirstOrDefaultAsync(e => e.EmployeeCode == employeeCode && !e.IsDeleted);
        }

        public async Task<Employee?> GetByEmailAsync(string email)
        {
            return await _context.Employees
                .Include(e => e.Department)
                .Include(e => e.Position)
                .FirstOrDefaultAsync(e => e.Email == email && !e.IsDeleted);
        }

        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            return await _context.Employees
                .Include(e => e.Department)
                .Include(e => e.Position)
                .Include(e => e.Manager)
                .Where(e => !e.IsDeleted)
                .OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName)
                .ToListAsync();
        }

        public async Task<IEnumerable<Employee>> GetByDepartmentAsync(int departmentId)
        {
            return await _context.Employees
                .Include(e => e.Department)
                .Include(e => e.Position)
                .Include(e => e.Manager)
                .Where(e => e.DepartmentId == departmentId && !e.IsDeleted)
                .OrderBy(e => e.FirstName)
                .ToListAsync();
        }

        public async Task<IEnumerable<Employee>> GetByManagerAsync(int managerId)
        {
            return await _context.Employees
                .Include(e => e.Department)
                .Include(e => e.Position)
                .Where(e => e.ManagerId == managerId && !e.IsDeleted)
                .OrderBy(e => e.FirstName)
                .ToListAsync();
        }

       
        //public async Task<(IEnumerable<Employee> employees, int totalCount)> GetFilteredAsync(EmployeeFilterRequest filter)
        //{
        //    var query = _context.Employees
        //        .Include(e => e.Department)
        //        .Include(e => e.Position)
        //        .Include(e => e.Manager)
        //        .Where(e => !e.IsDeleted);

        //    // Apply filters
        //    if (!string.IsNullOrEmpty(filter.SearchTerm))
        //    {
        //        var searchTerm = filter.SearchTerm.ToLower();
        //        query = query.Where(e =>
        //            e.FirstName.ToLower().Contains(searchTerm) ||
        //            e.LastName.ToLower().Contains(searchTerm) ||
        //            e.Email.ToLower().Contains(searchTerm) ||
        //            e.EmployeeCode.ToLower().Contains(searchTerm));
        //    }

        //    if (filter.DepartmentId.HasValue)
        //    {
        //        query = query.Where(e => e.DepartmentId == filter.DepartmentId.Value);
        //    }

        //    if (!string.IsNullOrEmpty(filter.EmploymentStatus))
        //    {
        //        query = query.Where(e => e.EmploymentStatus == filter.EmploymentStatus);
        //    }

        //    if (!string.IsNullOrEmpty(filter.EmploymentType))
        //    {
        //        query = query.Where(e => e.EmploymentType == filter.EmploymentType);
        //    }

        //    if (filter.ManagerId.HasValue)
        //    {
        //        query = query.Where(e => e.ManagerId == filter.ManagerId.Value);
        //    }

        //    if (filter.MinSalary.HasValue)
        //    {
        //        query = query.Where(e => e.BaseSalary >= filter.MinSalary.Value);
        //    }

        //    if (filter.MaxSalary.HasValue)
        //    {
        //        query = query.Where(e => e.BaseSalary <= filter.MaxSalary.Value);
        //    }

        //    // Get total count before pagination
        //    var totalCount = await query.CountAsync();

        //    // Apply sorting
        //    query = filter.SortBy?.ToLower() switch
        //    {
        //        "name" => filter.SortDescending ?
        //            query.OrderByDescending(e => e.FirstName).ThenByDescending(e => e.LastName) :
        //            query.OrderBy(e => e.FirstName).ThenBy(e => e.LastName),
        //        "email" => filter.SortDescending ?
        //            query.OrderByDescending(e => e.Email) :
        //            query.OrderBy(e => e.Email),
        //        "department" => filter.SortDescending ?
        //            query.OrderByDescending(e => e.Department.Name) :
        //            query.OrderBy(e => e.Department.Name),
        //        "joindate" => filter.SortDescending ?
        //            query.OrderByDescending(e => e.DateOfJoining) :
        //            query.OrderBy(e => e.DateOfJoining),
        //        _ => query.OrderBy(e => e.FirstName).ThenBy(e => e.LastName)
        //    };

        //    // Apply pagination
        //    if (filter.Page > 0 && filter.PageSize > 0)
        //    {
        //        query = query.Skip((filter.Page - 1) * filter.PageSize).Take(filter.PageSize);
        //    }

        //    var employees = await query.ToListAsync();

        //    return (employees, totalCount);
        //}

        public async Task<Employee> CreateAsync(Employee employee)
        {
            if (string.IsNullOrEmpty(employee.EmployeeCode))
            {
                employee.EmployeeCode = await GenerateEmployeeCodeAsync();
            }

            employee.CreatedAt = DateTime.UtcNow;
            employee.UpdatedAt = DateTime.UtcNow;

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            return await GetByIdAsync(employee.Id) ?? employee;
        }

        public async Task<Employee> UpdateAsync(Employee employee)
        {
            employee.UpdatedAt = DateTime.UtcNow;

            _context.Employees.Update(employee);
            await _context.SaveChangesAsync();

            return await GetByIdAsync(employee.Id) ?? employee;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null || employee.IsDeleted)
                return false;

            // Soft delete
            employee.IsDeleted = true;
            employee.UpdatedAt = DateTime.UtcNow;
            employee.EmploymentStatus = "Terminated";

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Employees.AnyAsync(e => e.Id == id && !e.IsDeleted);
        }

        public async Task<bool> EmailExistsAsync(string email, int? excludeId = null)
        {
            var query = _context.Employees.Where(e => e.Email == email && !e.IsDeleted);

            if (excludeId.HasValue)
            {
                query = query.Where(e => e.Id != excludeId.Value);
            }

            return await query.AnyAsync();
        }

        public async Task<string> GenerateEmployeeCodeAsync()
        {
            var currentYear = DateTime.UtcNow.Year.ToString()[2..];
            var prefix = $"EMP{currentYear}";

            var lastEmployee = await _context.Employees
                .Where(e => e.EmployeeCode.StartsWith(prefix))
                .OrderByDescending(e => e.EmployeeCode)
                .FirstOrDefaultAsync();

            if (lastEmployee == null)
            {
                return $"{prefix}0001";
            }

            var lastNumberStr = lastEmployee.EmployeeCode[prefix.Length..];
            if (int.TryParse(lastNumberStr, out var lastNumber))
            {
                return $"{prefix}{(lastNumber + 1):D4}";
            }

            return $"{prefix}0001";
        }

        public async Task<IEnumerable<Employee>> GetSubordinatesAsync(int managerId)
        {
            return await _context.Employees
                .Include(e => e.Department)
                .Include(e => e.Position)
                .Where(e => e.ManagerId == managerId && !e.IsDeleted)
                .OrderBy(e => e.FirstName)
                .ToListAsync();
        }

        Task<IEnumerable<IEntity>> IGenericRepository<IEntity>.GetAllAsync()
        {
            throw new NotImplementedException();
        }

        Task<IEntity?> IGenericRepository<IEntity>.GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEntity?> GetByIdAsync(int id, params Expression<Func<IEntity, object>>[] includes)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IEntity>> FindAsync(Expression<Func<IEntity, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IEntity>> FindAsync(Expression<Func<IEntity, bool>> predicate, params Expression<Func<IEntity, object>>[] includes)
        {
            throw new NotImplementedException();
        }

        public Task<IEntity?> FirstOrDefaultAsync(Expression<Func<IEntity, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<IEntity?> FirstOrDefaultAsync(Expression<Func<IEntity, bool>> predicate, params Expression<Func<IEntity, object>>[] includes)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AnyAsync(Expression<Func<IEntity, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<int> CountAsync(Expression<Func<IEntity, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<IEntity> AddAsync(IEntity entity)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IEntity>> AddRangeAsync(IEnumerable<IEntity> entities)
        {
            throw new NotImplementedException();
        }

        public Task<IEntity> UpdateAsync(IEntity entity)
        {
            throw new NotImplementedException();
        }

        Task<IEntity> IGenericRepository<IEntity>.DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEntity> DeleteAsync(IEntity entity)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IEntity>> DeleteRangeAsync(IEnumerable<IEntity> entities)
        {
            throw new NotImplementedException();
        }

        public IQueryable<IEntity> Query()
        {
            throw new NotImplementedException();
        }

        public IQueryable<IEntity> QueryNoTracking()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IEntity>> GetPagedAsync(int pageNumber, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IEntity>> GetPagedAsync(Expression<Func<IEntity, bool>> predicate, int pageNumber, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Task<(IEnumerable<Employee> employees, int totalCount)> GetFilteredAsync(EmployeeFilterRequest filter)
        {
            throw new NotImplementedException();
        }
    }
}
