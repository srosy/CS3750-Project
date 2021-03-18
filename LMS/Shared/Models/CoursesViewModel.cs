using LMS.Data.Models;
using System.Collections.Generic;

namespace LMS.Shared.Models
{
    public class CoursesViewModel
    {
        public Account Account { get; set; }
        public List<Course> Courses { get; set; }
        public List<Account> Professors { get; set; }
        public List<Enrollment> Enrollments { get; set; }
        public virtual Dictionary<string, object> TableAttributes { get; set; } = new Dictionary<string, object>() { { "title", "Select a course to edit." } };
        public string FormTitle { get; set; }
    }
}
