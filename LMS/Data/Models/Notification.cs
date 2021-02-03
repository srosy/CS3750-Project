using System;
using System.ComponentModel.DataAnnotations;

namespace LMS.Data.Models
{
    public class Notification
    {
        [Key]
        public int NotificationId { get; set; }
        public int CourseId { get; set; }
        public int Type { get; set; }
        public string Message { get; set; }

        public DateTime CreateDate { get; set; }
        public DateTime DeleteDate { get; set; }
    }
}
