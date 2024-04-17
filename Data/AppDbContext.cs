using KeyValue.Models;
using Microsoft.EntityFrameworkCore;

namespace KeyValue.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<KeyValueData> KeyValues { get; set; }
    }
}
