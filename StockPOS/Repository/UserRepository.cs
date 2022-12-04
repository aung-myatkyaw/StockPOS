using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StockPOS.Models;

namespace StockPOS.Repository
{
    public class UserRepository: RepositoryBase<User>, IUserRepository
    {
        public UserRepository(StockPOSContext repositoryContext)
            :base(repositoryContext)
        {
        }

        public async Task<bool> CheckExistingUserName(string username)
        {
            return await RepositoryContext.Users.AnyAsync(e => e.UserName == username);
        }
    }
}