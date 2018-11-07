using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingWebAPI.Interfaces
{
    public interface ICrawler
    {
        void Refresh();

        void Login(string username, string pwd);
        bool TryRemoveStartDialog();
        bool IsConnected();
        void SwitchMode();
        void GoToDemo();
        void GoToReal();

        bool OpenPositionsLeft();

        void CheckAllOpenPositions();

        bool OpenBuyPosition(Share share, Guid id, int units, int takeProfitInPercent, int stopLossInPercent);
        bool OpenSellPosition(Share share, Guid id, int units, int takeProfitInPercent, int stopLossInPercent);

        PositionClosedResult GetPositionClosedResult(TradeInfo info);
        PositionClosedResult GetPositionClosedResult(Guid id);

        TradeInfo GetOpenPositionInfo(Guid id);

        void OpenBuyOrder(Share share, int units, int takeProfitInPercent, float orderRate);
        void OpenSellOrder(Share share, int units, int takeProfitInPercent, float orderRate);

        float GetOpenPositionValue(TradeInfo openPosition);

        float? GetCurrentBuyPrice(Share share);
        float? GetCurrentSellPrice(Share share);

        event EventHandler<OpenNewPositionEventArgs> OnOpenNewPosition;
        event EventHandler<PositionClosedEventArgs> OnPositionClosed;
    }
}
