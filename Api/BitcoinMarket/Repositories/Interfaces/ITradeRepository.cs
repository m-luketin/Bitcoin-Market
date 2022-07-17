using BitcoinMarket.Data;
using BitcoinMarket.Data.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BitcoinMarket.Repositories.Interfaces
{
    public interface ITradeRepository
    {
        Task<string> AddTrade(int userId, bool IsBuy, int type, decimal tradeValue, decimal limitValue);
        Task<Trade> GetTradeById(int id);
        Task<List<Trade>> GetActiveTradesByUserId(int userId, int page, int pageSize);
        Task<List<Trade>> GetFinishedTradesByUserId(int userId, int page, int pageSize);
        Task<List<Trade>> GetLatestTrades(int page, int pageSize);
        Task<List<Trade>> GetLatestSells(int page, int pageSize);
        Task<List<Trade>> GetLatestBuys(int page, int pageSize);
        Task<List<ChartPoint>> AggregateChartData();
        Task<string> RemoveTrade(int userId, int tradeId);

    }
}
