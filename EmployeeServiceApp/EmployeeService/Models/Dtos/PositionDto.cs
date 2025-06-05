namespace EmployeeService.Models.Dtos
{
    public class PositionDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        
        // Navigation property for employees in the position
        public List<EmployeeDto> Employees { get; set; } = new List<EmployeeDto>();
    }
}