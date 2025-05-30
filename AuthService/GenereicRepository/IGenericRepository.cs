using AuthService.DomainModel;
using AuthService.Model;
using System.Dynamic;
using System.Linq.Expressions;

namespace AuthService.GenereicRepository
{
    public interface IGenericRepository<TEntity> where TEntity : class, IEntity
    {
        // Pagination methods
        //Task<PagedResult<TEntity>> GetPagedAsync(PaginationParameters parameters);
        //Task<PagedResult<TEntity>> GetPagedAsync(
        //    int pageNumber,
        //    int pageSize,
        //    Expression<Func<TEntity, bool>>? filter = null,
        //    Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        //    string? includeProperties = null);

        //// Count methods
        //Task<int> CountAsync();
        //Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate);

        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity?> GetByIdAsync(int id);
        Task<TEntity?> GetByIdAsync(int id, params Expression<Func<TEntity, object>>[] includes);
        Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);
        Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes);
        Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes);
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);  
        Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> AddAsync(TEntity entity);
        Task<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> entities);
        Task<TEntity> UpdateAsync(TEntity entity);
        Task<TEntity> DeleteAsync(int id);
        Task<TEntity> DeleteAsync(TEntity entity);
        Task<IEnumerable<TEntity>> DeleteRangeAsync(IEnumerable<TEntity> entities);
        IQueryable<TEntity> Query();
        IQueryable<TEntity> QueryNoTracking();
        Task<IEnumerable<TEntity>> GetPagedAsync(int pageNumber, int pageSize);
        Task<IEnumerable<TEntity>> GetPagedAsync(Expression<Func<TEntity, bool>> predicate, int pageNumber, int pageSize);

    }
}
