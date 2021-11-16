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
    }
}