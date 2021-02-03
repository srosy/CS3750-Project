using LMS.Data.Models;
using System.Collections.Generic;

namespace LMS.Shared.Models
{
    public class CoursesViewModel
    {
        public List<Course> Courses { get; set; }
        public List<Account> Professors { get; set; }
        public readonly Dictionary<string, object> TableAttributes = new Dictionary<string, object>() { { "title", "Select a course to edit." } };
        public string FormTitle { get; set; }
    }
}
