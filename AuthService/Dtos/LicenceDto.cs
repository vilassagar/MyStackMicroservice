using System.ComponentModel.DataAnnotations;

namespace AuthService.Dtos
{
    public class LicenceDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int NumberOfUser { get; set; }
        public int CurrentUserCount { get; set; }
        public int AvailableSlots { get; set; }
        public List<TenantDto> Tenants { get; set; } = new();
    }

    public class CreateLicenceDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Range(1, int.MaxValue)]
        public int NumberOfUser { get; set; }
    }
    public class UpdateLicenceDto
    {
        [StringLength(100)]
        public string? Name { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        [Range(1, int.MaxValue)]
        public int? NumberOfUser { get; set; }
    }
}
