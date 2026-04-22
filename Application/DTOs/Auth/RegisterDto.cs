using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Auth
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "الاسم مطلوب.")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "يجب أن يكون الاسم بين 3 و 200 حرف.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "البريد الإلكتروني مطلوب.")]
        [EmailAddress(ErrorMessage = "صيغة البريد الإلكتروني غير صحيحة.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "كلمة المرور مطلوبة.")]
        [MinLength(8, ErrorMessage = "كلمة المرور يجب أن تكون 8 أحرف على الأقل.")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "تأكيد كلمة المرور مطلوب.")]
        [Compare("Password", ErrorMessage = "كلمتا المرور غير متطابقتين.")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "رقم الهاتف مطلوب.")]
        [MinLength(10, ErrorMessage = "رقم الهاتف يجب أن يكون 10 أرقام على الأقل.")]
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
