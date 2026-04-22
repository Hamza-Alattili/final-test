using Application.DTOs.Appointment;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace final_test.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AppointmentsController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentsController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        private int GetCurrentUserId()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier) ?? User.FindFirst("sub");
            return int.TryParse(claim?.Value, out var id) ? id : 0;
        }

        /// <summary>جلب جميع المواعيد (Admin فقط)</summary>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var list = await _appointmentService.GetAllAsync();
            return Ok(list);
        }

        /// <summary>جلب مواعيد المريض الحالي</summary>
        [HttpGet("my")]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> GetMyAppointments()
        {
            var userId = GetCurrentUserId();
            var list = await _appointmentService.GetByPatientIdAsync(userId);
            return Ok(list);
        }

        /// <summary>جلب مواعيد طبيب معين</summary>
        [HttpGet("doctor/{doctorId:int}")]
        [Authorize(Roles = "Admin,Doctor")]
        public async Task<IActionResult> GetByDoctor(int doctorId)
        {
            var list = await _appointmentService.GetByDoctorIdAsync(doctorId);
            return Ok(list);
        }

        /// <summary>جلب موعد بالمعرف</summary>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var appointment = await _appointmentService.GetByIdAsync(id);
            return appointment == null ? NotFound(new { message = "الموعد غير موجود." }) : Ok(appointment);
        }

        /// <summary>حجز موعد جديد (Patient فقط)</summary>
        [HttpPost]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> Create([FromBody] AppointmentCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var userId = GetCurrentUserId();
            try
            {
                var result = await _appointmentService.CreateAsync(userId, dto);
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        /// <summary>إلغاء موعد (Patient فقط)</summary>
        [HttpPatch("{id:int}/cancel")]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> Cancel(int id)
        {
            var userId = GetCurrentUserId();
            try
            {
                var result = await _appointmentService.CancelAsync(id, userId);
                return result ? NoContent() : NotFound(new { message = "الموعد غير موجود." });
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
        }

        /// <summary>تأكيد موعد (Doctor/Admin)</summary>
        [HttpPatch("{id:int}/confirm")]
        [Authorize(Roles = "Admin,Doctor")]
        public async Task<IActionResult> Confirm(int id)
        {
            var result = await _appointmentService.ConfirmAsync(id);
            return result ? NoContent() : NotFound(new { message = "الموعد غير موجود." });
        }

        /// <summary>إتمام موعد (Doctor/Admin)</summary>
        [HttpPatch("{id:int}/complete")]
        [Authorize(Roles = "Admin,Doctor")]
        public async Task<IActionResult> Complete(int id)
        {
            var result = await _appointmentService.CompleteAsync(id);
            return result ? NoContent() : NotFound(new { message = "الموعد غير موجود." });
        }
    }
}
