using BitcoinMarket.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BitcoinMarket.Repositories.Interfaces
{
    public interface ITradeRepository
    {
        Task<bool> AddTrade(Trade tradeToAdd);
        Task<Trade> GetTradeById(int id);
        Task<List<Trade>> GetAllTradesByUserId(int userId);
        Task<List<Trade>> GetSellTradesByUserId(int userId);
        Task<List<Trade>> GetBuyTradesByUserId(int userId);
        Task<List<Trade>> GetLatestTrades(int page, int pageSize);
        Task<List<Trade>> GetLatestOffers(int page, int pageSize);
    }
}
