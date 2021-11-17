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
                return BadRequest("Current page cannot be 0");
            if (filter.CurrentPage < 0)
                return BadRequest("Current page cannot be less than 0");
            if (filter.ItemsPrPage < 0)
                return BadRequest("Items per page cannot be less than 0");
            return Ok(_productService.GetAllProducts(filter));
        }
        
        [HttpGet("{id}")]
        public ActionResult<Product> GetById(int id)
        {
            if (id < 0)
                return BadRequest("id cannot be less than 0");
            return Ok(_productService.GetById(id));
        }

        [HttpPost]
        public ActionResult<Product> Post([FromBody] PostProductDto productDto)
        {
            if (productDto == null)
                throw new InvalidDataException("product cannot be null");
            if (productDto.Name is null or "")
                return BadRequest("name cannot be empty");
            if (productDto.Price <= 0)
                return BadRequest("price must be greater than 0");
            if (productDto.Color is null or "")
                return BadRequest("color cannot be empty");
                
            try
            {
                return Ok(_productService.Create(new Product
                {
                    Name = productDto.Name,
                    Color = productDto.Color,
                    Price = productDto.Price
                }));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id}")]
        public ActionResult<PostProductDto> DeleteProduct(int id)
        {
            var product = _productService.Delete(id);
            var dto = new PostProductDto()
            {
                Name = product.Name,
                Color = product.Color,
                Price = product.Price
            };
            return Ok(dto);
        }

        [HttpPut]
        public ActionResult<Product> Update([FromBody] PutProductDto productDto)
        {
            if (productDto == null)
                throw new InvalidDataException("product to update cannot be null");
            if (productDto.Id < 0)
                return BadRequest("id cannot be less than 0");
            if (productDto.Name==null | productDto.Name.Equals(""))
                return BadRequest("name cannot be empty");
            if (productDto.Price <= 0)
                return BadRequest("price must be greater than 0");
            if (productDto.Color==null | productDto.Color.Equals(""))
                return BadRequest("color cannot be empty");
            
            return _productService.Update(new Product
            {
                Id = productDto.Id,
                Name = productDto.Name,
                Color = productDto.Color,
                Price = productDto.Price
            });

        }
    }
}