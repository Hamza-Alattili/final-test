using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Review
{
    public class ReviewCreateDto
    {
        [Required(ErrorMessage = "معرف الطبيب مطلوب.")]
        public int DoctorId { get; set; }

        [Required(ErrorMessage = "التعليق مطلوب.")]
        [StringLength(1000, MinimumLength = 5, ErrorMessage = "يجب أن يكون التعليق بين 5 و 1000 حرف.")]
        public string Comment { get; set; } = string.Empty;

        [Required(ErrorMessage = "التقييم مطلوب.")]
        [Range(1, 5, ErrorMessage = "التقييم يجب أن يكون بين 1 و 5.")]
        public int Rating { get; set; }
    }
}

