using BitcoinMarket.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BitcoinMarket.Data.DTO
{
    public class LoginResponseData : AuthData
    {
        public decimal UsdBalance { get; set; }

        public decimal BtcBalance { get; set; }
    }
}
