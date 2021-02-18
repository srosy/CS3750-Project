using Microsoft.AspNetCore.Identity;

namespace LMS.Data.Models
{
    public class User : IdentityUser
    {
        public int AccountId { get; set; }
        public int UserType { get; set; }
    }
}
