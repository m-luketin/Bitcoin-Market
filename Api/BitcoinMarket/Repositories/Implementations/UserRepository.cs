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

        public async Task<List<User>> GetAllUsers(int userId)
        {
            if (!IsUserAdmin(userId))
                return null;

            var allUsers = await _context.Users.ToListAsync();
            var userToExclude = await _context.Users.FindAsync(userId);
            allUsers.Remove(userToExclude);
            return allUsers;
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

        public async Task<bool> RemoveUser(int userId)
        {
            var userOrders = _context.Orders.Where(o => o.TransactionOwnerId == userId).Include(o => o.PartialBuyOrders).Include(o => o.PartialSellOrders);

            foreach (var userOrder in userOrders)
            {
                foreach (var partialSellOrder in userOrder.PartialSellOrders)
                {
                    _context.PartialOrders.Remove(partialSellOrder);
                }

                foreach (var partialBuyOrder in userOrder.PartialSellOrders)
                {
                    _context.PartialOrders.Remove(partialBuyOrder);
                }

                _context.Orders.Remove(userOrder);
            }

            var userToDelete = _context.Users.Find(userId);
            _context.Users.Remove(userToDelete);

            await _context.SaveChangesAsync();
            return true;
        }

        public bool IsUserAdmin(int userId)
        {
            return _context.Users.Any(u => u.Id == userId && u.isAdmin);
        }
    }
}
