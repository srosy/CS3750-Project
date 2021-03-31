using System.ComponentModel.DataAnnotations;

namespace LMS.Data.Models
{
    public class NotificationViewed
    {
        [Key]
        public int ViewedId { get; set; }
        public int NotificationId { get; set; }
        public int AccountId { get; set; }
        public bool Viewed { get; set; }
    }
}
