using AuthService.Dtos;
using AuthService.Helper;
using AuthService.Services.Interface;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AuthService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TenantsController : ControllerBase
    {
        private readonly ITenantService _tenantService;

        public TenantsController(ITenantService tenantService)
        {
            _tenantService = tenantService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TenantDto>>> GetTenants()
        {
            var tenants = await _tenantService.GetAllTenantsAsync();
            return Ok(tenants);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TenantDto>> GetTenant(int id)
        {
            var tenant = await _tenantService.GetTenantByIdAsync(id);
            if (tenant == null)
                return NotFound();

            return Ok(tenant);
        }

        [HttpGet("{id}/with-users")]
        public async Task<ActionResult<TenantDto>> GetTenantWithUsers(int id)
        {
            var tenant = await _tenantService.GetTenantWithUsersAsync(id);
            if (tenant == null)
                return NotFound();

            return Ok(tenant);
        }

        [HttpGet("by-licence/{licenceId}")]
        public async Task<ActionResult<IEnumerable<TenantDto>>> GetTenantsByLicence(int licenceId)
        {
            var tenants = await _tenantService.GetTenantsByLicenceAsync(licenceId);
            return Ok(tenants);
        }

        [HttpGet("{id}/user-count")]
        public async Task<ActionResult<int>> GetUserCountByTenant(int id)
        {
            var count = await _tenantService.GetUserCountByTenantAsync(id);
            return Ok(count);
        }

        [HttpPost]
        public async Task<ActionResult<TenantDto>> CreateTenant(CreateTenantDto createTenantDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var tenant = await _tenantService.CreateTenantAsync(createTenantDto);
                return CreatedAtAction(nameof(GetTenant), new { id = tenant.Id }, tenant);
            }
            catch (NotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<TenantDto>> UpdateTenant(int id, UpdateTenantDto updateTenantDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var tenant = await _tenantService.UpdateTenantAsync(id, updateTenantDto);
                return Ok(tenant);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTenant(int id)
        {
            try
            {
                var result = await _tenantService.DeleteTenantAsync(id);
                if (!result)
                    return NotFound();

                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
