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

        Task<List<User>> GetAllUsers(int id);

        Task<User> GetUserByUsername(string username);

        Task<bool> Login(string username, string password);

        Task<bool> Register(string username, string password);

        Task<string> SetUserBalance(int userId, decimal usdBalance, decimal btcBalance);

        Task<bool> RemoveUser(int userId);

        bool IsUserAdmin(int userId);
    }
}
