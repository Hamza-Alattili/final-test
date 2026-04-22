using Application.DTOs.Appointment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IAppointmentService
    {
        Task<IEnumerable<AppointmentDto>> GetAllAsync();
        Task<AppointmentDto?> GetByIdAsync(int id);
        Task<IEnumerable<AppointmentDto>> GetByPatientIdAsync(int patientId);
        Task<IEnumerable<AppointmentDto>> GetByDoctorIdAsync(int doctorId);
        Task<AppointmentDto> CreateAsync(int patientId, AppointmentCreateDto dto);
        Task<bool> CancelAsync(int appointmentId, int userId);
        Task<bool> ConfirmAsync(int appointmentId);
        Task<bool> CompleteAsync(int appointmentId);
    }
}
