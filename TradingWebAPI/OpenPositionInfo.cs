using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EtoroWebAPI
{
    public class OpenPositionInfo
    {
        public Share Share { get; private set; }
        public DateTime TimeStamp { get; private set; }
        public BuySell BuySell { get; private set; }
        public int Amount { get; private set; }
        public float Rate { get; private set; }
        public float? TakeProfit { get; private set; }

        public OpenPositionInfo(Share share, DateTime timeStamp, BuySell buySell, int amount, float rate, float takeProfit)
        {
            this.Share = share;
            this.TimeStamp = timeStamp;
            this.BuySell = buySell;
            this.Amount = amount;
            this.Rate = rate;
            this.TakeProfit = takeProfit;
        }

        public OpenPositionInfo(Share share, DateTime timeStamp, BuySell buySell, int amount, float rate)
        {
            this.Share = share;
            this.TimeStamp = timeStamp;
            this.BuySell = buySell;
            this.Amount = amount;
            this.Rate = rate;
        }
    }

    public enum BuySell
    {
        Buy,
        Sell
    }
}
