using LMS.Data.Models;
using System.Collections.Generic;

namespace LMS.Shared.Models
{
    public class CourseDetailViewModel
    {
        public Course Course { get; set; }
        public Account Account { get; set; }
        public List<Assignment> Assignments { get; set; }
    }
}
