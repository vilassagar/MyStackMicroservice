using EmployeeService.Models.DomainModel;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Security.Principal;

namespace EmployeeService.Data.GenereicRepository
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class, IEntity
    {
        protected readonly EmployeeDbContext _context;
        protected readonly DbSet<TEntity> _dbSet;

        public GenericRepository(EmployeeDbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }
        public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            IQueryable<TEntity> query = _dbSet;

            // Apply soft delete filter if entity supports it
            if (typeof(ISoftDeletable).IsAssignableFrom(typeof(TEntity)))
            {
                query = query.Where(e => !((ISoftDeletable)e).IsDeleted);
            }

            return await query.ToListAsync();
        }

        public virtual async Task<TEntity?> GetByIdAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);

            // Check soft delete
            if (entity is ISoftDeletable softDeletable && softDeletable.IsDeleted)
            {
                return null;
            }

            return entity;
        }

        public virtual async Task<TEntity?> GetByIdAsync(int id, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = _dbSet;

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            var entity = await query.FirstOrDefaultAsync(e => e.Id == id);

            // Check soft delete
            if (entity is ISoftDeletable softDeletable && softDeletable.IsDeleted)
            {
                return null;
            }

            return entity;
        }

        public virtual async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            IQueryable<TEntity> query = _dbSet.Where(predicate);

            // Apply soft delete filter if entity supports it
            if (typeof(ISoftDeletable).IsAssignableFrom(typeof(TEntity)))
            {
                query = query.Where(e => !((ISoftDeletable)e).IsDeleted);
            }

            return await query.ToListAsync();
        }

        public virtual async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = _dbSet;

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            query = query.Where(predicate);

            // Apply soft delete filter if entity supports it
            if (typeof(ISoftDeletable).IsAssignableFrom(typeof(TEntity)))
            {
                query = query.Where(e => !((ISoftDeletable)e).IsDeleted);
            }

            return await query.ToListAsync();
        }

        public virtual async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            IQueryable<TEntity> query = _dbSet;

            // Apply soft delete filter if entity supports it
            if (typeof(ISoftDeletable).IsAssignableFrom(typeof(TEntity)))
            {
                query = query.Where(e => !((ISoftDeletable)e).IsDeleted);
            }

            return await query.FirstOrDefaultAsync(predicate);
        }

        public virtual async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = _dbSet;

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            // Apply soft delete filter if entity supports it
            if (typeof(ISoftDeletable).IsAssignableFrom(typeof(TEntity)))
            {
                query = query.Where(e => !((ISoftDeletable)e).IsDeleted);
            }

            return await query.FirstOrDefaultAsync(predicate);
        }

        public virtual async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
        {
            IQueryable<TEntity> query = _dbSet;

            // Apply soft delete filter if entity supports it
            if (typeof(ISoftDeletable).IsAssignableFrom(typeof(TEntity)))
            {
                query = query.Where(e => !((ISoftDeletable)e).IsDeleted);
            }

            return await query.AnyAsync(predicate);
        }

        public virtual async Task<int> CountAsync()
        {
            IQueryable<TEntity> query = _dbSet;

            // Apply soft delete filter if entity supports it
            if (typeof(ISoftDeletable).IsAssignableFrom(typeof(TEntity)))
            {
                query = query.Where(e => !((ISoftDeletable)e).IsDeleted);
            }

            return await query.CountAsync();
        }

        public virtual async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            IQueryable<TEntity> query = _dbSet;

            // Apply soft delete filter if entity supports it
            if (typeof(ISoftDeletable).IsAssignableFrom(typeof(TEntity)))
            {
                query = query.Where(e => !((ISoftDeletable)e).IsDeleted);
            }

            return await query.CountAsync(predicate);
        }

        public virtual async Task<TEntity> AddAsync(TEntity entity)
        {
            // Set audit fields if entity supports it
            if (entity is IAuditableEntity auditableEntity)
            {
                auditableEntity.CreatedAt = DateTime.UtcNow;
                auditableEntity.UpdatedAt = DateTime.UtcNow;
            }

            if (entity is ISoftDeletable softDeletable)
            {
                softDeletable.IsDeleted = false;
            }

            await _dbSet.AddAsync(entity);
            return entity;
        }

        public virtual async Task<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> entities)
        {
            var entityList = entities.ToList();
            var now = DateTime.UtcNow;

            foreach (var entity in entityList)
            {
                if (entity is IAuditableEntity auditableEntity)
                {
                    auditableEntity.CreatedAt = now;
                    auditableEntity.UpdatedAt = now;
                }

                if (entity is ISoftDeletable softDeletable)
                {
                    softDeletable.IsDeleted = false;
                }
            }

            await _dbSet.AddRangeAsync(entityList);
            return entityList;
        }

        public virtual async Task<TEntity> UpdateAsync(TEntity entity)
        {
            if (entity is IAuditableEntity auditableEntity)
            {
                auditableEntity.UpdatedAt = DateTime.UtcNow;
            }

            _dbSet.Update(entity);
            return await Task.FromResult(entity);
        }

        public virtual async Task<TEntity> DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                return await DeleteAsync(entity);
            }
            throw new NotFoundException($"Entity with id {id} not found");
        }

        public virtual async Task<TEntity> DeleteAsync(TEntity entity)
        {
            // Soft delete if entity supports it
            if (entity is ISoftDeletable softDeletable)
            {
                softDeletable.IsDeleted = true;
                if (entity is IAuditableEntity auditableEntity)
                {
                    auditableEntity.UpdatedAt = DateTime.UtcNow;
                }
                _dbSet.Update(entity);
            }
            else
            {
                _dbSet.Remove(entity);
            }

            return await Task.FromResult(entity);
        }

        public virtual async Task<IEnumerable<TEntity>> DeleteRangeAsync(IEnumerable<TEntity> entities)
        {
            var entityList = entities.ToList();
            var now = DateTime.UtcNow;

            foreach (var entity in entityList)
            {
                if (entity is ISoftDeletable softDeletable)
                {
                    softDeletable.IsDeleted = true;
                    if (entity is IAuditableEntity auditableEntity)
                    {
                        auditableEntity.UpdatedAt = now;
                    }
                }
            }

            if (entityList.Any(e => e is ISoftDeletable))
            {
                _dbSet.UpdateRange(entityList);
            }
            else
            {
                _dbSet.RemoveRange(entityList);
            }

            return await Task.FromResult(entityList);
        }

        public virtual IQueryable<TEntity> Query()
        {
            IQueryable<TEntity> query = _dbSet;

            // Apply soft delete filter if entity supports it
            if (typeof(ISoftDeletable).IsAssignableFrom(typeof(TEntity)))
            {
                query = query.Where(e => !((ISoftDeletable)e).IsDeleted);
            }

            return query;
        }

        public virtual IQueryable<TEntity> QueryNoTracking()
        {
            IQueryable<TEntity> query = _dbSet.AsNoTracking();

            // Apply soft delete filter if entity supports it
            if (typeof(ISoftDeletable).IsAssignableFrom(typeof(TEntity)))
            {
                query = query.Where(e => !((ISoftDeletable)e).IsDeleted);
            }

            return query;
        }

        public virtual async Task<IEnumerable<TEntity>> GetPagedAsync(int pageNumber, int pageSize)
        {
            IQueryable<TEntity> query = _dbSet;

            // Apply soft delete filter if entity supports it
            if (typeof(ISoftDeletable).IsAssignableFrom(typeof(TEntity)))
            {
                query = query.Where(e => !((ISoftDeletable)e).IsDeleted);
            }

            return await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public virtual async Task<IEnumerable<TEntity>> GetPagedAsync(Expression<Func<TEntity, bool>> predicate, int pageNumber, int pageSize)
        {
            IQueryable<TEntity> query = _dbSet.Where(predicate);

            // Apply soft delete filter if entity supports it
            if (typeof(ISoftDeletable).IsAssignableFrom(typeof(TEntity)))
            {
                query = query.Where(e => !((ISoftDeletable)e).IsDeleted);
            }

            return await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }


    }
}
