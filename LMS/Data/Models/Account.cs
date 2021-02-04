using System;
using System.ComponentModel.DataAnnotations;

namespace LMS.Data.Models
{
    public class Account
    {
        [Key] public int AccountId { get; set; }
        public int Role { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        [DataType(DataType.EmailAddress)] public string Email { get; set; }

        [DataType(DataType.Date)] public DateTime DOB { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? DeleteDate { get; set; }
    }
}
