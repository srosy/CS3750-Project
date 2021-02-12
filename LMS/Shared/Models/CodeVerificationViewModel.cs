using System.ComponentModel.DataAnnotations;

namespace LMS.Shared.Models
{
    public class CodeVerificationViewModel
    {
        [Required] public string Email { get; set; }
        [MaxLength(6)] public string VerificationCode { get; set; }
    }
}
