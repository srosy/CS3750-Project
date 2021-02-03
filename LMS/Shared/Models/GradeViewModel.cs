using LMS.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMS.Shared.Models
{
    public class GradeViewModel
    {
        public List<Submission> StudentGrades { get; set; }
        public List<Submission> InstructorCourseGrades { get; set; }
    }
}
