using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ProductAssignment.Core;
using ProductAssignment.Core.Models;
using ProductAssignment.WebApi.Controllers;
using Xunit;

namespace ProductAssignment.WebApi.Test.Controllers
{
    public class ProductControllerTest
    {
        private readonly ProductController _controller;
        private readonly Mock<IProductService> _mockService;

        public ProductControllerTest()
        {
           _mockService = new Mock<IProductService>();
            _controller = new ProductController(_mockService.Object);
        }
        
        [Fact]
        public void ProductController_IsOfTypeControllerBase()
        {
            Assert.IsAssignableFrom<ControllerBase>(_controller);
        }

        [Fact]
        public void ProductController_UsesApiControllerAttribute()
        {
            var typeInfo = typeof(ProductController).GetTypeInfo();
            var attribute = typeInfo.GetCustomAttributes()
                .FirstOrDefault(a => a.GetType().Name.Equals("ApiControllerAttribute"));
            Assert.NotNull(attribute);
        }
        
        [Fact]
        public void ProductController_UsesRouteAttribute_WithParamApiControllerNameRoute()
        {
            //Arrange
            var typeInfo = typeof(ProductController).GetTypeInfo();
            var attr = typeInfo.GetCustomAttributes().FirstOrDefault(a => a.GetType()
                .Name.Equals("RouteAttribute"));
            //Assert
            var routeAttr = attr as RouteAttribute;
            Assert.Equal("api/[Controller]", routeAttr.Template);
        }
        
        [Fact]
        public void ProductController_UsesRouteAttribute()
        {
            //Arrange
            var typeInfo = typeof(ProductController).GetTypeInfo();
            var attr = typeInfo.GetCustomAttributes().FirstOrDefault(a => a.GetType()
                .Name.Equals("RouteAttribute"));
            //Assert
            Assert.NotNull(attr);
        }
        
        [Fact]
        public void ProductController_WithNullProductService_ThrowsInvalidDataException()
        {
            Assert.Throws<InvalidDataException>(
                () => new ProductController(null));
        }
        
        [Fact]
        public void ProductController_WithNullProductService_ThrowsInvalidDataExceptionWithMessage()
        {
            var exception = Assert.Throws<InvalidDataException>(
                () => new ProductController(null));
            Assert.Equal("Product service cannot be null", exception.Message);
        }

        #region create

        [Fact]
        public void ProductController_HasPostMethod()
        {
            var method = typeof(ProductController).GetMethods().FirstOrDefault( m => 
                "Post".Equals(m.Name));
            Assert.NotNull(method);
        }
        
        [Fact]
        public void ProductController_PostMethod_IsPublic()
        {
            var method = typeof(ProductController).GetMethods().FirstOrDefault( m => 
                "Post".Equals(m.Name));
            Assert.True(method.IsPublic);
        }
        
        [Fact]
        public void ProductController_PostMethod_ReturnsCreatedStudentInActionResult()
        {
            var method = typeof(ProductController).GetMethods().FirstOrDefault( m => 
                "Post".Equals(m.Name));
            Assert.Equal(typeof(ActionResult<Product>).FullName, method.ReturnType.FullName);
        }
        
        [Fact]
        public void Post_HasPostHttpAttribute()
        {
            var methodInfo = typeof(ProductController).
                GetMethods().FirstOrDefault(m => m.Name == "Post");
            var attr = methodInfo.CustomAttributes.FirstOrDefault(ca
                => ca.AttributeType.Name == "HttpPostAttribute");
            Assert.NotNull(attr);
        }
        
        [Fact]
        public void Post_CallsService_OnlyOnce()
        {
            //Arrange
            var prod = new Product() {Name = "namee"};
            //Act
            _controller.Post(prod);
            //Assert
            _mockService.Verify(s => s.Create(prod), Times.Once);
        }
        #endregion
        
    }
}