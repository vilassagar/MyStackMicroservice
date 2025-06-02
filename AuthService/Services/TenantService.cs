using AuthService.DomainModel;
using AutoMapper;

namespace AuthService.Services
{

    public class TenantService : ITenantService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<TenantService> _logger;

        public TenantService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<TenantService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<TenantDto>> GetAllTenantsAsync()
        {
            try
            {
                var tenants = await _unitOfWork.Tenants.GetTenantsWithUserCountAsync();
                return _mapper.Map<IEnumerable<TenantDto>>(tenants);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all tenants");
                throw;
            }
        }

        public async Task<TenantDto?> GetTenantByIdAsync(int id)
        {
            try
            {
                var tenant = await _unitOfWork.Tenants.GetByIdAsync(id);
                return tenant != null ? _mapper.Map<TenantDto>(tenant) : null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting tenant with ID {TenantId}", id);
                throw;
            }
        }

        public async Task<TenantDto?> GetTenantWithUsersAsync(int id)
        {
            try
            {
                var tenant = await _unitOfWork.Tenants.GetTenantWithUsersAsync(id);
                return tenant != null ? _mapper.Map<TenantDto>(tenant) : null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting tenant with users for ID {TenantId}", id);
                throw;
            }
        }

        public async Task<IEnumerable<TenantDto>> GetTenantsByLicenceAsync(int licenceId)
        {
            try
            {
                var tenants = await _unitOfWork.Tenants.GetTenantsByLicenceAsync(licenceId);
                return _mapper.Map<IEnumerable<TenantDto>>(tenants);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting tenants for licence {LicenceId}", licenceId);
                throw;
            }
        }

        public async Task<TenantDto> CreateTenantAsync(CreateTenantDto createTenantDto)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                // Validate licence exists
                var licenceExists = await _unitOfWork.Licences.AnyAsync(l => l.Id == createTenantDto.LicenceId);
                if (!licenceExists)
                {
                    throw new NotFoundException($"Licence with ID {createTenantDto.LicenceId} not found");
                }

                var tenant = _mapper.Map<Tenant>(createTenantDto);
                await _unitOfWork.Tenants.AddAsync(tenant);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                _logger.LogInformation("Tenant created with ID {TenantId}", tenant.Id);
                return _mapper.Map<TenantDto>(tenant);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Error occurred while creating tenant");
                throw;
            }
        }

        public async Task<TenantDto> UpdateTenantAsync(int id, UpdateTenantDto updateTenantDto)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var existingTenant = await _unitOfWork.Tenants.GetByIdAsync(id);
                if (existingTenant == null)
                {
                    throw new NotFoundException($"Tenant with ID {id} not found");
                }

                // Validate licence if provided
                if (updateTenantDto.LicenceId.HasValue)
                {
                    var licenceExists = await _unitOfWork.Licences.AnyAsync(l => l.Id == updateTenantDto.LicenceId.Value);
                    if (!licenceExists)
                    {
                        throw new NotFoundException($"Licence with ID {updateTenantDto.LicenceId} not found");
                    }
                }

                _mapper.Map(updateTenantDto, existingTenant);
                await _unitOfWork.Tenants.UpdateAsync(existingTenant);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                _logger.LogInformation("Tenant updated with ID {TenantId}", id);
                return _mapper.Map<TenantDto>(existingTenant);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Error occurred while updating tenant with ID {TenantId}", id);
                throw;
            }
        }

        public async Task<bool> DeleteTenantAsync(int id)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var tenant = await _unitOfWork.Tenants.GetTenantWithUsersAsync(id);
                if (tenant == null)
                {
                    return false;
                }

                // Check if tenant has users
                if (tenant.Users.Any())
                {
                    throw new InvalidOperationException("Cannot delete tenant that has users");
                }

                await _unitOfWork.Tenants.DeleteAsync(tenant);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                _logger.LogInformation("Tenant deleted with ID {TenantId}", id);
                return true;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Error occurred while deleting tenant with ID {TenantId}", id);
                throw;
            }
        }

        public async Task<int> GetUserCountByTenantAsync(int tenantId)
        {
            try
            {
                return await _unitOfWork.Tenants.GetUserCountByTenantAsync(tenantId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting user count for tenant {TenantId}", tenantId);
                throw;
            }
        }
    }

}
