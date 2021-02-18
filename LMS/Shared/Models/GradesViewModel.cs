using LMS.Data.Models;
using System.Collections.Generic;

namespace LMS.Shared.Models
{
    public class GradesViewModel
    {
        public Account Account { get; set; }
        public List<Course> Courses { get; set; }
        public List<GradeViewModel> Grades { get; set; }
    }

    public class GradeViewModel
    {
        public int CourseId { get; set; }
        public List<Assignment> Assignments { get; set; }
        public List<Submission> Submissions { get; set; }
        public decimal OverallPercentageGrade { get; set; }
        public string OverallLetterGrade { get; set; }
        public List<Grade> Grades { get; set; }
    }

    public class Grade
    {
        public int AssignmentId { get; set; }
        public string AssignmentName { get; set; }
        public decimal Score { get; set; }
        public string ScoreDisplay { get; set; }
        public string LetterGrade { get; set; }
    }
}
