using System.ComponentModel.DataAnnotations;
using System;

namespace LMS.Shared.Models
{
    public class AccountViewModel
    {
        const string MIN_DOB = "1/1/1900";
        const string MAX_DOB = "4/1/2003";

        public AuthenticationViewModel Auth { get; set; } = new AuthenticationViewModel();

        [Required] public string FirstName { get; set; }
        [Required] public string LastName { get; set; }
        [Required] [EmailAddress] public string Email { get; set; }
        [Required] public int Role { get; set; }

        [Required] [DataType(DataType.Date)]
        [Range(typeof(DateTime), MIN_DOB, MAX_DOB)]
        public DateTime Birthday { get; set; }
    }
}
