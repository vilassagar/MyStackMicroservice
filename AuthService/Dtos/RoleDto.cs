using AuthService.DomainModel;
using System.ComponentModel.DataAnnotations;

namespace AuthService.Dtos
{
    public class RoleDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<PermissionDto> Permissions { get; set; } = new();
        public int UserCount { get; set; }
    }
    public class CreateRoleDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        public List<int> PermissionIds { get; set; } = new();
    }
    public class UpdateRoleDto
    {
        [StringLength(100)]
        public string? Name { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        public List<int>? PermissionIds { get; set; }
    }

}
