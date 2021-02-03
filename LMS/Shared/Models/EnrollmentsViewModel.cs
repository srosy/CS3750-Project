using LMS.Data.Models;
using System.Collections.Generic;

namespace LMS.Shared.Models
{
    public class EnrollmentsViewModel : CoursesViewModel
    {
        public Account Account { get; set; }
        public List<Enrollment> Enrollments { get; set; }
    }
}
