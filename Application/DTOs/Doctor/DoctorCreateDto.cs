using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Doctor
{
    public class DoctorCreateDto
    {
        [Required(ErrorMessage = "اسم الطبيب مطلوب.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "يجب أن يكون الاسم بين 3 و 100 حرف.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "البريد الإلكتروني مطلوب.")]
        [EmailAddress(ErrorMessage = "صيغة البريد الإلكتروني غير صحيحة.")]
        public string Email { get; set; } = string.Empty;

        public string Biography { get; set; } = string.Empty;

        [Required(ErrorMessage = "التخصص مطلوب.")]
        public string Specialization { get; set; } = string.Empty;

        [Range(0, 60, ErrorMessage = "سنوات الخبرة يجب أن تكون بين 0 و 60.")]
        public int ExperienceYears { get; set; }

        [Required(ErrorMessage = "المدينة مطلوبة.")]
        public string City { get; set; } = string.Empty;

        [Required(ErrorMessage = "معرف العيادة مطلوب.")]
        public int ClinicId { get; set; }
    }
}
