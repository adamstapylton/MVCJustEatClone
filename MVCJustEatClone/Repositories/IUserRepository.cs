using MVCJustEatClone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCJustEatClone.Repositories
{
    public interface IUserRepository
    {
        Task<User> RegisterUser(User user);
        Task<User> GetUserByEmailAndPassword(string email, string password);
        Task<bool> CheckUserExistsAsync(string email);
    }
}
