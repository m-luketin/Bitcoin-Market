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
    public class TradeRepository : ITradeRepository
    {
        private readonly BitcoinMarketDbContext _context;
        public TradeRepository(BitcoinMarketDbContext context)
        {
            _context = context;
        }

        public async Task<string> AddTrade(int userId, bool isBuy, int type, decimal tradeValue, decimal limitValue)
        {
            var tradeType = (TradeType)type;

            if (tradeValue == 0 || (tradeType == TradeType.LimitOrder && limitValue == 0))
                return "Invalid value";

            var userToCharge = await _context.Users.FindAsync(userId);
            if (userToCharge == null)
                return "Invalid user";

            var tradeToAdd = new Trade
            {
                TransactionOwnerId = userId,
                TransactionStarted = DateTime.Now,
                Type = tradeType,
                IsBuy = isBuy
            };

            if (isBuy) {
                if (userToCharge.UsdBalance - tradeValue <= 0)
                    return "Not enough funds";

                userToCharge.UsdBalance -= tradeValue;
                if (tradeType == TradeType.LimitOrder)
                {
                    tradeToAdd.ValueInBtc = tradeValue;
                    tradeToAdd.ValueInUsd = limitValue;
                }
                else
                    tradeToAdd.ValueInUsd = tradeValue;

                _context.Trades.Add(tradeToAdd);
                await _context.SaveChangesAsync();

                var bestSales = await GetBestSellOffers();

                if (bestSales.Count > 0)
                {
                    for (var i = 0; tradeToAdd.FilledValue < tradeToAdd.ValueInUsd && i < bestSales.Count; i++)
                    {
                        var sellTrade = bestSales.ElementAt(i);
                        if (sellTrade.ValueInBtc == 0 || sellTrade.ValueInUsd == 0 || (tradeType == TradeType.LimitOrder && sellTrade.ValueInUsd / sellTrade.ValueInBtc > tradeToAdd.ValueInUsd / tradeToAdd.ValueInBtc))
                            continue;

                        var partialTradeToAdd = new PartialTrade()
                        {
                            BuyTradeId = tradeToAdd.Id,
                            SellTradeId = sellTrade.Id,
                        };

                        var unspentBuyingUsd = tradeToAdd.ValueInUsd - tradeToAdd.FilledValue;
                        var unspentBuyingBtc = unspentBuyingUsd / tradeToAdd.ValueInUsd * tradeToAdd.ValueInBtc;
                        var unspentSellingUsd = (sellTrade.ValueInBtc - sellTrade.FilledValue) / sellTrade.ValueInBtc * sellTrade.ValueInUsd;

                        if (unspentBuyingUsd == 0 || unspentBuyingBtc == 0 || unspentSellingUsd == 0)
                            continue;

                        if (unspentBuyingUsd > unspentSellingUsd)
                        {
                            partialTradeToAdd.Value = unspentSellingUsd;
                            tradeToAdd.FilledValue += unspentSellingUsd;
                            sellTrade.FilledValue = sellTrade.ValueInBtc;
                            sellTrade.TransactionFinished = DateTime.Now;
                        } 
                        else {
                            partialTradeToAdd.Value = unspentBuyingUsd;
                            sellTrade.FilledValue += unspentBuyingBtc; 
                            tradeToAdd.FilledValue = tradeToAdd.ValueInUsd;
                            tradeToAdd.TransactionFinished = DateTime.Now;
                        }

                        _context.PartialTrades.Add(partialTradeToAdd);
                    }
                }

                if (tradeType == TradeType.MarketOrder && tradeToAdd.FilledValue != tradeToAdd.ValueInUsd)
                {
                    _context.Trades.Remove(tradeToAdd);
                    await _context.SaveChangesAsync();
                    return "Not enough sales to finish market order for set prices";
                }
            } 
            else if (!isBuy)
            {
                if (userToCharge.BtcBalance - tradeValue <= 0)
                    return "Not enough funds";

                userToCharge.BtcBalance -= tradeValue;
                tradeToAdd.ValueInBtc = tradeValue;

                if (tradeType == TradeType.LimitOrder)
                    tradeToAdd.ValueInUsd = limitValue;

                _context.Trades.Add(tradeToAdd);
                await _context.SaveChangesAsync();

                var bestBuys = await GetBestBuyOffers();

                if (bestBuys.Count > 0)
                {
                    for (var i = 0; tradeToAdd.FilledValue < tradeToAdd.ValueInBtc && i < bestBuys.Count; i++)
                    {
                        var buyTrade = bestBuys.ElementAt(i);
                        if (buyTrade.ValueInUsd == 0 || buyTrade.ValueInBtc == 0 ||(tradeType == TradeType.LimitOrder && buyTrade.ValueInUsd / buyTrade.ValueInBtc < tradeToAdd.ValueInUsd / tradeToAdd.ValueInBtc))
                            continue;
                        var partialTradeToAdd = new PartialTrade()
                        {
                            BuyTradeId = buyTrade.Id,
                            SellTradeId = tradeToAdd.Id,
                        };

                        var unspentSellingBtc = tradeToAdd.ValueInBtc - tradeToAdd.FilledValue;
                        var unspentSellingUsd = unspentSellingBtc / tradeToAdd.ValueInBtc * tradeToAdd.ValueInUsd;
                        var unspentBuyingBtc = (buyTrade.ValueInUsd - buyTrade.FilledValue) / buyTrade.ValueInUsd * buyTrade.ValueInBtc;
                        if (unspentSellingBtc == 0 || unspentSellingUsd == 0 || unspentBuyingBtc == 0)
                            continue;

                        if (unspentSellingBtc > unspentBuyingBtc)
                        {
                            partialTradeToAdd.Value = unspentBuyingBtc;
                            tradeToAdd.FilledValue += unspentBuyingBtc;
                            buyTrade.FilledValue = buyTrade.ValueInUsd;
                            buyTrade.TransactionFinished = DateTime.Now;
                        }
                        else
                        {
                            partialTradeToAdd.Value = unspentSellingBtc;
                            buyTrade.FilledValue += unspentSellingUsd;
                            tradeToAdd.FilledValue = tradeToAdd.ValueInBtc;
                            tradeToAdd.TransactionFinished = DateTime.Now;
                        }

                        _context.PartialTrades.Add(partialTradeToAdd);
                    }
                }

                if (tradeType == TradeType.MarketOrder && tradeToAdd.FilledValue != tradeToAdd.ValueInBtc)
                {
                    _context.Trades.Remove(tradeToAdd);
                    await _context.SaveChangesAsync();
                    return "Not enough buys to finish market order for set prices";
                }
            }

            await _context.SaveChangesAsync();
            return "";
        }

        private async Task<List<Trade>> GetBestSellOffers()
        {
            return await _context.Trades.Where(t => t.TransactionFinished == null && !t.IsBuy)
                        .OrderBy(t => t.ValueInUsd / t.ValueInBtc)
                        .ToListAsync();
        }

        private async Task<List<Trade>> GetBestBuyOffers()
        {
            return await _context.Trades.Where(t => t.TransactionFinished == null && t.IsBuy && t.ValueInBtc != 0 && t.ValueInUsd != 0)
                        .OrderByDescending(t => t.ValueInUsd / t.ValueInBtc)
                        .ToListAsync();
        }

        public async Task<List<Trade>> GetLatestTrades(int page, int pageSize)
        {
            return await _context.Trades.Where(t => t.TransactionFinished != null)
                                    .OrderByDescending(t => t.TransactionFinished)
                                    .Skip(pageSize * page)
                                    .Take(pageSize)
                                    .ToListAsync();
        }

        public async Task<List<Trade>> GetLatestSells(int page, int pageSize)
        {
            return await _context.Trades.Where(t => t.TransactionFinished == null && !t.IsBuy)
                                    .OrderBy(t => t.ValueInUsd)
                                    .Skip(pageSize * page)
                                    .Take(pageSize)
                                    .ToListAsync();
        }


        public async Task<List<Trade>> GetLatestBuys(int page, int pageSize)
        {
            return await _context.Trades.Where(t => t.TransactionFinished == null && t.IsBuy)
                                    .OrderByDescending(t => t.ValueInUsd)
                                    .Skip(pageSize * page)
                                    .Take(pageSize)
                                    .ToListAsync();
        }

        public async Task<Trade> GetTradeById(int id)
        {
            return await _context.Trades.FindAsync(id);
        }

        public async Task<List<Trade>> GetActiveTradesByUserId(int userId, int page, int pageSize)
        {
            var user = await _context.Users.Include(u => u.Trades).FirstOrDefaultAsync(u => u.Id == userId);
            if (user.Trades.ToList().Count <= 0)
                return null;

            return user.Trades
                .Where(t => t.TransactionFinished == null)
                .Skip(pageSize * page)
                .Take(pageSize)
                .OrderByDescending(t => t.TransactionStarted)
                .ToList();
        }

        public async Task<List<Trade>> GetFinishedTradesByUserId(int userId, int page, int pageSize)
        {
            var user = await _context.Users.Include(u => u.Trades).FirstOrDefaultAsync(u => u.Id == userId);
            if (user.Trades.ToList().Count <= 0)
                return null;

            return user.Trades
                .Where(t => t.TransactionFinished != null)
                .Skip(pageSize * page)
                .Take(pageSize)
                .OrderByDescending(t => t.TransactionFinished.Value).ToList();
        }

        public async Task<List<ChartPoint>> AggregateChartData()
        {
            var trades = await _context.Trades.Where(t => t.TransactionFinished != null).OrderBy(t => t.TransactionFinished).ToListAsync();
            var dates = new List<DateTime>();
            var chartPoints = new List<ChartPoint>();
            var chartPoint = new ChartPoint();

            foreach(var trade in trades)
            {
                var tradeDate = trade.TransactionFinished.Value.Date;

                if (chartPoint.Date.Date != tradeDate)
                {
                    chartPoints.Add(chartPoint.PassByValue());

                    dates.Add(tradeDate);
                    chartPoint.Date = tradeDate;
                    chartPoint.Open = trade.ValueInUsd;
                    chartPoint.High = trade.ValueInUsd;
                    chartPoint.Low = trade.ValueInUsd;
                    chartPoint.Close = trade.ValueInUsd;
                }
                else
                {
                    chartPoint.Close = trade.ValueInUsd;

                    if(trade.ValueInUsd > chartPoint.High)
                        chartPoint.High = trade.ValueInUsd;

                    if (trade.ValueInUsd < chartPoint.Low)
                        chartPoint.Low = trade.ValueInUsd;
                }
            }

            chartPoints.RemoveAt(0); // first element is empty
            return chartPoints;
        }

        public async Task<string> RemoveTrade(int userId, int tradeId)
        {
            var trade = await _context.Trades.FindAsync(tradeId);
            if(trade == null)
                return "Trade does not exist";

            if (trade.TransactionOwnerId != userId)
                return "You are not allowed to delete this trade since it is not yours";

            var user = await _context.Users.FindAsync(userId);
            if(trade.IsBuy)
                user.UsdBalance += trade.ValueInUsd - trade.FilledValue;
            else
                user.BtcBalance += trade.ValueInBtc - trade.FilledValue;

            _context.Trades.Remove(trade);
            await _context.SaveChangesAsync();
            
            return "";
        }

    }
}
