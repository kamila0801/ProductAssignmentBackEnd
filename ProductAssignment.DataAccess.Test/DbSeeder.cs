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
            _ctx.Database.EnsureDeleted();
            _ctx.Database.EnsureCreated();
            
            _ctx.Products.Add(new ProductEntity
            {
                Name = "blue lego",
                Color = "blue",
                Price = 12.25
            });
            _ctx.Products.Add(new ProductEntity
            {
                Name = "red lego",
                Color = "red",
                Price = 11.25
            });
            _ctx.Products.Add(new ProductEntity
            {
                Name = "purple lego",
                Color = "yellow",
                Price = 13.00
            });
            _ctx.SaveChanges();
        }
        
    }
}