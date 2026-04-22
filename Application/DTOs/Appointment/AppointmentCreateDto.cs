using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Appointment
{
    public class AppointmentCreateDto
    {
        [Required(ErrorMessage = "معرف الطبيب مطلوب.")]
        public int DoctorId { get; set; }

        [Required(ErrorMessage = "تاريخ الموعد مطلوب.")]
        public DateTime AppointmentDate { get; set; }

        [Required(ErrorMessage = "وقت البداية مطلوب.")]
        public TimeSpan StartTime { get; set; }

        [Required(ErrorMessage = "وقت النهاية مطلوب.")]
        public TimeSpan EndTime { get; set; }
    }
}
