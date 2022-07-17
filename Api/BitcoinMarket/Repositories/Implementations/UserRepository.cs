using BitcoinMarket.Data;
using BitcoinMarket.Data.DTO;
using BitcoinMarket.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BitcoinMarket.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly BitcoinMarketDbContext _context;
        public UserRepository(BitcoinMarketDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserById(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User> GetUserByUsername(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<bool> Login(string username, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username & u.Password == password);
            if (user == null)
                return false;

            return true;
        }

        public async Task<bool> Register(string username, string password)
        { 
            var userToAdd = new User { Username = username, Password = password };
            await _context.Users.AddAsync(userToAdd);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<string> SetUserBalance(int userId, decimal usdBalance, decimal btcBalance)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return "User not found";

            user.UsdBalance = usdBalance;
            user.BtcBalance = btcBalance;
            await _context.SaveChangesAsync();
            return "";
        }
    }
}
