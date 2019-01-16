using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingWebAPI.Interfaces
{
    public interface IChartProvider
    {
        ChartInfo GetCurrentChartInfo();

        ChartInfo[] GetChart(int count, Delay delay);
        
    }
}
