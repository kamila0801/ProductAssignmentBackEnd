using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using EntityFrameworkCore.Testing.Moq;
using ProductAssignment.Core.Filtering;
using ProductAssignment.Core.Models;
using ProductAssignment.DataAccess.Entities;
using ProductAssignment.DataAccess.Repositories;
using ProductAssignment.Domain.IRepositories;
using Xunit;

namespace ProductAssignment.DataAccess.Test.Repositories
{
    public class ProductRepositoryTest
    {
        private readonly MainDbContext _mockedDbContext;
        private readonly ProductRepository _repository;

        public ProductRepositoryTest()
        {
            _mockedDbContext = Create.MockedDbContextFor<MainDbContext>();
            _repository = new ProductRepository(_mockedDbContext);
        }
        
        [Fact]
        public void ProductRepository_IsIProductRepo()
        {
            Assert.IsAssignableFrom<IProductRepository>(_repository);
        }

        [Fact]
        public void ProductRepository_WithNullDbCtx_ThrowsInvalidDataException()
        {
            Assert.Throws<InvalidDataException>(() => new ProductRepository(null));
        }
        
        [Fact]
        public void ProductRepository_WithNullDbCtx_ThrowsExceptionWithMessage()
        {
            var ex = Assert.Throws<InvalidDataException>(() => new ProductRepository(null));
            Assert.Equal("Product Repository must have a dbContext", ex.Message);
        }

        #region get all
        
        [Fact]
        public void FindAll_GetAllProductsEntitiesInDbContext_AsListOfProducts()
        {
            var list = new List<ProductEntity>
            {
                new ProductEntity {Id = 1, Name = "first"},
                new ProductEntity {Id = 2, Name = "second"},
                new ProductEntity {Id = 3, Name = "third"}
            };
            _mockedDbContext.Set<ProductEntity>().AddRange(list);
            _mockedDbContext.SaveChanges();

            var expected = list
                .Select(pe => new Product
                {
                    Id = pe.Id,
                    Name = pe.Name
                })
                .ToList();
            
            Assert.Equal(expected, _repository.FindAll(new Filter{CurrentPage = 1, ItemsPrPage = 3}), new Comparer());
        }
        
        #endregion

        #region get by id

        [Fact]
        public void GetById_GetProductEntityInDbContext_AsProduct()
        {
            var productEntity = new ProductEntity {Id = 1, Name = "one"};
            _mockedDbContext.Set<ProductEntity>().AddRange(productEntity);
            _mockedDbContext.SaveChanges();

            var expected = new Product {Id = 1, Name = "one"};
            
            Assert.Equal(expected.Name, productEntity.Name);
            Assert.Equal(expected.Id, productEntity.Id);
        }
        

        #endregion
    }

    public class Comparer : IEqualityComparer<Product>
    {
        public bool Equals(Product x, Product y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;
            if (x.GetType() != y.GetType()) return false;
            return x.Id == y.Id && x.Name == y.Name;
        }

        public int GetHashCode(Product obj)
        {
            return HashCode.Combine(obj.Id, obj.Name);
        }
    }
    
    
}