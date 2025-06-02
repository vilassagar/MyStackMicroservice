using System.ComponentModel.DataAnnotations;

namespace AuthService.Dtos
{
    public class TenantDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int LicenceId { get; set; }
        public string LicenceName { get; set; } = string.Empty;
        public int UserCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<ApplicationUserDto> Users { get; set; } = new();
    }

    public class CreateTenantDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public int LicenceId { get; set; }
    }


    public class UpdateTenantDto
    {
        [StringLength(100)]
        public string? Name { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        public int? LicenceId { get; set; }
    }

}
