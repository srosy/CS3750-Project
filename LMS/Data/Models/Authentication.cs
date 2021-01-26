using System;
using System.ComponentModel.DataAnnotations;

namespace LMS.Data.Models
{
    public class Authentication
    {
        [Key]
        public int AuthId { get; set; }
        public int AccountId { get; set; }
        public string Password { get; set; }
        public DateTime CreateDate { get; internal set; }
    }
}
