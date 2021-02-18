using System;
using System.ComponentModel.DataAnnotations;

namespace LMS.Data.Models
{
    public class Course
    {
        [Key]
        public int CourseId { get; set; }
        [Required] public int ProfessorId { get; set; }
        [Required] [StringLength(10, ErrorMessage = "Name should be 6 characters or less i.e. (CS 3750)")]
        public string Name { get; set; }
        [Required] [StringLength(100)] public string Description { get; set; }
        [Required] [DataType(DataType.Date)] public DateTime StartDate { get; set; }
        [Required] [DataType(DataType.Date)] public DateTime EndDate { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? DeleteDate { get; set; }
        [Required] [Range(1, 4, ErrorMessage = "Credits range [1, 4]")] public int Credits { get; set; }
        [Required] public string StartTime { get; set; }
        [Required] public string EndTime { get; set; }
        public string Markup { get; set; }
    }
}
