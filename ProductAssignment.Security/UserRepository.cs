using System.Collections.Generic;
using System.Linq;
using ProductAssignment.Core.Models;
using ProductAssignment.Domain.IRepositories;

namespace ProductAssignment.Security
{
    public class UserRepository: IUserRepository
    {
        private readonly SecurityContext _context;

        public UserRepository(SecurityContext context)
        {
            _context = context;
        }

        public List<User> GetAll()
        {
            return _context.Users.ToList();
        }

        public User Get(long id)
        {
            return _context.Users.FirstOrDefault(b => b.Id == id);    
        }
        
    }
}