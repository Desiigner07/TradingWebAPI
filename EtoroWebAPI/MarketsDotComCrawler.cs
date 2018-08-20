using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EtoroWebAPI
{
    public class MarketsDotComCrawler : ICrawler
    {
        private const string MARKETS_URL = "https://www.markets.com/de/";
        private const string MARKETS_LIVE_URL = "https://live-trader.markets.com/trading-platform/";
        private const string MARKETS_DEMO_URL = "https://demo-trader.markets.com/trading-platform/";

        public List<OpenPositionInfo> OpenPositions { get; private set; }


        public IWebDriver Driver { get; private set; }

        private bool browserStarted = false;
        private bool DemoMode = false;

        public MarketsDotComCrawler(bool demoMode)
        {
            this.OpenPositions = new List<OpenPositionInfo>();
            this.DemoMode = demoMode;
            this.StartBrowser();
        }

        private void StartBrowser()
        {
            if (!browserStarted)
            {
                FirefoxOptions options = new FirefoxOptions()
                {
                    BrowserExecutableLocation = @"C:\Program Files\Firefox Developer Edition\firefox.exe"
                };
                this.Driver = new FirefoxDriver(options);
                this.Driver.Url = MARKETS_URL;
                browserStarted = true;
            }
        }

        public void Login(string username, string pwd)
        {
            string Xpath = "/html/body/div[1]/div/div[2]/span/div/div/div/div[2]/ul[1]/li[8]";
            IWebElement loginElement = this.Driver.FindElement(By.XPath(Xpath));
            loginElement.Click();

            Xpath = "/html/body/div[1]/div[4]/div[3]/div/div/div/div/div/div/form/div/div[3]/div[1]/input";
            IWebElement inputNameElement = this.Driver.FindElement(By.XPath(Xpath));

            Xpath = "/html/body/div[1]/div[4]/div[3]/div/div/div/div/div/div/form/div/div[3]/div[2]/input";
            IWebElement inputPwdElement = this.Driver.FindElement(By.XPath(Xpath));

            Xpath = "//*[@id=\"auth-button-login\"]";
            IWebElement loginButton = this.Driver.FindElement(By.XPath(Xpath));

            inputNameElement.SendKeys(username);
            inputPwdElement.SendKeys(pwd);

            loginButton.Click();
        }

        #region Orders 

        public void OpenBuyOrder(Share share, int units, int takeProfitInPercent)
        {
            throw new NotImplementedException();
        }

        public void OpenSellOrder(Share share, int units, int takeProfitInPercent)
        {
            throw new NotImplementedException();
        }
        #endregion 

        private void SelectShare(Share share)
        {
            string url = DemoMode ? MARKETS_DEMO_URL : MARKETS_LIVE_URL;
            string shareUrl = String.Format("{0}/{1}/{2}", url, "#instrument", share.ToString());

            this.Driver.Url = shareUrl;
        }

        public float GetCurrentBuyPrice(Share share)
        {
            SelectShare(share);

            string xpath = @"/html/body/div[1]/div[4]/div[1]/div[2]/div/div[1]/div[2]/div/div/div/div[2]/div[2]/div[1]/div[2]/table/tbody/tr/td/div/button";
            IWebElement BuyElement = this.Driver.FindElement(By.XPath(xpath));

            xpath = @"/div[2]/div[1]/span[1]/";
            IWebElement rateElement = BuyElement.FindElement(By.XPath(xpath));

            string currentPrice = rateElement.Text + rateElement.FindElement(By.XPath("/i[1]")).Text;

            return float.Parse(currentPrice);
        }

        public float GetCurrentSellPrice(Share share)
        {
            SelectShare(share);

            string xpath = @"/html/body/div[1]/div[4]/div[1]/div[2]/div/div[1]/div[2]/div/div/div/div[2]/div[2]/div[1]/div[1]/table/tbody/tr/td/div/button";
            IWebElement BuyElement = this.Driver.FindElement(By.XPath(xpath));

            xpath = @"/div[2]/div[1]/span[1]/";
            IWebElement rateElement = BuyElement.FindElement(By.XPath(xpath));

            string currentPrice = rateElement.Text + rateElement.FindElement(By.XPath("/i[1]")).Text;

            return float.Parse(currentPrice);
        }

        public void OpenBuyPosition(Share share, int units)
        {
            SelectShare(share);
        }

        public void OpenBuyPosition(Share share, int units, int takeProfitInPercent)
        {
            SelectShare(share);

            string xpath = @"/html/body/div[1]/div[4]/div[1]/div[2]/div/div[1]/div[2]/div/div/div/div[2]/div[2]/div[1]/div[2]/table/tbody/tr/td/div/button";
            IWebElement BuyElement = this.Driver.FindElement(By.XPath(xpath));

            xpath = @"/div[2]/div[1]/span[1]/";
            IWebElement rateElement = BuyElement.FindElement(By.XPath(xpath));

            string currentPrice = rateElement.Text + rateElement.FindElement(By.XPath("/i[1]")).Text;

            float currentBuyPrice = float.Parse(currentPrice);
            float takeProfit = GetTakeProfitValue(currentBuyPrice, takeProfitInPercent);
            DateTime timeStamp = DateTime.Now;

            BuyElement.Click();

            //Add input for order
        }


        public void OpenSellPosition(Share share, int units)
        {
            SelectShare(share);
        }

        public void OpenSellPosition(Share share, int units, int takeProfitInPercent)
        {
            SelectShare(share);

            string xpath = @"/html/body/div[1]/div[4]/div[1]/div[2]/div/div[1]/div[2]/div/div/div/div[2]/div[2]/div[1]/div[1]/table/tbody/tr/td/div/button";
            IWebElement SellElement = this.Driver.FindElement(By.XPath(xpath));
        }


        public float GetOpenBuyPositionValue(OpenPositionInfo openPosition)
        {
            string xpath = @"/html/body/div[1]/div[4]/div[1]/div[1]/div[1]/div[2]/div/div/div/div[1]/div[2]/div/div[2]";
            IWebElement openPositionsElement = this.Driver.FindElement(By.XPath(xpath));

            return 0;
        }

        public float GetOpenSellPositionValue(OpenPositionInfo openPosition)
        {
            throw new NotImplementedException();
        }

        #region Virtual or Real
        public void SwitchMode()
        {
            string xPath = "//*[@id=\"account-switch-checkbox\"]";
            IWebElement switchElement = this.Driver.FindElement(By.Id(xPath));

            switchElement.Click();
        }

        public void GoToDemo()
        {
            string virtualURl = "https://demo-trader.markets.com/trading-platform/#trading/Energy/";

            this.Driver.Url = virtualURl;
        }

        public void GoToReal()
        {
            string realUrl = "https://live-trader.markets.com/trading-platform/#trading/Energy/";

            this.Driver.Url = realUrl;
        }

        #endregion

        #region Helpers

        public static float GetTakeProfitValue(float currentPrice, int perc)
        {
            float percent = perc / 100;

            float takeProfit = (currentPrice * percent) / 10;               //Der Hebel ist 1 : 10

            return currentPrice + takeProfit;

        }


        #endregion

    }
}
