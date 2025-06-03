using AuthService.Data.UnitofWorkPattern;

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

       

        private int GetCurrentTenantId()
        {
            // Extract tenant ID from current user claims
            var tenantIdClaim = _httpContextAccessor.HttpContext?.User
                .FindFirst("TenantId")?.Value;

            return int.Parse(tenantIdClaim ?? "0");
        }
    }

}
