using System;
using System.Collections.Generic;
using System.IO;
using ProductAssignment.Core;
using ProductAssignment.Core.Models;
using ProductAssignment.Domain.IRepositories;

namespace ProductAssignment.Domain.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;
        public ProductService(IProductRepository repository)
        {
            if (repository == null)
                throw new InvalidDataException("ProductRepository cannot be null");
            _repository = repository;
        }
        public List<Product> GetAllProducts()
        {
            return _repository.FindAll();
        }

        public Product Create(Product newProduct)
        {
            if (newProduct == null)
                throw new ArgumentNullException();
            if (newProduct.Id != 0)
                throw new InvalidDataException("Id cannot be specified");
            if(newProduct.Name==null)
                throw new InvalidDataException("Name must be specified");
            return _repository.Create(newProduct);
        }
    }
}