using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Doctor
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        [ForeignKey("UserId")]
        public User? User { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Biography { get; set; } = string.Empty; // معلومات مصرح عنها
        public string Specialization { get; set; } = string.Empty;
        public int ExperienceYears { get; set; }

        // الموقع الجغرافي (مثلاً: عمان، إربد...)
        public string City { get; set; } = string.Empty;

        // العلاقة مع العيادة
        public int ClinicId { get; set; }
        public Clinic Clinic { get; set; } = null!;

        // أوقات الدوام (يمكن تخزينها كنص أو كجدول منفصل)
        public List<Review> Reviews { get; set; } = new();
    }
}
