using crud.Models;
using Microsoft.EntityFrameworkCore;

namespace crud.DataContext
{
    public class ApplicationDbContext : DbContext

    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options) 
        {
           
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=testing;Username=postgres;Password=3117");
        }

        public DbSet<Brands> Brands { get; set; }
    }
}
