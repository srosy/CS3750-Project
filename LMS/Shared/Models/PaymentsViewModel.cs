using LMS.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LMS.Shared.Models
{
    public class PaymentsViewModel
    {
        public Account Account { get; set; }
        public List<Payment> Payments { get; set; }
        public List<Enrollment> Enrollments { get; set; }
        public string FormTitle { get; set; }
        public Dictionary<string, object> TableAttributes { get; set; } = new Dictionary<string, object>() { { "title", "A previous payment." } };
    }

    public class PaymentFormViewModel
    {
        public int AccountId { get; set; }

        [Required]
        [MaxLength(4, ErrorMessage = "Expected CCV length of 3-4")]
        [MinLength(3, ErrorMessage = "Expected CCV length of 3-4")]
        public string CCV { get; set; }

        [Required]
        [DataType(DataType.CreditCard, ErrorMessage = "Invalid Credit Card")]
        [MaxLength(16, ErrorMessage = "Expected Credit Card length of 16")]
        [MinLength(16, ErrorMessage = "Expected Credit Card length of 16")]
        public string CC_Num { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Data.Helper.CustomValidation(ErrorMessage = "Expiration Date must be a future date")]
        public DateTime ExpDate { get; set; }
    }
}
