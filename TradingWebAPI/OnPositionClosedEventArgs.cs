namespace TradingWebAPI
{
    public class PositionClosedEventArgs
    {
        public TradeInfo Info { get; private set; }

        public PositionClosedResult ClosedResult { get; private set; }

        public PositionClosedEventArgs(TradeInfo info, PositionClosedResult result)
        {
            this.Info = info;
            this.ClosedResult = result;
        }
    }
}
