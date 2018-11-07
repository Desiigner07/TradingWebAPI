using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingWebAPI
{
    public class TradeInfo
    {
        public Share Share { get; private set; }
        public DateTime TimeStamp { get; private set; }
        public BuySell BuySell { get; private set; }
        public int Amount { get; private set; }
        public float Rate { get; private set; }
        public float? TakeProfit { get; private set; }
        public float? StopLoss { get; private set; }
        public Guid ID { get; private set; }
        public bool Closed { get; private set; }
        public PositionClosedResult ClosedResult { get; private set; }

        public TradeInfo(Guid id, Share share, BuySell buySell, int amount, float rate, float? takeProfit = null, float? stopLoss = null)
        {
            this.ID = id;
            this.Share = share;
            this.TimeStamp = DateTime.Now;
            this.BuySell = buySell;
            this.Amount = amount;
            this.Rate = rate;
            this.TakeProfit = takeProfit;
            this.StopLoss = stopLoss;
        }

        public TradeInfo(Guid id, Share share, BuySell buySell, int amount, float rate)
        {
            this.ID = id;
            this.Share = share;
            this.TimeStamp = DateTime.Now;
            this.BuySell = buySell;
            this.Amount = amount;
            this.Rate = rate;
        }

        public void ClosePosition(PositionClosedResult result)
        {
            Closed = true;
            this.ClosedResult = result;
        }

        public override string ToString()
        {
            string str = string.Format($"Open {BuySell} Position at {TimeStamp}. \n {Amount} Units @{Rate} | Take Profit: {TakeProfit} | Stop Loss: {StopLoss}");
            return str;
        }
    }

    public enum BuySell
    {
        Buy,
        Sell, 
        Fake
    }
}
