using LMS.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LMS.Shared.Models
{
    public class AnnouncementsViewModel
    {
        public Account Account { get; set; }
        public List<AnnouncementViewModel> Announcements { get; set; }
        public List<Course> Courses { get; set; }
    }

    public class AnnouncementViewModel
    {
        [Required] [MinLength(1, ErrorMessage = "You must input a Title.")] public string Title { get; set; }
        [Required] public int ProfessorAccountId { get; set; }
        public string ProfessorName { get; set; }
        [Required] [Range(1, 1000, ErrorMessage = "You must select a Course.")] public int CourseId { get; set; }
        [Required] [Range(1, 3, ErrorMessage = "You must select a Type.")] public int Type { get; set; }
        public string CourseName { get; set; }
        [Required] [MinLength(1, ErrorMessage = "You must input a Message.")] public string Message { get; set; }
        [DataType(DataType.Date)] public DateTime AnnouncementDate { get; set; }
        public bool Deleted { get; set; }
    }
}
