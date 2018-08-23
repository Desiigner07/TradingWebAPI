using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TradingWebAPI.Test
{
    [TestClass]
    public class MarketsDotComCrawlerTest
    {
        [TestMethod]
        public void SzenarioTest_01()
        {
            MarketsDotComCrawler crawler = new MarketsDotComCrawler(true);

            crawler.Login("p.pohly@gmx.de", "pierre2512");
            crawler.GoToDemo();

            float price = crawler.GetCurrentBuyPrice(Share.Brent_Oil);
        }
    }
}
