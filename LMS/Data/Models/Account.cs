using System;
using System.ComponentModel.DataAnnotations;

namespace LMS.Data.Models
{
    public class Account
    {
        [Key]
        public int AccountId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime CreateDate { get; set; }
        public int Role { get; set; }
    }
}
