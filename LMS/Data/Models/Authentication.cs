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
        public string Salt { get; set; }
        public string ResetCode { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? DeleteDate { get; set; }
    }
}
