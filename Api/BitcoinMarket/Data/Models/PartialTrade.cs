using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BitcoinMarket.Data.Models
{
    public class PartialTrade
    {
        public int PartialTradeId { get; set; }
        public int? SellTradeId { get; set; }
        public Trade SellTrade { get; set; }
        public int? BuyTradeId { get; set; }
        public Trade BuyTrade { get; set; }

        [Column(TypeName = "decimal(12,4)")]
        public decimal Value { get; set; }
    }
}
