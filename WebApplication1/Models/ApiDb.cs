using Microsoft.EntityFrameworkCore;
namespace WebApplication1.Models
{
    public class ApiDb:DbContext
    {
        public virtual DbSet<Weather> Weathers { get; set; }
    }
}
