using System.Collections.Generic;
using Moq;
using ProductAssignment.Core.Filtering;
using ProductAssignment.Core.Models;
using Xunit;

namespace ProductAssignment.Core.Test
{
    public class IProductServiceTest
    {
        [Fact]
        public void IProductService_IsAvailabe()
        {
            var service = new Mock<IProductService>().Object;
            Assert.NotNull(service);
        }

        #region get all
        [Fact]
        public void GetAllProducts_ReturnsListOfAllProducts()
        { 
            var filter = new Filter();
            var mock = new Mock<IProductService>(); 
            mock.Setup(s => s.GetAllProducts(filter))
                .Returns(new List<Product>()); 
            var service = mock.Object; 
            Assert.Equal(new List<Product>(), service.GetAllProducts(filter));
        }
        #endregion
        
        
        #region update

        [Fact]
        public void UpdateProduct_ReturnsUpdatedProduct()
        {
            var product = new Product {Id = 1, Name = "tom", Color = "blue", Price = 21.05};
            var mock = new Mock<IProductService>();
            mock.Setup(s => s.Update(product))
                .Returns(product);
            var service = mock.Object;
            Assert.Equal(product, service.Update(product));
        }
        #endregion
        
        #region Delete Test
        [Fact]
        public void DeleteProduct_WithParams_ReturnsDeletedProduct()
        {
            var serviceMock = new Mock<IProductService>();
            var pId = 1;
            serviceMock
                .Setup(s => s.Delete(pId))
                .Returns(new Product());
            Assert.NotNull(serviceMock.Object);
        }
        #endregion
    }
}