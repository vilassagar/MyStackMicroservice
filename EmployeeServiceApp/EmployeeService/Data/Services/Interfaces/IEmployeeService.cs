using EmployeeService.Models.Dtos;
using EmployeeService.Models.Requests;

namespace EmployeeService.Data.Services.Interfaces
{
    public interface IEmployeeService
    {
        Task<EmployeeDto?> GetByIdAsync(int id);
        Task<EmployeeDto?> GetByEmployeeCodeAsync(string employeeCode);
        Task<EmployeeDto?> GetByEmailAsync(string email);
        Task<IEnumerable<EmployeeDto>> GetAllAsync();
        Task<IEnumerable<EmployeeDto>> GetByDepartmentAsync(int departmentId);
        Task<IEnumerable<EmployeeDto>> GetByManagerAsync(int managerId);
        Task<(IEnumerable<EmployeeDto> employees, int totalCount)> GetFilteredAsync(EmployeeFilterRequest filter);
        Task<EmployeeDto> CreateAsync(CreateEmployeeDto createDto, string createdBy);
        Task<EmployeeDto> UpdateAsync(int id, UpdateEmployeeDto updateDto, string updatedBy);
        Task<bool> DeleteAsync(int id, string deletedBy);
        Task<IEnumerable<EmployeeBasicDto>> GetSubordinatesAsync(int managerId);
        Task<EmployeeDto> GetWithAllDetailsAsync(int id);
        Task<bool> TransferEmployeeAsync(int employeeId, int newDepartmentId, int? newManagerId, string transferredBy);
    }
}
