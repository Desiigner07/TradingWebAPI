using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingWebAPI
{
    public class UrlStorage
    {
        private static UrlStorage _instance;
        public static UrlStorage Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new UrlStorage();
                }
                return _instance;
            }
        }

        private UrlStorage()
        {
            this.ShareUrlStorage.Add(Share.Brent_Oil, "/Energy/BrentOil");
            this.ShareUrlStorage.Add(Share.Crude_Oil, "/Energy/OIL");
            this.ShareUrlStorage.Add(Share.Heating_Oil, "/Energy/HeatingOil");
            this.ShareUrlStorage.Add(Share.Natural_Gas, "/Energy/NaturalGas");
        }

        private const string MARKETS_LIVE_URL = "https://live-trader.markets.com/trading-platform/#trading";
        private const string MARKETS_DEMO_URL = "https://demo-trader.markets.com/trading-platform/#trading";

        private Dictionary<Share, string> ShareUrlStorage = new Dictionary<Share, string>();

        public string GetShareUrl(Share share, bool demo)
        {
            if (demo)
            {
                return GetDemoShareUrl(share);
            }
            else
            {
                return GetLiveShareUrl(share);
            }
        }

        private string GetDemoShareUrl(Share share)
        {
            string url = MARKETS_DEMO_URL + ShareUrlStorage[share];
            return url;
        }

        private string GetLiveShareUrl(Share share)
        {

            string url = MARKETS_LIVE_URL + ShareUrlStorage[share];
            return url;
        }
    }
}
