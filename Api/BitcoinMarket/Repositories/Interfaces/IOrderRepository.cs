using BitcoinMarket.Data;
using BitcoinMarket.Data.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BitcoinMarket.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        Task<string> AddOrder(int userId, bool IsBuy, int type, decimal orderValue, decimal limitValue);
        Task<Order> GetOrderById(int id);
        Task<List<Order>> GetActiveOrdersByUserId(int userId, int page, int pageSize);
        Task<List<Order>> GetFinishedOrdersByUserId(int userId, int page, int pageSize);
        Task<List<Order>> GetLatestOrders(int page, int pageSize);
        Task<List<Order>> GetLatestSells(int page, int pageSize);
        Task<List<Order>> GetLatestBuys(int page, int pageSize);
        Task<List<ChartPoint>> AggregateChartData();
        Task<string> RemoveOrder(int userId, int orderId);

    }
}
