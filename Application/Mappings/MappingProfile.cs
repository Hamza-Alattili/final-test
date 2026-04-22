using Application.DTOs.Appointment;
using Application.DTOs.Auth;
using Application.DTOs.Clinic;
using Application.DTOs.Doctor;
using Application.DTOs.Review;
using Application.DTOs.Schedule;
using Application.DTOs.User;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // User
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role != null ? src.Role.Name : string.Empty));

            CreateMap<User, UserInfoDto>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role != null ? src.Role.Name : string.Empty));

            // Doctor
            CreateMap<Doctor, DoctorDto>()
                .ForMember(dest => dest.ClinicName, opt => opt.MapFrom(src => src.Clinic != null ? src.Clinic.Name : string.Empty));

            CreateMap<DoctorCreateDto, Doctor>();
            CreateMap<DoctorUpdateDto, Doctor>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // Appointment
            CreateMap<Appointment, AppointmentDto>()
                .ForMember(dest => dest.PatientName, opt => opt.MapFrom(src => src.Patient != null ? src.Patient.Name : string.Empty))
                .ForMember(dest => dest.DoctorName, opt => opt.MapFrom(src => src.Doctor != null ? src.Doctor.Name : string.Empty));

            CreateMap<AppointmentCreateDto, Appointment>();

            // Review
            CreateMap<Review, ReviewDto>()
                .ForMember(dest => dest.PatientName, opt => opt.MapFrom(src => src.User != null ? src.User.Name : string.Empty))
                .ForMember(dest => dest.DoctorName, opt => opt.MapFrom(src => src.Doctor != null ? src.Doctor.Name : string.Empty));

            CreateMap<ReviewCreateDto, Review>();

            // Clinic
            CreateMap<Clinic, ClinicDto>();
            CreateMap<ClinicCreateDto, Clinic>();

            // Schedule
            CreateMap<Schedule, ScheduleDto>()
                .ForMember(dest => dest.DoctorName, opt => opt.MapFrom(src => src.Doctor != null ? src.Doctor.Name : string.Empty));

            CreateMap<ScheduleCreateDto, Schedule>();
        }
    }
}
