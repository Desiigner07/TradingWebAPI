using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingWebAPI
{
    public interface ICrawler
    {
        void Login(string username, string pwd);

        void SwitchMode();
        void GoToDemo();
        void GoToReal();

        OpenPositionInfo OpenBuyPosition(Share share, int units);

        OpenPositionInfo OpenSellPosition(Share share, int units);

        OpenPositionInfo OpenBuyPosition(Share share, int units, int takeProfitInPercent);

        OpenPositionInfo OpenSellPosition(Share share, int units, int takeProfitInPercent);

        void OpenBuyOrder(Share share, int units, int takeProfitInPercent, float orderRate);

        void OpenSellOrder(Share share, int units, int takeProfitInPercent, float orderRate);

        float GetOpenPositionValue(OpenPositionInfo openPosition);

    }
}
