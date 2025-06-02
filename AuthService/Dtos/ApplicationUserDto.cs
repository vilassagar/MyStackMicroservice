using AuthService.DomainModel;
using System.ComponentModel.DataAnnotations;

namespace AuthService.Dtos
{
    public class ApplicationUserDto
    {
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FullName => $"{FirstName} {LastName}".Trim();
        public int TenantId { get; set; }
        public string TenantName { get; set; } = string.Empty;
        public UserStatus Status { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public List<string> Roles { get; set; } = new();
        public List<string> Permissions { get; set; } = new();
    }
    public class UpdateUserDto
    {
        [StringLength(100)]
        public string? UserName { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        [StringLength(100)]
        public string? FirstName { get; set; }

        [StringLength(100)]
        public string? LastName { get; set; }

        public int? TenantId { get; set; }

        public UserStatus? Status { get; set; }

        public List<string>? Roles { get; set; }
    }

}
