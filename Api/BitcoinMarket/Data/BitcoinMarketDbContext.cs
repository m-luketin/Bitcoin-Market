using Microsoft.EntityFrameworkCore;
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
    }
}
