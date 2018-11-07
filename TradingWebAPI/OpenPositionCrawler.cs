using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingWebAPI
{
    public class OpenPositionCrawler
    {
        public TradeInfo Info { get; private set; }

        public IWebDriver Driver { get; private set; }

        public OpenPositionCrawler(TradeInfo info)
        {
            this.Info = info;
            FirefoxOptions options = new FirefoxOptions()
            {
                BrowserExecutableLocation = @"C:\Program Files\Firefox Developer Edition\firefox.exe"
            };
            this.Driver = new FirefoxDriver(options);
        }


    }
}
