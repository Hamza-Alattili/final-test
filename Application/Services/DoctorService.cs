using Application.DTOs.Doctor;
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
    public class DoctorService : IDoctorService
    {
        private readonly IRepository<Doctor> _doctorRepo;
        private readonly IRepository<Clinic> _clinicRepo;
        private readonly IMapper _mapper;

        public DoctorService(IRepository<Doctor> doctorRepo, IRepository<Clinic> clinicRepo, IMapper mapper)
        {
            _doctorRepo = doctorRepo;
            _clinicRepo = clinicRepo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<DoctorDto>> GetAllAsync()
        {
            var doctors = await _doctorRepo.GetAllAsync();
            return _mapper.Map<IEnumerable<DoctorDto>>(doctors);
        }

        public async Task<DoctorDto?> GetByIdAsync(int id)
        {
            var doctor = await _doctorRepo.GetByIdAsync(id);
            return doctor == null ? null : _mapper.Map<DoctorDto>(doctor);
        }

        public async Task<DoctorDto> CreateAsync(DoctorCreateDto dto)
        {
            var clinic = await _clinicRepo.GetByIdAsync(dto.ClinicId)
                ?? throw new KeyNotFoundException($"العيادة برقم {dto.ClinicId} غير موجودة.");

            var doctor = _mapper.Map<Doctor>(dto);
            await _doctorRepo.AddAsync(doctor);

            var saved = await _doctorRepo.GetByIdAsync(doctor.Id);
            return _mapper.Map<DoctorDto>(saved!);
        }

        public async Task<DoctorDto?> UpdateAsync(DoctorUpdateDto dto)
        {
            var doctor = await _doctorRepo.GetByIdAsync(dto.Id);
            if (doctor == null) return null;

            _mapper.Map(dto, doctor);
            await _doctorRepo.UpdateAsync(doctor);
            return _mapper.Map<DoctorDto>(doctor);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var doctor = await _doctorRepo.GetByIdAsync(id);
            if (doctor == null) return false;
            await _doctorRepo.DeleteAsync(doctor);
            return true;
        }

        public async Task<IEnumerable<DoctorDto>> SearchAsync(string? specialization, string? city)
        {
            var doctors = await _doctorRepo.FindAsync(d =>
                (string.IsNullOrEmpty(specialization) || d.Specialization.Contains(specialization)) &&
                (string.IsNullOrEmpty(city) || d.City.Contains(city))
            );
            return _mapper.Map<IEnumerable<DoctorDto>>(doctors);
        }
    }
}
