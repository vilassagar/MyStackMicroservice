using EmployeeService.Data.GenereicRepository;
using EmployeeService.Models.DomainModel;
using EmployeeService.Models.Requests;

namespace EmployeeService.Data.Repository.Interface
{
    public interface IEmployeeRepository: IGenericRepository<IEntity>
    {
       
        Task<Employee?> GetByEmployeeCodeAsync(string employeeCode);
        Task<Employee?> GetByEmailAsync(string email);

        Task<IEnumerable<Employee>> GetByDepartmentAsync(int departmentId);
        Task<IEnumerable<Employee>> GetByManagerAsync(int managerId);
        Task<(IEnumerable<Employee> employees, int totalCount)> GetFilteredAsync(EmployeeFilterRequest filter);
        Task<Employee> CreateAsync(Employee employee);
        Task<Employee> UpdateAsync(Employee employee);      
        Task<bool> ExistsAsync(int id);
        Task<bool> EmailExistsAsync(string email, int? excludeId = null);
        Task<string> GenerateEmployeeCodeAsync();
        Task<IEnumerable<Employee>> GetSubordinatesAsync(int managerId);+
        Task<Employee?> GetWithAllDetailsAsync(int id);
    }
}
