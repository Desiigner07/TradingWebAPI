using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingWebAPI
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

        public OpenPositionInfo OpenBuyPosition(Share share, int units)
        {
            SelectShare(share);

            string xpath = @"/html/body/div[1]/div[4]/div[1]/div[2]/div/div[1]/div[2]/div/div/div/div[2]/div[2]/div[1]/div[2]/table/tbody/tr/td/div/button";
            IWebElement BuyElement = this.Driver.FindElement(By.XPath(xpath));

            xpath = @"/div[2]/div[1]/span[1]/";
            IWebElement rateElement = BuyElement.FindElement(By.XPath(xpath));

            string currentPrice = rateElement.Text + rateElement.FindElement(By.XPath("/i[1]")).Text;

            float currentBuyPrice = float.Parse(currentPrice);
            DateTime timeStamp = DateTime.Now;

            BuyElement.Click();

            xpath = @"/html/body/div[8]/div/div/div/div/div/div/div[2]/div[1]/div[1]/div[2]/div[1]/input";
            IWebElement amountInputElement = this.Driver.FindElement(By.XPath(xpath));
            amountInputElement.Clear();
            amountInputElement.SendKeys(units.ToString());

            xpath = @"/html/body/div[8]/div/div/div/div/div/div/div[2]/div[1]/div[6]/div/span[1]/button";
            IWebElement openPositonElement = this.Driver.FindElement(By.XPath(xpath));

            //openPositonElement.Click();   //Danger Zone
            OpenPositionInfo info = new OpenPositionInfo(share, timeStamp, BuySell.Buy, units, currentBuyPrice);
            return info;
        }

        public OpenPositionInfo OpenBuyPosition(Share share, int units, int takeProfitInPercent)
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

            xpath = @"/html/body/div[8]/div/div/div/div/div/div/div[2]/div[1]/div[1]/div[2]/div[1]/input";
            IWebElement amountInputElement = this.Driver.FindElement(By.XPath(xpath));
            amountInputElement.Clear();
            amountInputElement.SendKeys(units.ToString());

            xpath = @"/html/body/div[8]/div/div/div/div/div/div/div[2]/div[1]/div[2]/div[1]/label";
            IWebElement checkBoxElement = this.Driver.FindElement(By.XPath(xpath));
            checkBoxElement.Click();

            xpath = @"/html/body/div[8]/div/div/div/div/div/div/div[2]/div[1]/div[2]/div[2]/div[1]/input";
            IWebElement takeProfitInputElement = this.Driver.FindElement(By.XPath(xpath));
            takeProfitInputElement.Clear();
            takeProfitInputElement.SendKeys(takeProfit.ToString());

            xpath = @"/html/body/div[8]/div/div/div/div/div/div/div[2]/div[1]/div[6]/div/span[1]/button";
            IWebElement openPositonElement = this.Driver.FindElement(By.XPath(xpath));


            //openPositonElement.Click();   //Danger Zone
            OpenPositionInfo info = new OpenPositionInfo(share, timeStamp, BuySell.Buy, units, currentBuyPrice, takeProfit);
            return info;
        }


        public OpenPositionInfo OpenSellPosition(Share share, int units)
        {
            SelectShare(share);

            return null;
        }

        public OpenPositionInfo OpenSellPosition(Share share, int units, int takeProfitInPercent)
        {
            SelectShare(share);

            string xpath = @"/html/body/div[1]/div[4]/div[1]/div[2]/div/div[1]/div[2]/div/div/div/div[2]/div[2]/div[1]/div[1]/table/tbody/tr/td/div/button";
            IWebElement SellElement = this.Driver.FindElement(By.XPath(xpath));

            xpath = @"/div[2]/div[1]/span[1]/";
            IWebElement rateElement = SellElement.FindElement(By.XPath(xpath));

            string currentPrice = rateElement.Text + rateElement.FindElement(By.XPath("/i[1]")).Text;

            float currentSellPrice = float.Parse(currentPrice);
            float takeProfit = GetTakeProfitValue(currentSellPrice, takeProfitInPercent);
            DateTime timeStamp = DateTime.Now;

            SellElement.Click();

            xpath = @"/html/body/div[8]/div/div/div/div/div/div/div[2]/div[1]/div[1]/div[2]/div[1]/input";
            IWebElement amountInputElement = this.Driver.FindElement(By.XPath(xpath));
            amountInputElement.Clear();
            amountInputElement.SendKeys(units.ToString());

            xpath = @"/html/body/div[8]/div/div/div/div/div/div/div[2]/div[1]/div[2]/div[1]/label";
            IWebElement checkBoxElement = this.Driver.FindElement(By.XPath(xpath));
            checkBoxElement.Click();

            xpath = @"/html/body/div[8]/div/div/div/div/div/div/div[2]/div[1]/div[2]/div[2]/div[1]/input";
            IWebElement takeProfitInputElement = this.Driver.FindElement(By.XPath(xpath));
            takeProfitInputElement.Clear();
            takeProfitInputElement.SendKeys(takeProfit.ToString());

            xpath = @"/html/body/div[8]/div/div/div/div/div/div/div[2]/div[1]/div[6]/div/span[1]/button";
            IWebElement openPositonElement = this.Driver.FindElement(By.XPath(xpath));

            //openPositonElement.Click();   //DangerZone

            OpenPositionInfo info = new OpenPositionInfo(share, timeStamp, BuySell.Sell, units, currentSellPrice);
            return info;
        }


        public float GetOpenPositionValue(OpenPositionInfo openPosition)
        {
            string xpath = @"/html/body/div[1]/div[4]/div[1]/div[1]/div[1]/div[2]/div/div/div/div[1]/div[2]/div/div[2]";
            IWebElement openPositionsElement = this.Driver.FindElement(By.XPath(xpath));
            openPositionsElement.Click();

            xpath = @"/html/body/div[1]/div[4]/div[1]/div[2]/div/div[1]/div[2]/div/div/div/div[2]/div/table/tbody";
            IWebElement rowsElement = this.Driver.FindElement(By.XPath(xpath));

            for (int i = 1; i < 100; i += 2)
            {
                xpath = string.Format(@"/tr[{0}]/td[1]/div", i);
                IWebElement rowElement = rowsElement.FindElement(By.XPath(xpath));
                if (rowElement.Text == GetShareName(openPosition.Share, true))
                {
                    xpath = string.Format(@"/tr[{0}]", i++);
                    break;
                }
            }

            IWebElement parentElement = rowsElement.FindElement(By.XPath(xpath));

            xpath = @"/td[1]/div";
            IWebElement[] positionElements = parentElement.FindElements(By.XPath(xpath)).ToArray();

            foreach (IWebElement positionElement in positionElements)
            {
                //Check the amount, timestamp and price propertys and return the current profit/lost
            }

            return 0;
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

        private string GetShareName(Share share, bool withSpace)
        {
            return !withSpace ? share.ToString().Remove(share.ToString().IndexOf('_'), 1) : share.ToString().Replace('_', ' ');
        }


        #endregion

    }
}
