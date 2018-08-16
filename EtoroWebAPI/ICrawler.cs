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

        void OpenVirtualMode();

        void OpenBuyPosition(Share share);

        void OpenSellPosition(Share share);

        void OpenBuyPosition(Share share, float takeProfit);

        void OpenSellPosition(Share share, float takeProfit);

        void OpenBuyOrder(Share share, float takeProfit);

        void OpenSellOrder(Share share, float takeProfit);



    }
}
