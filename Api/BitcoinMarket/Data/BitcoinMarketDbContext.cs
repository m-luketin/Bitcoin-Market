using BitcoinMarket.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BitcoinMarket.Data
{
    public class BitcoinMarketDbContext : DbContext
    {
        public BitcoinMarketDbContext(DbContextOptions options) : base(options)
        { }

        public DbSet<User> Users { get; set; }
        public DbSet<Trade> Trades { get; set; }
        public DbSet<PartialTrade> PartialTrades { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Trade>().HasOne(to => to.TransactionOwner).WithMany(t => t.Trades).HasForeignKey(t => t.TransactionOwnerId).OnDelete(DeleteBehavior.ClientSetNull);
            modelBuilder.Entity<User>().HasMany(u => u.Trades).WithOne(t => t.TransactionOwner).HasForeignKey(t => t.TransactionOwnerId).OnDelete(DeleteBehavior.ClientSetNull);
            modelBuilder.Entity<Trade>().HasMany(to => to.PartialBuyTrades).WithOne(t => t.BuyTrade).HasForeignKey(t => t.BuyTradeId).OnDelete(DeleteBehavior.ClientSetNull);
            modelBuilder.Entity<PartialTrade>().HasOne(pt => pt.BuyTrade).WithMany(bt => bt.PartialBuyTrades).HasForeignKey(pt => pt.BuyTradeId).OnDelete(DeleteBehavior.ClientSetNull);
            modelBuilder.Entity<Trade>().HasMany(to => to.PartialSellTrades).WithOne(t => t.SellTrade).HasForeignKey(t => t.SellTradeId).OnDelete(DeleteBehavior.ClientSetNull);
            modelBuilder.Entity<PartialTrade>().HasOne(pt => pt.SellTrade).WithMany(bt => bt.PartialSellTrades).HasForeignKey(pt => pt.SellTradeId).OnDelete(DeleteBehavior.ClientSetNull);
            
            modelBuilder.Entity<User>().Property(u => u.BtcBalance).HasPrecision(20, 10);
            modelBuilder.Entity<Trade>().Property(u => u.ValueInBtc).HasPrecision(20, 10);

            var intListValueConverter = new ValueConverter<List<int>, string>(
            i => string.Join(",", i),
            s => string.IsNullOrWhiteSpace(s) ? new List<int>() : s.Split(new[] { ',' }).Select(v => int.Parse(v)).ToList());

            base.OnModelCreating(modelBuilder);
        }
    }
}
