using ProductAssignment.DataAccess.Entities;

namespace ProductAssignment.DataAccess.Test
{
    public class DbSeeder
    {
        private readonly MainDbContext _ctx;

        public DbSeeder(MainDbContext ctx)
        {
            _ctx = ctx;
        }

        public void SeedDevelopment()
        {
            _ctx.Database.EnsureDeleted(); //no database now
            _ctx.Database.EnsureCreated();
            
            _ctx.Products.Add(new ProductEntity {Name = "blue lego"});
            _ctx.Products.Add(new ProductEntity {Name = "red lego"});
            _ctx.Products.Add(new ProductEntity {Name = "purple lego"});
            _ctx.SaveChanges();
        }
        
    }
}