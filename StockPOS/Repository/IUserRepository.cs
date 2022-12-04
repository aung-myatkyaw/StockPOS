using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StockPOS.Models;

namespace StockPOS.Repository
{
    public interface IUserRepository : IRepositoryBase<User>
    {
        Task<bool> CheckExistingUserName(string username);
    }
}