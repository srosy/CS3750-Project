using LMS.Data.Models;
using System.Collections.Generic;

namespace LMS.Shared.Models
{
    public class EnrollmentsViewModel : CoursesViewModel
    {
        public Account Account { get; set; }
        public List<Enrollment> Enrollments { get; set; }
        public override Dictionary<string, object> TableAttributes { get; set; } = new Dictionary<string, object>() { { "title", "Select a course to edit." } };
    }
}
