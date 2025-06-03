using AuthService.DomainModel;
using AuthService.GenereicRepository;
using AuthService.Repository;
using AuthService.Repository.Interface;
using Microsoft.EntityFrameworkCore.Storage;

namespace AuthService.Data.UnitofWorkPattern
{
   
    public interface IUnitOfWork : IDisposable
    {
        ILicenceRepository LicenceRepository { get; }
        ITenantRepository TenantRepository { get; }
        IApplicationUserRepository UserRepository { get; }
        IRoleRepository RoleRepository { get; }
        IPermissionRepository PermissionRepository { get; }
        IRolePermissionRepository RolePermissionRepository { get; }
        IUserRefreshTokenRepository RefreshTokenRepository { get; }

        IUserRoleRepository UserRoleRepository { get; }
        IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class, IEntity;

        Task<int> SaveChangesAsync();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }

    public class UnitOfWork : IUnitOfWork
    {
        private readonly AuthDbContext _context;
        private readonly Dictionary<Type, object> _repositories;
        private IDbContextTransaction? _transaction;

        public UnitOfWork(AuthDbContext context)
        {
            _context = context;
            _repositories = new Dictionary<Type, object>();

            // Initialize specific repositories
            LicenceRepository = new LicenceRepository(_context);
            TenantRepository = new TenantRepository(_context);
            UserRepository = new ApplicationUserRepository(_context);
            RoleRepository = new RoleRepository(_context);
            PermissionRepository = new PermissionRepository(_context);
            RefreshTokenRepository = new UserRefreshTokenRepository(_context);
        }

        public ILicenceRepository LicenceRepository { get; }
        public ITenantRepository TenantRepository { get; }
        public IApplicationUserRepository UserRepository { get; }
        public IRoleRepository RoleRepository { get; }
        public IPermissionRepository PermissionRepository { get; }
        public IUserRefreshTokenRepository RefreshTokenRepository { get; }
        public IUserRoleRepository UserRoleRepository { get; }
        public IRolePermissionRepository RolePermissionRepository { get; }

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class, IEntity
        {
            var type = typeof(TEntity);

            if (!_repositories.ContainsKey(type))
            {
                _repositories[type] = new GenericRepository<TEntity>(_context);
            }

            return (IGenericRepository<TEntity>)_repositories[type];
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _context.Dispose();
        }
    }

}
