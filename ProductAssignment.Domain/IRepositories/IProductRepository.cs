using System.Collections.Generic;
using ProductAssignment.Core.Filtering;
using ProductAssignment.Core.Models;

namespace ProductAssignment.Domain.IRepositories
{
    public interface IProductRepository
    {
        List<Product> FindAll(Filter filter);
        Product GetById(int id);
        Product Create(Product newProduct);
        Product Delete(int productId);
    }
}