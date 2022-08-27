using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BitcoinMarket.Data.Models
{
    public class PartialOrder
    {
        public int PartialOrderId { get; set; }
        public int? SellOrderId { get; set; }
        public Order SellOrder { get; set; }
        public int? BuyOrderId { get; set; }
        public Order BuyOrder { get; set; }

        [Column(TypeName = "decimal(12,4)")]
        public decimal Value { get; set; }
    }
}
