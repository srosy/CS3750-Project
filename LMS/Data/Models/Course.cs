using System.ComponentModel.DataAnnotations;

namespace LMS.Data.Models
{
    public class Course
    {
        [Key]
        public int CourseId { get; set; }
        public int ProfessorId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
