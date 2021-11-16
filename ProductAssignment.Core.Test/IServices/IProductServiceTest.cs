using System.Collections.Generic;
using Moq;
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
        public void GetAllProducts_NoParams_ReturnsListOfAllProducts()
        {
            var mock = new Mock<IProductService>();
            mock.Setup(s => s.GetAllProducts())
                .Returns(new List<Product>());
            var service = mock.Object;
            Assert.Equal(new List<Product>(), service.GetAllProducts());
        }
    }
}