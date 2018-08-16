using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EtoroWebAPI
{
    public interface ICrawler
    {
        void Login(string username, string pwd);

        void SwitchMode();
        void GoToDemo();
        void GoToReal();

        void OpenBuyPosition(Share share);

        void OpenSellPosition(Share share);

        void OpenBuyPosition(Share share, int takeProfitInPercent);

        void OpenSellPosition(Share share, int takeProfitInPercent);

        void OpenBuyOrder(Share share, int takeProfitInPercent);

        void OpenSellOrder(Share share, int takeProfitInPercent);



    }
}
