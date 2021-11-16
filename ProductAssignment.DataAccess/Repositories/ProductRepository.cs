using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ProductAssignment.Core.Models;
using ProductAssignment.DataAccess.Entities;
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

        public Product Create(Product newProduct)
        {
            _ctx.Products.Add(new ProductEntity
            {
                Id = newProduct.Id,
                Name = newProduct.Name
            }).State = EntityState.Added;
            _ctx.SaveChanges();
            return newProduct;
        }
    }
}