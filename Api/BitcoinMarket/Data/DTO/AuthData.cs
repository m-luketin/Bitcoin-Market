using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BitcoinMarket.Data.Models
{
    public class AuthData
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public long TokenExpirationTime { get; set; }
    }
}
