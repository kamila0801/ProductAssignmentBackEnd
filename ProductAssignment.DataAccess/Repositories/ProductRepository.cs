using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ProductAssignment.Core.Filtering;
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
        public List<Product> FindAll(Filter filter)
        {
            return _ctx.Products
                .Select(pe => new Product
                {
                    Id = pe.Id,
                    Name = pe.Name
                })
                .Skip((filter.CurrentPage - 1) * filter.ItemsPrPage)
                .Take(filter.ItemsPrPage)
                .ToList();
        }
        
        public Product GetById(int id)
        {
            var entity = _ctx.Products
                .FirstOrDefault(pe => pe.Id == id);
            return (entity!=null ? new Product {Name = entity.Name, Id = entity.Id} : null);
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