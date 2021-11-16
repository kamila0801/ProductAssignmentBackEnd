using System.Collections.Generic;
using ProductAssignment.Core.Models;

namespace ProductAssignment.Core
{
    public interface IProductService
    {
        List<Product> GetAllProducts();
        Product Create(Product newProduct);
    }
}