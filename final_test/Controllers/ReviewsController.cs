using Application.DTOs.Review;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace final_test.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewsController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        private int GetCurrentUserId()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier) ?? User.FindFirst("sub");
            return int.TryParse(claim?.Value, out var id) ? id : 0;
        }

        /// <summary>جلب تقييمات طبيب معين</summary>
        [HttpGet("doctor/{doctorId:int}")]
        public async Task<IActionResult> GetByDoctor(int doctorId)
        {
            var reviews = await _reviewService.GetByDoctorIdAsync(doctorId);
            return Ok(reviews);
        }

        /// <summary>إضافة تقييم (Patient فقط)</summary>
        [HttpPost]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> Create([FromBody] ReviewCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var userId = GetCurrentUserId();
            var review = await _reviewService.CreateAsync(userId, dto);
            return Ok(review);
        }

        /// <summary>حذف تقييم</summary>
        [HttpDelete("{id:int}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = GetCurrentUserId();
            try
            {
                var result = await _reviewService.DeleteAsync(id, userId);
                return result ? NoContent() : NotFound(new { message = "التقييم غير موجود." });
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
        }
    }
}
