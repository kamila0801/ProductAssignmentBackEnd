using Microsoft.EntityFrameworkCore;
using ProductAssignment.DataAccess.Entities;

namespace ProductAssignment.DataAccess
{
    public class MainDbContext : DbContext
    {
        
        public MainDbContext(DbContextOptions<MainDbContext> options) : base(options)
        {
            
        }
        
        public virtual DbSet<ProductEntity> Products { get; set; }
    }
}