using BitcoinMarket.Data;
using BitcoinMarket.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BitcoinMarket.Repositories.Implementations
{
    public class TradeRepository : ITradeRepository
    {
        private readonly BitcoinMarketDbContext _context;
        public TradeRepository(BitcoinMarketDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddTrade(Trade tradeToAdd)
        {
            _context.Trades.Add(tradeToAdd);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Trade>> GetLatestTrades(int page, int pageSize)
        {
            return await _context.Trades.Where(t => t.TransactionFinished != null)
                                    .OrderByDescending(t => t.TransactionFinished)
                                    .Skip(pageSize * page)
                                    .Take(pageSize)
                                    .ToListAsync();
        }

        public async Task<List<Trade>> GetLatestOffers(int page, int pageSize)
        {
            return await _context.Trades.Where(t => t.TransactionFinished == null)
                                    .OrderByDescending(t => t.TransactionFinished)
                                    .Skip(pageSize * page)
                                    .Take(pageSize)
                                    .ToListAsync();
        }

        public async Task<Trade> GetTradeById(int id)
        {
            return await _context.Trades.FindAsync(id);
        }

        public async Task<List<Trade>> GetAllTradesByUserId(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            var buyerTrades = user.BuyerTrades;
            var sellerTrades = user.SellerTrades;

            return buyerTrades.Concat(sellerTrades).ToList();
        }

        public async Task<List<Trade>> GetSellTradesByUserId(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            return user.SellerTrades.ToList();
        }

        public async Task<List<Trade>> GetBuyTradesByUserId(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            return user.BuyerTrades.ToList();
        }
    }
}
