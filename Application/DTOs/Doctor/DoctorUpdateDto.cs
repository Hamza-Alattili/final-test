using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Doctor
{
    public class DoctorUpdateDto
    {
        [Required(ErrorMessage = "معرف الطبيب مطلوب.")]
        public int Id { get; set; }

        [StringLength(100, MinimumLength = 3, ErrorMessage = "يجب أن يكون الاسم بين 3 و 100 حرف.")]
        public string? Name { get; set; }

        [EmailAddress(ErrorMessage = "صيغة البريد الإلكتروني غير صحيحة.")]
        public string? Email { get; set; }

        public string? Biography { get; set; }

        public string? Specialization { get; set; }

        [Range(0, 60, ErrorMessage = "سنوات الخبرة يجب أن تكون بين 0 و 60.")]
        public int? ExperienceYears { get; set; }

        public string? City { get; set; }

        public int? ClinicId { get; set; }
    }
}

