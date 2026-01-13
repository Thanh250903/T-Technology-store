using Ecommerce_Web.Models;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_Web.Data
{
    public class ApplicationDbContext : DbContext
    {

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories  { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { 
        
                
        }
    }
}
