using IPZ_1.Models;
using Microsoft.EntityFrameworkCore;

namespace IPZ_1.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        {

        }

        public DbSet<Category> Category { get; set; }
        public DbSet<Product> Product { get; set; }

    }
}
