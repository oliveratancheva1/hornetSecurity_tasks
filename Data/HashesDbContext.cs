using Microsoft.EntityFrameworkCore;
using HashProcessorApp.Data.Models;

namespace HashProcessorApp.Data
{
    public class HashesDbContext : DbContext
    {
        // Constructor that accepts DbContextOptions
        public HashesDbContext(DbContextOptions<HashesDbContext> options)
            : base(options)
        {
            // Initialize DbSet if needed (EF Core normally does this automatically)
            HashCounts = Set<HashCount>();
        }

        // Define the DbSet for your entities
        public DbSet<HashCount> HashCounts { get; set; }
    }
}
