using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingWebAPI
{
    public class ChartInfo
    {
        public float CurrentPrice { get; private set; }

        public float High { get; private set; }

        public float Low { get; private set; }

        public float Open { get; private set; }

        public float Volume { get; private set; }

        public float Change { get; private set; }
    }
}
