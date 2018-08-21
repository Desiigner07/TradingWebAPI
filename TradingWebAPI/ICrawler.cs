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

        void OpenBuyPosition(Share share, int units);

        void OpenSellPosition(Share share, int units);

        void OpenBuyPosition(Share share, int units, int takeProfitInPercent);

        void OpenSellPosition(Share share, int units, int takeProfitInPercent);

        void OpenBuyOrder(Share share, int units, int takeProfitInPercent);

        void OpenSellOrder(Share share, int units, int takeProfitInPercent);

        float GetOpenPositionValue(OpenPositionInfo openPosition);

    }
}
