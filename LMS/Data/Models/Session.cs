using System;
using System.ComponentModel.DataAnnotations;

namespace LMS.Data.Models
{
    public class Session
    {
        [Key]
        public int Id { get; set; }
        public Guid SessionId { get; set; }
        public DateTime ExpireDate { get; set; }
        public int AccountId { get; set; }
    }
}
