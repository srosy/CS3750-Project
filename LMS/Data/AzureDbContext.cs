using LMS.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace LMS.Data
{
    public class AzureDbContext : DbContext
    {
        public AzureDbContext(DbContextOptions<AzureDbContext> options)
           : base(options)
        {
        }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Authentication> Authentications { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Submission> Submissions { get; set; }
        public DbSet<Settings> Settings { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
    }
}
