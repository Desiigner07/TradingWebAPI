using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingWebAPI
{
    public class OpenFakePositionInfo : OpenPositionInfo
    {

        public OpenFakePositionInfo(Share share, float rate, float takeProfit, float stopLoss) : base(share, DateTime.Now, BuySell.Fake, 0, rate, takeProfit, stopLoss)
        {

        }

    }
}
