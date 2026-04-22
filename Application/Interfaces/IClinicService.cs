using Application.DTOs.Clinic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IClinicService
    {
        Task<IEnumerable<ClinicDto>> GetAllAsync();
        Task<ClinicDto?> GetByIdAsync(int id);
        Task<ClinicDto> CreateAsync(ClinicCreateDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
