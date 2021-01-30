using LMS.Data.Models;
using System.Collections.Generic;

namespace LMS.Shared.Models
{
    public class RegistrationViewModel
    {
        public List<Course> AvailableCourses { get; set; }
        public Account Account { get; set; }
        public List<Enrollement> Registrations { get; set; }
    }
}
