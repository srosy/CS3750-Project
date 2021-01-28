using System.ComponentModel.DataAnnotations;

namespace LMS.Shared.Models
{
    public class AccountModel
    {
        public AuthModel Auth { get; set; } = new AuthModel();

        [Required] public string FirstName { get; set; }
        [Required] public string LastName { get; set; }
        [Required] [EmailAddress] public string Email { get; set; }
        [Required] public int Role { get; set; }
        //[Required] public  Birthday { get; set; }
    }
}
