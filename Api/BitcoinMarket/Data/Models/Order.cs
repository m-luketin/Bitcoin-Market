using BitcoinMarket.Data.Enums;
using BitcoinMarket.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BitcoinMarket.Data
{
    public class Order
    {
        public int Id { get; set; }
        public int? TransactionOwnerId { get; set; }
        
        public User TransactionOwner { get; set; }
        public ICollection<PartialOrder> PartialBuyOrders { get; set; }
        public ICollection<PartialOrder> PartialSellOrders { get; set; }
        public DateTime TransactionStarted { get; set; }
        public DateTime? TransactionFinished { get; set; }

        [Column(TypeName = "decimal(12,2)")]
        public decimal ValueInUsd { get; set; }

        [Column(TypeName = "decimal(12,4)")]
        public decimal ValueInBtc { get; set; }

        [Column(TypeName = "decimal(12,2)")]
        public decimal FilledValue { get; set; }

        public bool IsBuy { get; set; }
        public bool IsCancelled { get; set; }
        public OrderType Type{ get; set; }

    }
}
