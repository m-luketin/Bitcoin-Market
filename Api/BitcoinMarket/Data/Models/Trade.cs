using BitcoinMarket.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BitcoinMarket.Data
{
    public class Trade
    {
        public int Id { get; set; }
        public int SellerId { get; set; }
        public User Seller { get; set; }
        public int? BuyerId { get; set; }
        public User Buyer { get; set; }
        public DateTime TransactionStarted { get; set; }
        public DateTime? TransactionFinished { get; set; }

        [Column(TypeName = "decimal(12,2)")]
        public decimal ValueInUsd { get; set; }

        [Column(TypeName = "decimal(12,4)")]
        public decimal ValueInBtc { get; set; }

        public TradeType Type{ get; set; }
    }
}
