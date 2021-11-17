using System.Collections.Generic;
using ProductAssignment.Core.Filtering;
using ProductAssignment.Core.Models;

namespace ProductAssignment.Core
{
    public interface IProductService
    {
        List<Product> GetAllProducts(Filter filter);
        Product GetById(int id);
        Product Create(Product newProduct);
        Product Delete(int productId);
    }
}