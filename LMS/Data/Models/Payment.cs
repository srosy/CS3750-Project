using System;
using System.ComponentModel.DataAnnotations;

namespace LMS.Data.Models
{
    public class Payment
    {
        [Key]
        public int PaymentId { get; set; }
        public int AccountId { get; set; }

        [DataType(DataType.CreditCard)]
        [MaxLength(16)] public string CardNumber { get; set; }

        [DataType(DataType.Date)]
        public DateTime ExpDate { get; set; }

        public DateTime? TransactionDate { get; set; }

        [MaxLength(4)] public int CCV { get; set; }
        [DataType(DataType.Currency)] public decimal AttemptAmount { get; set; }
        [DataType(DataType.Currency)] public decimal AuthAmount { get; set; }
        [DataType(DataType.Currency)] public decimal PaymentAmount { get; set; }
    }
}
