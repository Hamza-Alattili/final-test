using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Clinic
{
    public class ClinicCreateDto
    {
        [Required(ErrorMessage = "اسم العيادة مطلوب.")]
        [StringLength(200, MinimumLength = 3)]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "عنوان العيادة مطلوب.")]
        public string Address { get; set; } = string.Empty;

        [Required(ErrorMessage = "رقم هاتف العيادة مطلوب.")]
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
