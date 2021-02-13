using System;
using System.ComponentModel.DataAnnotations;

namespace LMS.Data.Models
{
    public class Submission
    {
        [Key]
        public int SubmissionId { get; set; }
        public int AssignmentId { get; set; }
        public int AccountId { get; set; }
        [Range(0, 100)] public int Score { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? DeleteDate { get; set; }
        public string Comments { get; set; }
        public string UploadFileName { get; set; }
        public string UploadFilePath { get; set; }
    }
}
