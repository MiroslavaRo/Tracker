using Microsoft.EntityFrameworkCore;

namespace Tracker.Models
{
    public class TrackerDbContext : DbContext
    {
        public TrackerDbContext(DbContextOptions options):base(options)
        {

        }

        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}
