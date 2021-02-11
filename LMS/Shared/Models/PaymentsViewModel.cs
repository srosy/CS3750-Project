using LMS.Data.Models;
using System.Collections.Generic;

namespace LMS.Shared.Models
{
    public class PaymentsViewModel
    {
        public Account Account { get; set; }
        public List<Payment> Payments { get; set; }
    }
}
