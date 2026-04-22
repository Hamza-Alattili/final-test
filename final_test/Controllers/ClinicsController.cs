using Application.DTOs.Clinic;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace final_test.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClinicsController : ControllerBase
    {
        private readonly IClinicService _clinicService;

        public ClinicsController(IClinicService clinicService)
        {
            _clinicService = clinicService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var clinics = await _clinicService.GetAllAsync();
            return Ok(clinics);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var clinic = await _clinicService.GetByIdAsync(id);
            return clinic == null ? NotFound(new { message = "العيادة غير موجودة." }) : Ok(clinic);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] ClinicCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var clinic = await _clinicService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = clinic.Id }, clinic);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _clinicService.DeleteAsync(id);
            return result ? NoContent() : NotFound(new { message = "العيادة غير موجودة." });
        }
    }
}
