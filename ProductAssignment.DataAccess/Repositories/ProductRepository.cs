using System.Collections.Generic;
using System.IO;
using System.Linq;
using ProductAssignment.Core.Models;
using ProductAssignment.Domain.IRepositories;

namespace ProductAssignment.DataAccess.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly MainDbContext _ctx;
        public ProductRepository(MainDbContext ctx)
        {
            if (ctx == null)
                throw new InvalidDataException("Product Repository must have a dbContext");
            _ctx = ctx;
        }
        public List<Product> FindAll()
        {
            return _ctx.Products
                .Select(pe => new Product
                {
                    Id = pe.Id,
                    Name = pe.Name
                })
                .ToList();
        }
    }
}