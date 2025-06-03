

namespace EmployeeService.Models.DomainModel
{
    public interface IEntity
    {
        int Id { get; set; }
    }

    public interface IAuditableEntity : IEntity
    {
        DateTime CreatedAt { get; set; }
        DateTime UpdatedAt { get; set; }
    }
    public interface ISoftDeletable
    {
        bool IsDeleted { get; set; }
    }

}