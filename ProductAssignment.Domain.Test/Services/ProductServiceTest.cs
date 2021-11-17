using System;
using System.Collections.Generic;
using System.IO;
using Moq;
using ProductAssignment.Core;
using ProductAssignment.Core.Filtering;
using ProductAssignment.Core.Models;
using ProductAssignment.Domain.IRepositories;
using ProductAssignment.Domain.Services;
using Xunit;

namespace ProductAssignment.Domain.Test.Services
{
    public class ProductServiceTest
    {
        private readonly Mock<IProductRepository> _mock;
        private readonly ProductService _service;

        public ProductServiceTest()
        {
            _mock = new Mock<IProductRepository>();
            _service = new ProductService(_mock.Object);
        }
        [Fact]
        public void ProductService_IsIProductService()
        {
            Assert.True(_service is IProductService);
        }

        [Fact]
        public void ProductService_WithNullIProductRepo_ThrowsInvalidDataException()
        {
            Assert.Throws<InvalidDataException>(() => new ProductService(null));
        }
        
        [Fact]
        public void ProductService_WithNullIProductRepo_ThrowsExWithMessage()
        {
            var ex = Assert.Throws<InvalidDataException>(() => new ProductService(null));
            Assert.Equal("ProductRepository cannot be null", ex.Message);
        }

        //TODO doesnt work after adding filter
        /* 
        [Fact]
        public void GetAllProducts_CallsProductRepositoriesFindAll_ExactlyOnce()
        {
            _service.GetAllProducts(new Filter());
            _mock.Verify(r=>r.FindAll(new Filter()), Times.Once);
        }

        */

        #region Create
        
        [Fact]
        public void Create_ReturnsCreatedProductWithId()
        {
            var passedProduct = new Product()
            {
                Name = "kuba",
            };
            var expectedStudent = new Product
            {
                Id = 1,
                Name = "kuba",
            };
            _mock.Setup(repo => repo.Create(passedProduct)).Returns(expectedStudent);
            var actualStudent = _service.Create(passedProduct);
            Assert.Equal(expectedStudent, actualStudent);
        }

        [Fact]
        public void Create_ProductNull_ThrowsArgumentNullException()
        {
            Product invalidProduct = null;
            void Actual() => _service.Create(invalidProduct);
            Assert.Throws<ArgumentNullException>(Actual);
        }

        [Fact]
        public void Create_IdIsSpecified_ThrowsInvalidDataException()
        {
            var invalidProduct = new Product()
            {
                Id = 1,
                Name = "kuba",
            };
            void Actual() => _service.Create(invalidProduct);
            Assert.Throws<InvalidDataException>(Actual);
        }
        [Fact]
        public void Create_IdIsSpecified_ThrowsInvalidDataExceptionWithMessage()
        {
            var invalidProduct = new Product()
            {
                Id = 1,
                Name = "kuba",
            };
            void Actual() => _service.Create(invalidProduct);
            var exception = Assert.Throws<InvalidDataException>(Actual);
            Assert.Equal("Id cannot be specified", exception.Message);
        }
        
        [Fact]
        public void Create_NameIsntSpecified_ThrowsInvalidDataException()
        {
            var invalidProduct = new Product();
            void Actual() => _service.Create(invalidProduct);
            Assert.Throws<InvalidDataException>(Actual);
        }
        
        [Fact]
        public void Create_NameIsntSpecified_ThrowsInvalidDataExceptionWithMessage()
        {
            var invalidProduct = new Product()
            {
            };
            void Actual() => _service.Create(invalidProduct);
            var exception = Assert.Throws<InvalidDataException>(Actual);
            Assert.Equal("Name must be specified", exception.Message);
        }
        #endregion

        #region Delete
        [Fact]
        public void ProductService_Delete_ParameterProduct_ReturnProduct()
        {
            // Arrange
            var product = new Product()
            {
                Id = 1,
                Name = "Laszlo"
            };
            _mock.Setup(r => r.GetById(product.Id))
                .Returns(product);
            _mock.Setup(r => r.Delete(product.Id))
                .Returns(product);
            // Act
            var actual = _service.Delete(product.Id);
            // Assert
            Assert.Equal(product,actual);
        }
        
        [Fact]
        public void DeleteProduct_WithParams_CallsProductRepositoryOnce()
        {
            
            var productId = (int) 1;
            
            //Act
            _service.Delete(productId);
            
            //Assert
            _mock.Verify(r => r.Delete(productId), Times.Once);
        }
        

        #endregion

    }
}