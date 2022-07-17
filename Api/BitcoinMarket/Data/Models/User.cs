using BitcoinMarket.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BitcoinMarket.Data
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        [Column(TypeName = "decimal(12,2)")]
        public decimal UsdBalance { get; set; }

        [Column(TypeName = "decimal(12,4)")]
        public decimal BtcBalance { get; set; }

        public ICollection<Trade> Trades { get; set; }
    }
}
