using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EtoroWebAPI
{
    class MarketsDotComCrawler : ICrawler
    {
        private const string MARKETS_URL = "https://www.markets.com/de/";

        public IWebDriver Driver { get; private set; }

        private bool browserStarted = false;


        private void StartBrowser()
        {
            if (!browserStarted)
            {
                this.Driver = new FirefoxDriver();
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

        }

        public void OpenBuyOrder(Share share, float takeProfit)
        {
            throw new NotImplementedException();
        }

        public void OpenBuyPosition(Share share)
        {
            throw new NotImplementedException();
        }

        public void OpenBuyPosition(Share share, float takeProfit)
        {
            throw new NotImplementedException();
        }

        public void OpenSellOrder(Share share, float takeProfit)
        {
            throw new NotImplementedException();
        }

        public void OpenSellPosition(Share share)
        {
            throw new NotImplementedException();
        }

        public void OpenSellPosition(Share share, float takeProfit)
        {
            throw new NotImplementedException();
        }

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
    }
}
