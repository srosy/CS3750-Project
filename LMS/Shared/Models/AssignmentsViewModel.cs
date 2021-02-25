using LMS.Data.Models;
using System.Collections.Generic;

namespace LMS.Shared.Models
{
    public class AssignmentsViewModel
    {
        public List<Assignment> Assignments { get; set; }
        public Account Account { get; set; }
        public List<Course> Courses { get; set; }
        public List<Enrollment> Enrollments { get; set; }
        public List<Submission> Submissions { get; set; }

        public virtual Dictionary<string, object> TableAttributes { get; set; } = new Dictionary<string, object>() { { "title", "Select an assignment to edit." } };
        public string FormTitle { get; set; }
    }
}
