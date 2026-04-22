using Application.DTOs.Appointment;
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
    public class AppointmentService : IAppointmentService
    {
        private readonly IRepository<Appointment> _appointmentRepo;
        private readonly IRepository<Doctor> _doctorRepo;
        private readonly IMapper _mapper;

        public AppointmentService(
            IRepository<Appointment> appointmentRepo,
            IRepository<Doctor> doctorRepo,
            IMapper mapper)
        {
            _appointmentRepo = appointmentRepo;
            _doctorRepo = doctorRepo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AppointmentDto>> GetAllAsync()
        {
            var list = await _appointmentRepo.GetAllAsync();
            return _mapper.Map<IEnumerable<AppointmentDto>>(list);
        }

        public async Task<AppointmentDto?> GetByIdAsync(int id)
        {
            var appointment = await _appointmentRepo.GetByIdAsync(id);
            return appointment == null ? null : _mapper.Map<AppointmentDto>(appointment);
        }

        public async Task<IEnumerable<AppointmentDto>> GetByPatientIdAsync(int patientId)
        {
            var list = await _appointmentRepo.FindAsync(a => a.PatientId == patientId);
            return _mapper.Map<IEnumerable<AppointmentDto>>(list);
        }

        public async Task<IEnumerable<AppointmentDto>> GetByDoctorIdAsync(int doctorId)
        {
            var list = await _appointmentRepo.FindAsync(a => a.DoctorId == doctorId);
            return _mapper.Map<IEnumerable<AppointmentDto>>(list);
        }

        public async Task<AppointmentDto> CreateAsync(int patientId, AppointmentCreateDto dto)
        {
            var doctor = await _doctorRepo.GetByIdAsync(dto.DoctorId)
                ?? throw new KeyNotFoundException($"الطبيب برقم {dto.DoctorId} غير موجود.");

            var conflicts = await _appointmentRepo.FindAsync(a =>
                a.DoctorId == dto.DoctorId &&
                a.AppointmentDate.Date == dto.AppointmentDate.Date &&
                a.Status != "Cancelled" &&
                a.StartTime < dto.EndTime &&
                a.EndTime > dto.StartTime);

            if (conflicts.Any())
                throw new InvalidOperationException("يوجد تعارض في مواعيد الطبيب في هذا الوقت.");

            var appointment = _mapper.Map<Appointment>(dto);
            appointment.PatientId = patientId;
            appointment.Status = "Pending";
            appointment.CreatedAt = DateTime.UtcNow;

            await _appointmentRepo.AddAsync(appointment);
            return _mapper.Map<AppointmentDto>(appointment);
        }

        public async Task<bool> CancelAsync(int appointmentId, int userId)
        {
            var appointment = await _appointmentRepo.GetByIdAsync(appointmentId);
            if (appointment == null) return false;
            if (appointment.PatientId != userId)
                throw new UnauthorizedAccessException("لا يمكنك إلغاء موعد لا يخصك.");

            appointment.Status = "Cancelled";
            await _appointmentRepo.UpdateAsync(appointment);
            return true;
        }

        public async Task<bool> ConfirmAsync(int appointmentId)
        {
            var appointment = await _appointmentRepo.GetByIdAsync(appointmentId);
            if (appointment == null) return false;
            appointment.Status = "Confirmed";
            await _appointmentRepo.UpdateAsync(appointment);
            return true;
        }

        public async Task<bool> CompleteAsync(int appointmentId)
        {
            var appointment = await _appointmentRepo.GetByIdAsync(appointmentId);
            if (appointment == null) return false;
            appointment.Status = "Completed";
            await _appointmentRepo.UpdateAsync(appointment);
            return true;
        }
    }
}
