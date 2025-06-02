using AuthService.DomainModel;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AuthService.Controllers
{
    public class LicenceService : ILicenceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<LicenceService> _logger;

        public LicenceService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<LicenceService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<LicenceDto>> GetAllLicencesAsync()
        {
            try
            {
                var licences = await _unitOfWork.Licences.GetLicencesWithTenantsAsync();
                return _mapper.Map<IEnumerable<LicenceDto>>(licences);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all licences");
                throw;
            }
        }

        public async Task<LicenceDto?> GetLicenceByIdAsync(int id)
        {
            try
            {
                var licence = await _unitOfWork.Licences.GetByIdAsync(id);
                return licence != null ? _mapper.Map<LicenceDto>(licence) : null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting licence with ID {LicenceId}", id);
                throw;
            }
        }

        public async Task<LicenceDto?> GetLicenceWithTenantsAsync(int id)
        {
            try
            {
                var licence = await _unitOfWork.Licences.GetLicenceWithTenantsAsync(id);
                return licence != null ? _mapper.Map<LicenceDto>(licence) : null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting licence with tenants for ID {LicenceId}", id);
                throw;
            }
        }

        public async Task<LicenceDto> CreateLicenceAsync(CreateLicenceDto createLicenceDto)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var licence = _mapper.Map<Licence>(createLicenceDto);
                await _unitOfWork.Licences.AddAsync(licence);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                _logger.LogInformation("Licence created with ID {LicenceId}", licence.Id);
                return _mapper.Map<LicenceDto>(licence);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Error occurred while creating licence");
                throw;
            }
        }

        public async Task<LicenceDto> UpdateLicenceAsync(int id, UpdateLicenceDto updateLicenceDto)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var existingLicence = await _unitOfWork.Licences.GetByIdAsync(id);
                if (existingLicence == null)
                {
                    throw new NotFoundException($"Licence with ID {id} not found");
                }

                // Validate NumberOfUser decrease doesn't violate current usage
                if (updateLicenceDto.NumberOfUser.HasValue)
                {
                    var currentUsage = await GetCurrentUserCountAsync(id);
                    if (updateLicenceDto.NumberOfUser.Value < currentUsage)
                    {
                        throw new InvalidOperationException($"Cannot reduce user limit below current usage ({currentUsage})");
                    }
                }

                _mapper.Map(updateLicenceDto, existingLicence);
                await _unitOfWork.Licences.UpdateAsync(existingLicence);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                _logger.LogInformation("Licence updated with ID {LicenceId}", id);
                return _mapper.Map<LicenceDto>(existingLicence);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Error occurred while updating licence with ID {LicenceId}", id);
                throw;
            }
        }

        public async Task<bool> DeleteLicenceAsync(int id)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var licence = await _unitOfWork.Licences.GetLicenceWithTenantsAsync(id);
                if (licence == null)
                {
                    return false;
                }

                // Check if licence has tenants
                if (licence.Tenant.Any())
                {
                    throw new InvalidOperationException("Cannot delete licence that has tenants");
                }

                await _unitOfWork.Licences.DeleteAsync(licence);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                _logger.LogInformation("Licence deleted with ID {LicenceId}", id);
                return true;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Error occurred while deleting licence with ID {LicenceId}", id);
                throw;
            }
        }

        public async Task<bool> HasAvailableUserSlotsAsync(int licenceId, int requestedSlots)
        {
            try
            {
                return await _unitOfWork.Licences.HasAvailableUserSlotsAsync(licenceId, requestedSlots);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while checking available slots for licence {LicenceId}", licenceId);
                throw;
            }
        }

        private async Task<int> GetCurrentUserCountAsync(int licenceId)
        {
            var tenants = await _unitOfWork.Tenants.GetTenantsByLicenceAsync(licenceId);
            return tenants.Sum(t => t.Users.Count);
        }
    }

}
