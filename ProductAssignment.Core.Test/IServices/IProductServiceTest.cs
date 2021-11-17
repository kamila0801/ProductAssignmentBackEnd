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