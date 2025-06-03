using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeService.Models.DomainModel
{
    public class Department
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(10)]
        public string Code { get; set; } = string.Empty;

        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        public int? ParentDepartmentId { get; set; }

        public int? HeadOfDepartmentId { get; set; }

        [StringLength(100)]
        public string Location { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        public decimal? Budget { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public virtual Department? ParentDepartment { get; set; }
        public virtual ICollection<Department> SubDepartments { get; set; } = new List<Department>();
        public virtual Employee? HeadOfDepartment { get; set; }
        public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
        public virtual ICollection<Position> Positions { get; set; } = new List<Position>();

        // Computed Properties
        [NotMapped]
        public int EmployeeCount => Employees.Count(e => !e.IsDeleted && e.EmploymentStatus == "Active");
    }
}
