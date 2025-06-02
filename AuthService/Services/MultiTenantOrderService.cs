namespace AuthService.Services
{
    public class MultiTenantOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MultiTenantOrderService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<Order>> GetOrdersForCurrentTenantAsync()
        {
            var currentTenantId = GetCurrentTenantId();

            // Use the generic repository for entities not in specific repositories
            var orders = await _unitOfWork.Repository<Order>()
                .FindAsync(o => o.TenantId == currentTenantId);

            return orders.ToList();
        }

        private int GetCurrentTenantId()
        {
            // Extract tenant ID from current user claims
            var tenantIdClaim = _httpContextAccessor.HttpContext?.User
                .FindFirst("TenantId")?.Value;

            return int.Parse(tenantIdClaim ?? "0");
        }
    }

}
