using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingWebAPI
{
    public class OpenNewPositionEventArgs
    {
       public OpenPositionInfo Info { get; private set; }

        public OpenNewPositionEventArgs(OpenPositionInfo info)
        {
            this.Info = info;
        }
    }
}
