using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BitcoinMarket.Data.DTO
{
    public class ChartPoint
    {
        public ChartPoint PassByValue()
        {
            return new ChartPoint()
            {
                Date = Date,
                Open = Open,
                High = High,
                Low = Low,
                Close = Close
            };
        }

        public DateTime Date { get; set; }
        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Close { get; set; }
    }
}
