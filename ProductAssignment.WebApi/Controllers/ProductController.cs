using System;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using ProductAssignment.Core;
using ProductAssignment.Core.Models;

namespace ProductAssignment.WebApi.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService ?? throw new InvalidDataException("Product service cannot be null");
        }

        [HttpPost]
        public ActionResult<Product> Post([FromBody] Product product)
        {
            try
            {
                return Ok(_productService.Create(product));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}