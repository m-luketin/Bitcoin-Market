using BitcoinMarket.Data;
using BitcoinMarket.Data.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BitcoinMarket.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetUserById(int id);

        Task<User> GetUserByUsername(string username);

        Task<BuyerSeller> GetUsersByTradeId(int tradeId);         

        Task<bool> Login(string username, string password);

        Task<bool> Register(User userToAdd);
    }
}
