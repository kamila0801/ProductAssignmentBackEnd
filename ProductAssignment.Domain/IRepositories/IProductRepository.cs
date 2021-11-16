using System.Collections.Generic;
using ProductAssignment.Core.Models;

namespace ProductAssignment.Domain.IRepositories
{
    public interface IProductRepository
    {
        List<Product> FindAll();
    }
}