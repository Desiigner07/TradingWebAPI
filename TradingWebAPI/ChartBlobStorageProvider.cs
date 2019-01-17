using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingWebAPI.Interfaces;

namespace TradingWebAPI
{
    public class ChartBlobStorageProvider : IChartProvider
    {
        public ChartBlobStorageProvider(string connectionString)
        {

        }
        public ChartInfo[] GetChart(int count, Delay delay)
        {
            throw new NotImplementedException();
        }

        public ChartInfo GetCurrentChartInfo()
        {
            throw new NotImplementedException();
        }
    }
}
