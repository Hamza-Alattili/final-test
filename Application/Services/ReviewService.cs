using Application.DTOs.Review;
using Application.Interfaces;
using Application.Repositories.Interfaces;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IRepository<Review> _reviewRepo;
        private readonly IMapper _mapper;

        public ReviewService(IRepository<Review> reviewRepo, IMapper mapper)
        {
            _reviewRepo = reviewRepo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ReviewDto>> GetByDoctorIdAsync(int doctorId)
        {
            var reviews = await _reviewRepo.FindAsync(r => r.DoctorId == doctorId);
            return _mapper.Map<IEnumerable<ReviewDto>>(reviews);
        }

        public async Task<ReviewDto> CreateAsync(int userId, ReviewCreateDto dto)
        {
            var review = _mapper.Map<Review>(dto);
            review.UserId = userId;
            review.CreatedAt = DateTime.UtcNow;
            await _reviewRepo.AddAsync(review);
            return _mapper.Map<ReviewDto>(review);
        }

        public async Task<bool> DeleteAsync(int reviewId, int userId)
        {
            var review = await _reviewRepo.GetByIdAsync(reviewId);
            if (review == null) return false;
            if (review.UserId != userId)
                throw new UnauthorizedAccessException("لا يمكنك حذف تقييم لا يخصك.");
            await _reviewRepo.DeleteAsync(review);
            return true;
        }
    }
}
