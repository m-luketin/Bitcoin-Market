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
        public DbSet<Order> Orders { get; set; }
        public DbSet<PartialOrder> PartialOrders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>().HasOne(to => to.TransactionOwner).WithMany(t => t.Orders).HasForeignKey(t => t.TransactionOwnerId).OnDelete(DeleteBehavior.ClientSetNull);
            modelBuilder.Entity<User>().HasMany(u => u.Orders).WithOne(t => t.TransactionOwner).HasForeignKey(t => t.TransactionOwnerId).OnDelete(DeleteBehavior.ClientSetNull);
            modelBuilder.Entity<Order>().HasMany(to => to.PartialBuyOrders).WithOne(t => t.BuyOrder).HasForeignKey(t => t.BuyOrderId).OnDelete(DeleteBehavior.ClientSetNull);
            modelBuilder.Entity<PartialOrder>().HasOne(pt => pt.BuyOrder).WithMany(bt => bt.PartialBuyOrders).HasForeignKey(pt => pt.BuyOrderId).OnDelete(DeleteBehavior.ClientSetNull);
            modelBuilder.Entity<Order>().HasMany(to => to.PartialSellOrders).WithOne(t => t.SellOrder).HasForeignKey(t => t.SellOrderId).OnDelete(DeleteBehavior.ClientSetNull);
            modelBuilder.Entity<PartialOrder>().HasOne(pt => pt.SellOrder).WithMany(bt => bt.PartialSellOrders).HasForeignKey(pt => pt.SellOrderId).OnDelete(DeleteBehavior.ClientSetNull);
            
            modelBuilder.Entity<User>().Property(u => u.BtcBalance).HasPrecision(20, 10);
            modelBuilder.Entity<Order>().Property(u => u.ValueInBtc).HasPrecision(20, 10);

            var intListValueConverter = new ValueConverter<List<int>, string>(
            i => string.Join(",", i),
            s => string.IsNullOrWhiteSpace(s) ? new List<int>() : s.Split(new[] { ',' }).Select(v => int.Parse(v)).ToList());

            base.OnModelCreating(modelBuilder);
        }
    }
}
