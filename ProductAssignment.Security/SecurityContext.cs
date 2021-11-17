using Microsoft.EntityFrameworkCore;
using ProductAssignment.Core.Models;

namespace ProductAssignment.Security
{
    public class SecurityContext : DbContext
    {
        public SecurityContext(DbContextOptions contextOptions) : base(contextOptions)
        {
        }

        public DbSet<User> Users { get; set; }
    }
}