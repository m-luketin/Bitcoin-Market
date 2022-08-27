using BitcoinMarket.Data;
using BitcoinMarket.Data.DTO;
using BitcoinMarket.Data.Enums;
using BitcoinMarket.Data.Models;
using BitcoinMarket.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BitcoinMarket.Repositories.Implementations
{
    public class OrderRepository : IOrderRepository
    {
        private readonly BitcoinMarketDbContext _context;
        public OrderRepository(BitcoinMarketDbContext context)
        {
            _context = context;
        }

        public async Task<string> AddOrder(int userId, bool isBuy, int type, decimal orderValue, decimal limitValue)
        {
            var orderType = (OrderType)type;

            if (orderValue == 0 || (orderType == OrderType.LimitOrder && limitValue == 0))
                return "Invalid value";

            var userToCharge = await _context.Users.FindAsync(userId);
            if (userToCharge == null)
                return "Invalid user";

            var orderToAdd = new Order
            {
                TransactionOwnerId = userId,
                TransactionStarted = DateTime.Now,
                Type = orderType,
                IsBuy = isBuy
            };

            if (isBuy) {
                if (userToCharge.UsdBalance - orderValue <= 0)
                    return "Not enough funds";

                userToCharge.UsdBalance -= orderValue;
                if (orderType == OrderType.LimitOrder)
                {
                    orderToAdd.ValueInBtc = orderValue;
                    orderToAdd.ValueInUsd = limitValue;
                }
                else
                    orderToAdd.ValueInUsd = orderValue;

                _context.Orders.Add(orderToAdd);
                await _context.SaveChangesAsync();

                var bestSales = await GetBestSellOffers();

                if (bestSales.Count > 0)
                {
                    for (var i = 0; orderToAdd.FilledValue < orderToAdd.ValueInUsd && i < bestSales.Count; i++)
                    {
                        var sellOrder = bestSales.ElementAt(i);
                        if (sellOrder.ValueInBtc == 0 || sellOrder.ValueInUsd == 0 || (orderType == OrderType.LimitOrder && sellOrder.ValueInUsd / sellOrder.ValueInBtc > orderToAdd.ValueInUsd / orderToAdd.ValueInBtc))
                            continue;

                        var partialOrderToAdd = new PartialOrder()
                        {
                            BuyOrderId = orderToAdd.Id,
                            SellOrderId = sellOrder.Id,
                        };

                        var unspentBuyingUsd = orderToAdd.ValueInUsd - orderToAdd.FilledValue;
                        var unspentBuyingBtc = unspentBuyingUsd / orderToAdd.ValueInUsd * orderToAdd.ValueInBtc;
                        var unspentSellingUsd = (sellOrder.ValueInBtc - sellOrder.FilledValue) / sellOrder.ValueInBtc * sellOrder.ValueInUsd;

                        if (unspentBuyingUsd == 0 || unspentBuyingBtc == 0 || unspentSellingUsd == 0)
                            continue;

                        if (unspentBuyingUsd > unspentSellingUsd)
                        {
                            partialOrderToAdd.Value = unspentSellingUsd;
                            orderToAdd.FilledValue += unspentSellingUsd;
                            sellOrder.FilledValue = sellOrder.ValueInBtc;
                            sellOrder.TransactionFinished = DateTime.Now;
                        } 
                        else {
                            partialOrderToAdd.Value = unspentBuyingUsd;
                            sellOrder.FilledValue += unspentBuyingBtc; 
                            orderToAdd.FilledValue = orderToAdd.ValueInUsd;
                            orderToAdd.TransactionFinished = DateTime.Now;
                        }

                        _context.PartialOrders.Add(partialOrderToAdd);
                    }
                }

                if (orderType == OrderType.MarketOrder && orderToAdd.FilledValue != orderToAdd.ValueInUsd)
                {
                    _context.Orders.Remove(orderToAdd);
                    await _context.SaveChangesAsync();
                    return "Not enough sales to finish market order for set prices";
                }
            } 
            else if (!isBuy)
            {
                if (userToCharge.BtcBalance - orderValue <= 0)
                    return "Not enough funds";

                userToCharge.BtcBalance -= orderValue;
                orderToAdd.ValueInBtc = orderValue;

                if (orderType == OrderType.LimitOrder)
                    orderToAdd.ValueInUsd = limitValue;

                _context.Orders.Add(orderToAdd);
                await _context.SaveChangesAsync();

                var bestBuys = await GetBestBuyOffers();

                if (bestBuys.Count > 0)
                {
                    for (var i = 0; orderToAdd.FilledValue < orderToAdd.ValueInBtc && i < bestBuys.Count; i++)
                    {
                        var buyOrder = bestBuys.ElementAt(i);
                        if (buyOrder.ValueInUsd == 0 || buyOrder.ValueInBtc == 0 ||(orderType == OrderType.LimitOrder && buyOrder.ValueInUsd / buyOrder.ValueInBtc < orderToAdd.ValueInUsd / orderToAdd.ValueInBtc))
                            continue;
                        var partialOrderToAdd = new PartialOrder()
                        {
                            BuyOrderId = buyOrder.Id,
                            SellOrderId = orderToAdd.Id,
                        };

                        var unspentSellingBtc = orderToAdd.ValueInBtc - orderToAdd.FilledValue;
                        var unspentSellingUsd = unspentSellingBtc / orderToAdd.ValueInBtc * orderToAdd.ValueInUsd;
                        var unspentBuyingBtc = (buyOrder.ValueInUsd - buyOrder.FilledValue) / buyOrder.ValueInUsd * buyOrder.ValueInBtc;
                        if (unspentSellingBtc == 0 || unspentSellingUsd == 0 || unspentBuyingBtc == 0)
                            continue;

                        if (unspentSellingBtc > unspentBuyingBtc)
                        {
                            partialOrderToAdd.Value = unspentBuyingBtc;
                            orderToAdd.FilledValue += unspentBuyingBtc;
                            buyOrder.FilledValue = buyOrder.ValueInUsd;
                            buyOrder.TransactionFinished = DateTime.Now;
                        }
                        else
                        {
                            partialOrderToAdd.Value = unspentSellingBtc;
                            buyOrder.FilledValue += unspentSellingUsd;
                            orderToAdd.FilledValue = orderToAdd.ValueInBtc;
                            orderToAdd.TransactionFinished = DateTime.Now;
                        }

                        _context.PartialOrders.Add(partialOrderToAdd);
                    }
                }

                if (orderType == OrderType.MarketOrder && orderToAdd.FilledValue != orderToAdd.ValueInBtc)
                {
                    _context.Orders.Remove(orderToAdd);
                    await _context.SaveChangesAsync();
                    return "Not enough buys to finish market order for set prices";
                }
            }

            await _context.SaveChangesAsync();
            return "";
        }

        private async Task<List<Order>> GetBestSellOffers()
        {
            return await _context.Orders.Where(t => t.TransactionFinished == null && !t.IsBuy && !t.IsCancelled)
                        .OrderBy(t => t.ValueInUsd / t.ValueInBtc)
                        .ToListAsync();
        }

        private async Task<List<Order>> GetBestBuyOffers()
        {
            return await _context.Orders.Where(t => t.TransactionFinished == null && t.IsBuy && t.ValueInBtc != 0 && t.ValueInUsd != 0 && !t.IsCancelled)
                        .OrderByDescending(t => t.ValueInUsd / t.ValueInBtc)
                        .ToListAsync();
        }

        public async Task<List<Order>> GetLatestOrders(int page, int pageSize)
        {
            return await _context.Orders.Where(t => t.TransactionFinished != null && !t.IsCancelled)
                                    .OrderByDescending(t => t.TransactionFinished)
                                    .Skip(pageSize * page)
                                    .Take(pageSize)
                                    .ToListAsync();
        }

        public async Task<List<Order>> GetLatestSells(int page, int pageSize)
        {
            return await _context.Orders.Where(t => t.TransactionFinished == null && !t.IsBuy && !t.IsCancelled)
                                    .OrderBy(t => t.ValueInUsd)
                                    .Skip(pageSize * page)
                                    .Take(pageSize)
                                    .ToListAsync();
        }


        public async Task<List<Order>> GetLatestBuys(int page, int pageSize)
        {
            return await _context.Orders.Where(t => t.TransactionFinished == null && t.IsBuy && !t.IsCancelled)
                                    .OrderByDescending(t => t.ValueInUsd)
                                    .Skip(pageSize * page)
                                    .Take(pageSize)
                                    .ToListAsync();
        }

        public async Task<Order> GetOrderById(int id)
        {
            return await _context.Orders.FindAsync(id);
        }

        public async Task<List<Order>> GetActiveOrdersByUserId(int userId, int page, int pageSize)
        {
            var user = await _context.Users.Include(u => u.Orders).FirstOrDefaultAsync(u => u.Id == userId);
            if (user.Orders.ToList().Count <= 0)
                return null;

            return user.Orders
                .Where(t => t.TransactionFinished == null && !t.IsCancelled)
                .Skip(pageSize * page)
                .Take(pageSize)
                .OrderByDescending(t => t.TransactionStarted)
                .ToList();
        }

        public async Task<List<Order>> GetFinishedOrdersByUserId(int userId, int page, int pageSize)
        {
            var user = await _context.Users.Include(u => u.Orders).FirstOrDefaultAsync(u => u.Id == userId);
            if (user.Orders.ToList().Count <= 0)
                return null;

            return user.Orders
                .Where(t => t.TransactionFinished != null)
                .Skip(pageSize * page)
                .Take(pageSize)
                .OrderByDescending(t => t.TransactionFinished.Value).ToList();
        }

        public async Task<List<ChartPoint>> AggregateChartData()
        {
            var orders = await _context.Orders.Where(t => t.TransactionFinished != null && !t.IsCancelled).OrderBy(t => t.TransactionFinished).ToListAsync();
            var chartPoints = new List<ChartPoint>();
            var chartPoint = new ChartPoint();
            var previousOrder = orders[0];

            foreach(var order in orders)
            {
                var orderDate = order.TransactionFinished.Value.Date;

                if (!DateTime.Equals(previousOrder.TransactionFinished.Value.Date, orderDate))
                {
                    foreach (DateTime day in EachDay(previousOrder.TransactionFinished.Value.Date, orderDate))
                    {
                        if (chartPoints.Count == 0)
                            break;

                        var tempChartPoint = new ChartPoint
                        {
                            Date = day,
                            Open = chartPoints[chartPoints.Count - 1].Open,
                            High = chartPoints[chartPoints.Count - 1].High,
                            Low = chartPoints[chartPoints.Count - 1].Low,
                            Close = chartPoints[chartPoints.Count - 1].Close
                        };

                        chartPoints.Add(tempChartPoint.PassByValue());
                    }

                    chartPoints.Add(chartPoint.PassByValue());

                    chartPoint.Date = orderDate;
                    chartPoint.Open = order.ValueInUsd;
                    chartPoint.High = order.ValueInUsd;
                    chartPoint.Low = order.ValueInUsd;
                    chartPoint.Close = order.ValueInUsd;
                }
                else
                {
                    chartPoint.Close = order.ValueInUsd;

                    if(order.ValueInUsd > chartPoint.High)
                        chartPoint.High = order.ValueInUsd;

                    if (order.ValueInUsd < chartPoint.Low)
                        chartPoint.Low = order.ValueInUsd;
                }
                previousOrder = order;
            }

            chartPoints.RemoveAt(0); // first element is empty
            return chartPoints.OrderBy(cp => cp.Date).ToList();
        }

        private IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
        {
            for (var day = from.Date.AddDays(1); day.Date < thru.Date; day = day.AddDays(1))
                yield return day;
        }

        public async Task<string> RemoveOrder(int userId, int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if(order == null)
                return "Order does not exist";

            if (order.IsCancelled)
                return "Order already cancelled";

            if (order.TransactionOwnerId != userId)
                return "You are not allowed to delete this order since it is not yours";

            var user = await _context.Users.FindAsync(userId);
            if(order.IsBuy)
                user.UsdBalance += order.ValueInUsd - order.FilledValue;
            else
                user.BtcBalance += order.ValueInBtc - order.FilledValue;

            order.IsCancelled = true;
            order.TransactionFinished = DateTime.Now;
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
            
            return "";
        }
    }
}
