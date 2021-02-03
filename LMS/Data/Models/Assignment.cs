using LMS.Data.Enum;
using System;
using System.ComponentModel.DataAnnotations;

namespace LMS.Data.Models
{
    public class Assignment
    {
        [Key]
        public int AssignmentId { get; set; }
        public int CourseId { get; set; }
        [Range(0, 100)] public int MaxScore { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public AssignmentType Type { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? DeleteDate { get; set; }
    }
}
