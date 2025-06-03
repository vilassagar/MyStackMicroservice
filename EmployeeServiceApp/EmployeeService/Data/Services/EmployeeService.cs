using AutoMapper;
using EmployeeService.Data.Repository.Interface;
using EmployeeService.Data.Services.Interfaces;
using EmployeeService.InfrastructureLayer.Caching;
using EmployeeService.InfrastructureLayer.Messaging;
using EmployeeService.Models.Dtos;
using EmployeeService.Models.Requests;

namespace EmployeeService.Data.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IMapper _mapper;
        private readonly IEventPublisher _eventPublisher;
        private readonly ICacheService _cacheService;
        private readonly ILogger<EmployeeService> _logger;

        public EmployeeService(
            IEmployeeRepository employeeRepository,
            IDepartmentRepository departmentRepository,
            IMapper mapper,
            IEventPublisher eventPublisher,
            ICacheService cacheService,
            ILogger<EmployeeService> logger)
        {
            _employeeRepository = employeeRepository;
            _departmentRepository = departmentRepository;
            _mapper = mapper;
            _eventPublisher = eventPublisher;
            _cacheService = cacheService;
            _logger = logger;
        }

        public async Task<EmployeeDto?> GetByIdAsync(int id)
        {
            var cacheKey = $"employee_{id}";

            var cachedEmployee = await _cacheService.GetAsync<EmployeeDto>(cacheKey);
            if (cachedEmployee != null)
            {
                return cachedEmployee;
            }

            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee == null)
                return null;

            var employeeDto = _mapper.Map<EmployeeDto>(employee);

            // Cache for 30 minutes
            await _cacheService.SetAsync(cacheKey, employeeDto, TimeSpan.FromMinutes(30));

            return employeeDto;
        }

        public async Task<EmployeeDto?> GetByEmployeeCodeAsync(string employeeCode)
        {
            var employee = await _employeeRepository.GetByEmployeeCodeAsync(employeeCode);
            return employee == null ? null : _mapper.Map<EmployeeDto>(employee);
        }

        public async Task<EmployeeDto?> GetByEmailAsync(string email)
        {
            var employee = await _employeeRepository.GetByEmailAsync(email);
            return employee == null ? null : _mapper.Map<EmployeeDto>(employee);
        }

        public async Task<IEnumerable<EmployeeDto>> GetAllAsync()
        {
            var employees = await _employeeRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<EmployeeDto>>(employees);
        }

        public async Task<IEnumerable<EmployeeDto>> GetByDepartmentAsync(int departmentId)
        {
            var employees = await _employeeRepository.GetByDepartmentAsync(departmentId);
            return _mapper.Map<IEnumerable<EmployeeDto>>(employees);
        }

        public async Task<IEnumerable<EmployeeDto>> GetByManagerAsync(int managerId)
        {
            var employees = await _employeeRepository.GetByManagerAsync(managerId);
            return _mapper.Map<IEnumerable<EmployeeDto>>(employees);
        }

        public async Task<(IEnumerable<EmployeeDto> employees, int totalCount)> GetFilteredAsync(EmployeeFilterRequest filter)
        {
            var (employees, totalCount) = await _employeeRepository.GetFilteredAsync(filter);
            var employeeDtos = _mapper.Map<IEnumerable<EmployeeDto>>(employees);

            return (employeeDtos, totalCount);
        }

        public Task<EmployeeDto> CreateAsync(CreateEmployeeDto createDto, string createdBy)
        {
            throw new NotImplementedException();
        }

        public Task<EmployeeDto> UpdateAsync(int id, UpdateEmployeeDto updateDto, string updatedBy)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id, string deletedBy)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<EmployeeBasicDto>> GetSubordinatesAsync(int managerId)
        {
            throw new NotImplementedException();
        }

        public Task<EmployeeDto> GetWithAllDetailsAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> TransferEmployeeAsync(int employeeId, int newDepartmentId, int? newManagerId, string transferredBy)
        {
            throw new NotImplementedException();
        }
    }
}
