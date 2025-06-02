using AuthService.DomainModel;
using System.ComponentModel.DataAnnotations;

namespace AuthService.Dtos
{

    public class PermissionDto
    {
        public int Id { get; set; }
        public string PermissionName { get; set; } = string.Empty;
        public string Resource { get; set; } = string.Empty;
        public PermissionAction Action { get; set; }
        public string ActionName => Action.ToString();
        public string? Description { get; set; }
        public bool IsActive { get; set; }
    }
    public class CreatePermissionDto
    {
        [Required]
        [StringLength(100)]
        public string PermissionName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Resource { get; set; } = string.Empty;

        [Required]
        public PermissionAction Action { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }
    }

    public class UpdatePermissionDto
    {
        [StringLength(100)]
        public string? PermissionName { get; set; }

        [StringLength(100)]
        public string? Resource { get; set; }

        public PermissionAction? Action { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        public bool? IsActive { get; set; }
    }
}
