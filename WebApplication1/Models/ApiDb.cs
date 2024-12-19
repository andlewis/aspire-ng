using Microsoft.EntityFrameworkCore;
namespace WebApplication1.Models
{
    public class ApiDb(DbContextOptions<ApiDb> options) : DbContext(options)
    {

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        public virtual DbSet<Weather> Weathers { get; set; }
    }
}
