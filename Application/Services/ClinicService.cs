using Application.DTOs.Clinic;
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
    public class ClinicService : IClinicService
    {
        private readonly IRepository<Clinic> _clinicRepo;
        private readonly IMapper _mapper;

        public ClinicService(IRepository<Clinic> clinicRepo, IMapper mapper)
        {
            _clinicRepo = clinicRepo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ClinicDto>> GetAllAsync()
        {
            var clinics = await _clinicRepo.GetAllAsync();
            return _mapper.Map<IEnumerable<ClinicDto>>(clinics);
        }

        public async Task<ClinicDto?> GetByIdAsync(int id)
        {
            var clinic = await _clinicRepo.GetByIdAsync(id);
            return clinic == null ? null : _mapper.Map<ClinicDto>(clinic);
        }

        public async Task<ClinicDto> CreateAsync(ClinicCreateDto dto)
        {
            var clinic = _mapper.Map<Clinic>(dto);
            await _clinicRepo.AddAsync(clinic);
            return _mapper.Map<ClinicDto>(clinic);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var clinic = await _clinicRepo.GetByIdAsync(id);
            if (clinic == null) return false;
            await _clinicRepo.DeleteAsync(clinic);
            return true;
        }
    }
}
