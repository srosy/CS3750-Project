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
    }
}
