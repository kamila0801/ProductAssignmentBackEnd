using EntityFrameworkCore.Testing.Moq;
using Microsoft.EntityFrameworkCore;
using ProductAssignment.Core.Models;
using ProductAssignment.DataAccess.Entities;
using Xunit;

namespace ProductAssignment.DataAccess.Test
{
    public class DbContextTest
    {
        private readonly MainDbContext _mockedDbContext;

        public DbContextTest()
        {
            _mockedDbContext = Create.MockedDbContextFor<MainDbContext>();
        }

        [Fact]
        public void DbContext_WithDbContextOptions_IsAvailable()
        {
            Assert.NotNull(_mockedDbContext);
        }

        [Fact]
        public void DbContext_DbSets_MustHaveSetTypeProductEntity()
        {
            Assert.True(_mockedDbContext.Products is DbSet<ProductEntity>);
        }
    }
}