using Application.DTOs.Review;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IReviewService
    {
        Task<IEnumerable<ReviewDto>> GetByDoctorIdAsync(int doctorId);
        Task<ReviewDto> CreateAsync(int userId, ReviewCreateDto dto);
        Task<bool> DeleteAsync(int reviewId, int userId);
    }
}
