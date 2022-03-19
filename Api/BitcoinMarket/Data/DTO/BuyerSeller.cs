using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BitcoinMarket.Data.DTO
{
    public class BuyerSeller
    {
        public User Buyer { get; set; }
        public User Seller { get; set; }

        public BuyerSeller(User buyer, User seller)
        {
            Buyer = buyer;
            Seller = seller;
        }
    }
}
