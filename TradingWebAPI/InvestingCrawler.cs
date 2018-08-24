using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingWebAPI
{
    public class InvestingCrawler
    {
        public IWebDriver Driver { get; private set; }

        private const string INVESTING_OIL_THURSDAY_URL = "https://de.investing.com/economic-calendar/api-weekly-crude-stock-656";
        private const string INVESTING_OIL_WEDNESDAY_URL = "https://de.investing.com/economic-calendar/eia-crude-oil-inventories-75";

        public InvestingCrawler()
        {
            this.StartBrowser();

        }

        private void StartBrowser()
        {
            FirefoxOptions options = new FirefoxOptions()
            {
                BrowserExecutableLocation = @"C:\Program Files\Firefox Developer Edition\firefox.exe"
            };
            this.Driver = new FirefoxDriver(options);
        }

        public Trend GetTrend()
        {
            //Look at actual Datetime for actual Trend

            this.Driver.Url = INVESTING_OIL_THURSDAY_URL;
            System.Threading.Thread.Sleep(2000);
            return GetActualTrend();
        }


        public Trend GetActualTrend()
        {
            string xpath = @"/html/body/div[5]/section/div[6]/span[2]/div";
            IWebElement valueElement = this.Driver.FindElement(By.XPath(xpath));

            string valueString = valueElement.Text.Remove(valueElement.Text.Length - 1);
            float value = float.Parse(valueString);

            if (value < 0)
            {
                return Trend.UpTrend;
            }
            else
            {
                return Trend.DownTrend;
            }

        }
    }
}
