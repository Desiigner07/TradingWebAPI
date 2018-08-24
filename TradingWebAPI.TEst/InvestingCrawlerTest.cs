using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TradingWebAPI.Test
{
    [TestClass]
    public class InvestingCrawlerTest
    {
        [TestMethod]
        public void GetTrend_SzenarioTest01()
        {
            InvestingCrawler crawler = new InvestingCrawler();

            Trend trend = crawler.GetTrend();

            Assert.IsNotNull(trend);
        }
    }
}
