using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeService.Models.DomainModel
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string EmployeeCode { get; set; } = string.Empty;

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
        [StringLength(255)]
        public string Email { get; set; } = string.Empty;

        [Phone]
        [StringLength(20)]
        public string? PhoneNumber { get; set; }

        [StringLength(20)]
        public string? AlternatePhoneNumber { get; set; }

        public DateTime DateOfBirth { get; set; }

        [Required]
        [StringLength(10)]
        public string Gender { get; set; } = string.Empty; // Male, Female, Other

        [StringLength(20)]
        public string MaritalStatus { get; set; } = string.Empty;

        public DateTime DateOfJoining { get; set; }

        public DateTime? DateOfLeaving { get; set; }

        [Required]
        [StringLength(20)]
        public string EmploymentStatus { get; set; } = "Active"; // Active, Inactive, Terminated

        [Required]
        [StringLength(30)]
        public string EmploymentType { get; set; } = string.Empty; // Full-time, Part-time, Contract, Intern

        // Foreign Keys
        public int DepartmentId { get; set; }
        public int PositionId { get; set; }
        public int? ManagerId { get; set; }

        // Salary Information
        [Column(TypeName = "decimal(18,2)")]
        public decimal BaseSalary { get; set; }

        [StringLength(10)]
        public string Currency { get; set; } = "USD";

        // Emergency Contact
        [StringLength(100)]
        public string? EmergencyContactName { get; set; }

        [StringLength(20)]
        public string? EmergencyContactPhone { get; set; }

        [StringLength(100)]
        public string? EmergencyContactRelation { get; set; }

        // System Fields
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public string CreatedBy { get; set; } = string.Empty;
        public string UpdatedBy { get; set; } = string.Empty;
        public bool IsDeleted { get; set; } = false;

        // Navigation Properties
        public virtual Department Department { get; set; } = null!;
        public virtual Position Position { get; set; } = null!;
        public virtual Employee? Manager { get; set; }
        public virtual ICollection<Employee> Subordinates { get; set; } = new List<Employee>();
        public virtual ICollection<EmployeeAddress> Addresses { get; set; } = new List<EmployeeAddress>();

        // Computed Properties
        [NotMapped]
        public string FullName => $"{FirstName} {MiddleName} {LastName}".Trim();

        [NotMapped]
        public int YearsOfService => DateTime.UtcNow.Year - DateOfJoining.Year;

        [NotMapped]
        public bool IsManager => Subordinates.Any();
    }
}
