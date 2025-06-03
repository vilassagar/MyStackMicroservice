using AuthService.Dtos;
using AuthService.Helper;
using AuthService.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LicencesController : ControllerBase
    {
        private readonly ILicenceService _licenceService;

        public LicencesController(ILicenceService licenceService)
        {
            _licenceService = licenceService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LicenceDto>>> GetLicences()
        {
            var licences = await _licenceService.GetAllLicencesAsync();
            return Ok(licences);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LicenceDto>> GetLicence(int id)
        {
            var licence = await _licenceService.GetLicenceByIdAsync(id);
            if (licence == null)
                return NotFound();

            return Ok(licence);
        }

        [HttpGet("{id}/with-tenants")]
        public async Task<ActionResult<LicenceDto>> GetLicenceWithTenants(int id)
        {
            var licence = await _licenceService.GetLicenceWithTenantsAsync(id);
            if (licence == null)
                return NotFound();

            return Ok(licence);
        }

        [HttpGet("{id}/available-slots/{requestedSlots}")]
        public async Task<ActionResult<bool>> CheckAvailableSlots(int id, int requestedSlots)
        {
            var hasSlots = await _licenceService.HasAvailableUserSlotsAsync(id, requestedSlots);
            return Ok(hasSlots);
        }

        [HttpPost]
        public async Task<ActionResult<LicenceDto>> CreateLicence(CreateLicenceDto createLicenceDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var licence = await _licenceService.CreateLicenceAsync(createLicenceDto);
            return CreatedAtAction(nameof(GetLicence), new { id = licence.Id }, licence);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<LicenceDto>> UpdateLicence(int id, UpdateLicenceDto updateLicenceDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var licence = await _licenceService.UpdateLicenceAsync(id, updateLicenceDto);
                return Ok(licence);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteLicence(int id)
        {
            try
            {
                var result = await _licenceService.DeleteLicenceAsync(id);
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
