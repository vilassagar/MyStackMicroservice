using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeService.Models.DomainModel
{
    public class Position
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;

        [StringLength(10)]
        public string Code { get; set; } = string.Empty;

        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;

        public int DepartmentId { get; set; }

        [StringLength(20)]
        public string Level { get; set; } = string.Empty; // Junior, Mid, Senior, Lead, Manager

        [Column(TypeName = "decimal(18,2)")]
        public decimal MinSalary { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal MaxSalary { get; set; }

        public string RequiredSkills { get; set; } = string.Empty; // JSON array

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public virtual Department Department { get; set; } = null!;
        public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}
