using HRMS.Employee.API.Models.DTOs;
using System.ComponentModel.DataAnnotations;

namespace EmployeeService.Models.Dtos
{
    public class EmployeeDto
    {
        public int Id { get; set; }
        public string EmployeeCode { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string MiddleName { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; } = string.Empty;
        public string MaritalStatus { get; set; } = string.Empty;
        public DateTime DateOfJoining { get; set; }
        public DateTime? DateOfLeaving { get; set; }
        public string EmploymentStatus { get; set; } = string.Empty;
        public string EmploymentType { get; set; } = string.Empty;
        public decimal BaseSalary { get; set; }
        public string Currency { get; set; } = string.Empty;
        public int YearsOfService { get; set; }

        // Department and Position Info
        public DepartmentDto Department { get; set; } = null!;
        public PositionDto Position { get; set; } = null!;
        public EmployeeBasicDto? Manager { get; set; }

        // Emergency Contact
        public string? EmergencyContactName { get; set; }
        public string? EmergencyContactPhone { get; set; }
        public string? EmergencyContactRelation { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class EmployeeBasicDto
    {
        public int Id { get; set; }
        public string EmployeeCode { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
    }

    public class CreateEmployeeDto
    {
        [Required]
        [StringLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string LastName { get; set; } = string.Empty;

        [StringLength(100)]
        public string MiddleName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Phone]
        public string? PhoneNumber { get; set; }

        public string? AlternatePhoneNumber { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public string Gender { get; set; } = string.Empty;

        public string MaritalStatus { get; set; } = string.Empty;

        [Required]
        public DateTime DateOfJoining { get; set; }

        [Required]
        public string EmploymentType { get; set; } = string.Empty;

        [Required]
        public int DepartmentId { get; set; }

        [Required]
        public int PositionId { get; set; }

        public int? ManagerId { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal BaseSalary { get; set; }

        public string Currency { get; set; } = "USD";

        // Emergency Contact
        public string? EmergencyContactName { get; set; }
        public string? EmergencyContactPhone { get; set; }
        public string? EmergencyContactRelation { get; set; }

        // Address Information
        public CreateEmployeeAddressDto? Address { get; set; }
    }

    public class UpdateEmployeeDto
    {
        [StringLength(100)]
        public string? FirstName { get; set; }

        [StringLength(100)]
        public string? LastName { get; set; }

        [StringLength(100)]
        public string? MiddleName { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        [Phone]
        public string? PhoneNumber { get; set; }

        public string? AlternatePhoneNumber { get; set; }

        public string? MaritalStatus { get; set; }

        public string? EmploymentType { get; set; }

        public int? DepartmentId { get; set; }

        public int? PositionId { get; set; }

        public int? ManagerId { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? BaseSalary { get; set; }

        // Emergency Contact
        public string? EmergencyContactName { get; set; }
        public string? EmergencyContactPhone { get; set; }
        public string? EmergencyContactRelation { get; set; }
    }
}
