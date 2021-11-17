using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ProductAssignment.Core;
using ProductAssignment.Core.Filtering;
using ProductAssignment.Core.Models;
using ProductAssignment.WebApi.Controllers;
using ProductAssignment.WebApi.Dtos;
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
        
        #region controller initialization
        
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
        
        #endregion
        
        #region getAll

        [Fact]
        public void ProductController_HasGetAllMethod()
        {
            var method = typeof(ProductController)
                .GetMethods()
                .FirstOrDefault(m => "GetAll".Equals(m.Name));
            Assert.NotNull(method);
        }
        
        [Fact]
        public void GetAllMethod_IsPublic()
        {
            var method = typeof(ProductController)
                .GetMethods()
                .FirstOrDefault(m => "GetAll".Equals(m.Name));
            Assert.True(method is not null && method.IsPublic);
        }
        
        
        [Fact]
        public void GetAllMethod_ReturnsListOfProductsAsActionResult()
        {
            var method = typeof(ProductController)
                .GetMethods()
                .FirstOrDefault(m => "GetAll".Equals(m.Name));
            Assert.Equal(typeof(ActionResult<List<Product>>).FullName, method?.ReturnType.FullName);
        }

        [Fact]
        public void GetAll_HasHttpGetAttribute()
        {
            var methodInfo = typeof(ProductController)
                .GetMethods()
                .FirstOrDefault(m => m.Name == "GetAll");
            var attr = methodInfo.CustomAttributes
                .FirstOrDefault(ca => ca.AttributeType.Name == "HttpGetAttribute");
            Assert.NotNull(attr);
        }

        [Fact]
        public void GetAll_CallsServiceOnce()
        {
            Filter filter = new Filter {CurrentPage = 1, ItemsPrPage = 2};
            _controller.GetAll(filter);
            _mockService.Verify(s=>s.GetAllProducts(filter), Times.Once);
        }

        [Fact]
        public void GetAll_NullParam_ThrowsInvalidDataException()
        {
            Assert.Throws<InvalidDataException>(() => _controller.GetAll(null));
        }
        
        [Fact]
        public void GetAll_NullParam_ThrowsExceptionMessage()
        {
            var ex = Assert.Throws<InvalidDataException>(() => _controller.GetAll(null));
            Assert.Equal("Filter cannot be null", ex.Message);
        }

        [Fact]
        public void GetAll_FilterPageZero_ReturnsBadRequest()
        {
            var invalidFilter = new Filter {CurrentPage = 0};
            Assert.Equal(400, (_controller.GetAll(invalidFilter).Result as ObjectResult)?.StatusCode);
        }
        
        [Fact]
        public void GetAll_FilterPageLessThanZero_ReturnsBadRequest()
        {
            var invalidFilter = new Filter {CurrentPage = -1};
            Assert.Equal(400, (_controller.GetAll(invalidFilter).Result as ObjectResult)?.StatusCode);

        }
        
        [Fact]
        public void GetAll_FilterItemsLessThanZero_ReturnsBadRequest()
        {
            var invalidFilter = new Filter {CurrentPage = 1, ItemsPrPage = -1};
            Assert.Equal(400, (_controller.GetAll(invalidFilter).Result as ObjectResult)?.StatusCode);
        }
        
        #endregion
        
        #region getById
        
        [Fact]
        public void ProductController_HasGetByIdMethod()
        {
            var method = typeof(ProductController)
                .GetMethods()
                .FirstOrDefault(m => "GetById".Equals(m.Name));
            Assert.NotNull(method);
        }
        
        [Fact]
        public void GetById_IsPublic()
        {
            var method = typeof(ProductController)
                .GetMethods()
                .FirstOrDefault(m => "GetById".Equals(m.Name));
            Assert.True(method is not null && method.IsPublic);
        }
        
        
        [Fact]
        public void GetAllMethod_ReturnsProductAsActionResult()
        {
            var method = typeof(ProductController)
                .GetMethods()
                .FirstOrDefault(m => "GetById".Equals(m.Name));
            Assert.Equal(typeof(ActionResult<Product>).FullName, method?.ReturnType.FullName);
        }

        [Fact]
        public void GetById_HasHttpGetAttribute()
        {
            var methodInfo = typeof(ProductController)
                .GetMethods()
                .FirstOrDefault(m => m.Name == "GetAll");
            var attr = methodInfo.CustomAttributes
                .FirstOrDefault(ca => ca.AttributeType.Name == "HttpGetAttribute");
            Assert.NotNull(attr);
        }

        [Fact]
        public void GetById_ParamLessThanZero_ReturnsBadRequest()
        {
            Assert.Equal(400, (_controller.GetById(-1).Result as ObjectResult)?.StatusCode);
        }

        #endregion

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
        
        
        //TODO test fails after added dto ?? (program works properly)
        [Fact]
        public void Post_CallsService_OnlyOnce()
        {
            //Arrange
            var prod = new PostProductDto {Name = "namee"};
            var product = new Product {Name = prod.Name};
            //Act
            _controller.Post(prod);
            //Assert
            _mockService.Verify(s => s.Create(product), Times.Once);
        }
        #endregion

        #region update

        [Fact]
        public void ProductController_HasUpdateMethod()
        {
            var method = typeof(ProductController)
                .GetMethods()
                .FirstOrDefault(m => "Update".Equals(m.Name));
            Assert.NotNull(method);
        }
        
        [Fact]
        public void UpdateMethod_IsPublic()
        {
            var method = typeof(ProductController)
                .GetMethods()
                .FirstOrDefault(m => "Update".Equals(m.Name));
            Assert.True(method is not null && method.IsPublic);
        }
        
        
        [Fact]
        public void UpdateMethod_ReturnsProductAsActionResult()
        {
            var method = typeof(ProductController)
                .GetMethods()
                .FirstOrDefault(m => "Update".Equals(m.Name));
            Assert.Equal(typeof(ActionResult<Product>).FullName, method?.ReturnType.FullName);
        }

        [Fact]
        public void UpdateMethod_HasHttpPutAttribute()
        {
            var methodInfo = typeof(ProductController)
                .GetMethods()
                .FirstOrDefault(m => m.Name == "Update");
            var attr = methodInfo.CustomAttributes
                .FirstOrDefault(ca => ca.AttributeType.Name == "HttpPutAttribute");
            Assert.NotNull(attr);
        }

        [Fact]
        public void UpdateMethod_NullParam_ThrowsInvalidDataException()
        {
            var ex = Assert.Throws<InvalidDataException>(() => _controller.Update(null));
            Assert.Equal("product to update cannot be null", ex.Message);
        }
        
        [Fact]
        public void UpdateMethod_InvalidProductProperties_ReturnBadRequest()
        {
            var product = new PutProductDto
            {
                Id = -1,
                Name = "tom",
                Color = "blue",
                Price = 2.0
            };
            
            Assert.Equal(400, (_controller.Update(product).Result as ObjectResult)?.StatusCode);

            product.Id = 1;
            product.Name = "";
            Assert.Equal(400, (_controller.Update(product).Result as ObjectResult)?.StatusCode);

            product.Name = "tom";
            product.Color = "";
            Assert.Equal(400, (_controller.Update(product).Result as ObjectResult)?.StatusCode);

            product.Color = "blue";
            product.Price = -1;
            Assert.Equal(400, (_controller.Update(product).Result as ObjectResult)?.StatusCode);
        }

        #endregion
        
    }
}