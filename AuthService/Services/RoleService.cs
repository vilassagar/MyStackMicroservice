using AuthService.Data.UnitofWorkPattern;
using AuthService.DomainModel;
using AuthService.Dtos;
using AuthService.Services.Interface;
using AutoMapper;

namespace AuthService.Services
{
    public class RoleService : IRoleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper; // Assuming you're using AutoMapper

        public RoleService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<RoleDto> CreateRoleAsync(CreateRoleDto createRoleDto)
        {
            try
            {
                // Check if role with same name already exists
                var existingRole = await _unitOfWork.RoleRepository.GetByNameAsync(createRoleDto.Name);
                if (existingRole != null)
                {
                    throw new InvalidOperationException($"Role with name '{createRoleDto.Name}' already exists.");
                }

                var role = _mapper.Map<Role>(createRoleDto);
                role.CreatedAt = DateTime.UtcNow;

                await _unitOfWork.RoleRepository.AddAsync(role);
                await _unitOfWork.SaveChangesAsync();

                return _mapper.Map<RoleDto>(role);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<RoleDto>> GetAllRolesAsync()
        {
            try
            {
                var roles = await _unitOfWork.RoleRepository.GetAllAsync();
                return _mapper.Map<IEnumerable<RoleDto>>(roles);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<RoleDto?> GetRoleByIdAsync(int id)
        {
            try
            {
                var role = await _unitOfWork.RoleRepository.GetByIdAsync(id);
                return role != null ? _mapper.Map<RoleDto>(role) : null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<RoleDto?> GetRoleByNameAsync(string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                    return null;

                var role = await _unitOfWork.RoleRepository.GetByNameAsync(name);
                return role != null ? _mapper.Map<RoleDto>(role) : null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<RoleDto?> GetRoleWithPermissionsAsync(int id)
        {
            try
            {
                var role = await _unitOfWork.RoleRepository.GetRoleWithPermissionsAsync(id);
                return role != null ? _mapper.Map<RoleDto>(role) : null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<RoleDto> UpdateRoleAsync(int id, UpdateRoleDto updateRoleDto)
        {
            try
            {
                var existingRole = await _unitOfWork.RoleRepository.GetByIdAsync(id);
                if (existingRole == null)
                {
                    throw new KeyNotFoundException($"Role with ID {id} not found.");
                }

                // Check if another role with the same name exists (excluding current role)
                if (!string.IsNullOrEmpty(updateRoleDto.Name) &&
                    updateRoleDto.Name != existingRole.Name)
                {
                    var roleWithSameName = await _unitOfWork.RoleRepository.GetByNameAsync(updateRoleDto.Name);
                    if (roleWithSameName != null && roleWithSameName.Id != id)
                    {
                        throw new InvalidOperationException($"Role with name '{updateRoleDto.Name}' already exists.");
                    }
                }

                _mapper.Map(updateRoleDto, existingRole);
                existingRole.UpdatedAt = DateTime.UtcNow;

               await _unitOfWork.RoleRepository.UpdateAsync(existingRole);
                await _unitOfWork.SaveChangesAsync();

                return _mapper.Map<RoleDto>(existingRole);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> DeleteRoleAsync(int id)
        {
            try
            {
                var role = await _unitOfWork.RoleRepository.GetByIdAsync(id);
                if (role == null)
                {
                    return false;
                }

                // Check if role is assigned to any users
                var usersWithRole = await _unitOfWork.RoleRepository.GetUsersByRoleIdAsync(id);
                if (usersWithRole.Any())
                {
                    throw new InvalidOperationException("Cannot delete role that is assigned to users.");
                }

               await _unitOfWork.RoleRepository.DeleteAsync(role);
                await _unitOfWork.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<RoleDto>> GetUserRolesAsync(int userId)
        {
            try
            {
                var roles = await _unitOfWork.RoleRepository.GetUserRolesAsync(userId);
                return _mapper.Map<IEnumerable<RoleDto>>(roles);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> AssignPermissionsToRoleAsync(int roleId, List<int> permissionIds)
        {
            try
            {
                if (permissionIds == null || !permissionIds.Any())
                    return false;

                var role = await _unitOfWork.RoleRepository.GetByIdAsync(roleId);
                if (role == null)
                {
                    throw new KeyNotFoundException($"Role with ID {roleId} not found.");
                }

                // Verify all permissions exist
                var permissions = await _unitOfWork.PermissionRepository.GetByIdsAsync(permissionIds);
                if (permissions.Count() != permissionIds.Count)
                {
                    throw new InvalidOperationException("One or more permission IDs are invalid.");
                }

                // Get existing role permissions to avoid duplicates
                var existingRolePermissions = await _unitOfWork.RoleRepository
                    .GetRolePermissionsAsync(roleId);

                var existingPermissionIds = existingRolePermissions.Select(rp => rp.PermissionId).ToList();
                var newPermissionIds = permissionIds.Except(existingPermissionIds).ToList();

                if (!newPermissionIds.Any())
                    return true; // All permissions already assigned

                // Create new role-permission relationships
                var rolePermissions = newPermissionIds.Select(permissionId => new RolePermission
                {
                    RoleId = roleId,
                    PermissionId = permissionId,
                    CreatedAt = DateTime.UtcNow
                }).ToList();

                await _unitOfWork.RolePermissionRepository.AddRangeAsync(rolePermissions);
                await _unitOfWork.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> RemovePermissionsFromRoleAsync(int roleId, List<int> permissionIds)
        {
            try
            {
                if (permissionIds == null || !permissionIds.Any())
                    return false;

                var role = await _unitOfWork.RoleRepository.GetByIdAsync(roleId);
                if (role == null)
                {
                    throw new KeyNotFoundException($"Role with ID {roleId} not found.");
                }

                var rolePermissions = await _unitOfWork.RolePermissionRepository
                    .GetRolePermissionsByIdsAsync(roleId, permissionIds);

                if (!rolePermissions.Any())
                    return true; // Permissions not assigned to role

               await _unitOfWork.RolePermissionRepository.DeleteRangeAsync(rolePermissions);
                await _unitOfWork.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

}
