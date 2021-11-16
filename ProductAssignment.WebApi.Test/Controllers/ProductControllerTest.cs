using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using ProductAssignment.WebApi.Controllers;
using Xunit;

namespace ProductAssignment.WebApi.Test.Controllers
{
    public class ProductControllerTest
    {
        private readonly ProductController _controller;

        public ProductControllerTest()
        {
            _controller = new ProductController();
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
    }
}