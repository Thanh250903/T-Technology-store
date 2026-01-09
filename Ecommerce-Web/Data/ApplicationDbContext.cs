using Ecommerce_Web.Models;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_Web.Data
{
    public class ApplicationDbContext : DbContext
    {

        DbSet<Product> Products { get; set; }
        DbSet<Category> Categories  { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { 
        
                
        }
    }
}
