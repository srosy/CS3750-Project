using LMS.Data.Models;
using System.Collections.Generic;

namespace LMS.Shared.Models
{
    public class AssignmentSubmissionViewModel
    {
        public Assignment Assignment { get; set; }
        public List<Submission> Submissions { get; set; }
        public Account Account { get; set; }
    }
}
