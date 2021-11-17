using System.Collections.Generic;
using ProductAssignment.Core.Models;

namespace ProductAssignment.Domain.IRepositories
{
    public interface IUserRepository
    {
        List<User> GetAll();
    }
}