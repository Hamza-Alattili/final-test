using Application.DTOs.Doctor;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace final_test.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DoctorsController : ControllerBase
    {
        private readonly IDoctorService _doctorService;

        public DoctorsController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        /// <summary>جلب جميع الأطباء</summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var doctors = await _doctorService.GetAllAsync();
            return Ok(doctors);
        }

        /// <summary>البحث عن طبيب بالتخصص أو المدينة</summary>
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string? specialization, [FromQuery] string? city)
        {
            var doctors = await _doctorService.SearchAsync(specialization, city);
            return Ok(doctors);
        }

        /// <summary>جلب طبيب بالمعرف</summary>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var doctor = await _doctorService.GetByIdAsync(id);
            return doctor == null ? NotFound(new { message = "الطبيب غير موجود." }) : Ok(doctor);
        }

        /// <summary>إضافة طبيب جديد (Admin فقط)</summary>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] DoctorCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var doctor = await _doctorService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = doctor.Id }, doctor);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        /// <summary>تحديث بيانات طبيب (Admin فقط)</summary>
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] DoctorUpdateDto dto)
        {
            if (id != dto.Id) return BadRequest(new { message = "معرف الطبيب غير متطابق." });
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _doctorService.UpdateAsync(dto);
            return result == null ? NotFound(new { message = "الطبيب غير موجود." }) : Ok(result);
        }

        /// <summary>حذف طبيب (Admin فقط)</summary>
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _doctorService.DeleteAsync(id);
            return result ? NoContent() : NotFound(new { message = "الطبيب غير موجود." });
        }
    }
}
