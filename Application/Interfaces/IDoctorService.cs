using Application.DTOs.Doctor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IDoctorService
    {
        Task<IEnumerable<DoctorDto>> GetAllAsync();
        Task<DoctorDto?> GetByIdAsync(int id);
        Task<DoctorDto> CreateAsync(DoctorCreateDto dto);
        Task<DoctorDto?> UpdateAsync(DoctorUpdateDto dto);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<DoctorDto>> SearchAsync(string? specialization, string? city);
    }
}

