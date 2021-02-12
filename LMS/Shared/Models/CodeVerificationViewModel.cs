using System.ComponentModel.DataAnnotations;

namespace LMS.Shared.Models
{
    public class CodeVerificationViewModel
    {
        [MaxLength(6)] public string VerificationCode { get; set; }
    }
}
