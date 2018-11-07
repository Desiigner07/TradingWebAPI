using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingWebAPI
{
    public class OpenNewPositionEventArgs
    {
       public TradeInfo Info { get; private set; }

        public OpenNewPositionEventArgs(TradeInfo info)
        {
            this.Info = info;
        }
    }
}
