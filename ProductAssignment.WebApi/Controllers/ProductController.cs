using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using ProductAssignment.Core;
using ProductAssignment.Core.Filtering;
using ProductAssignment.Core.Models;
using ProductAssignment.WebApi.Dtos;

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

        [HttpGet]
        public ActionResult<List<Product>> GetAll([FromQuery] Filter filter)
        {
            if (filter == null)
                throw new InvalidDataException("Filter cannot be null");
            if (filter.CurrentPage == 0)
                throw new InvalidDataException("Current page cannot be 0");
            if (filter.CurrentPage < 0)
                throw new InvalidDataException("Current page cannot be less than 0");
            if (filter.ItemsPrPage < 0)
                throw new InvalidDataException("Items per page cannot be less than 0");
            return Ok(_productService.GetAllProducts(filter));
        }
        
        [HttpGet("{id}")]
        public ActionResult<Product> GetById(int id)
        {
            if (id < 0)
                throw new InvalidDataException("id cannot be less than 0");
            return Ok(_productService.GetById(id));
        }

        [HttpPost]
        public ActionResult<Product> Post([FromBody] PostProductDto productDto)
        {
            try
            {
                return Ok(_productService.Create(new Product{Name = productDto.Name}));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}