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
            string loginXpath = "/html/body/div[1]/div/div[2]/span/div/div/div/div[2]/ul[1]/li[8]";
            IWebElement loginElement = this.Driver.FindElement(By.XPath(loginXpath));
            loginElement.Click();

            string inputNameXpath = "/html/body/div[1]/div[4]/div[3]/div/div/div/div/div/div/form/div/div[3]/div[1]/input";
            IWebElement inputNameElement = this.Driver.FindElement(By.XPath(inputNameXpath));

            string inputPwdXpath = "/html/body/div[1]/div[4]/div[3]/div/div/div/div/div/div/form/div/div[3]/div[2]/input";
            IWebElement inputPwdElement = this.Driver.FindElement(By.XPath(inputPwdXpath));

            string loginButtonXpath = "//*[@id=\"auth - button - login\"]";
            IWebElement loginButton = this.Driver.FindElement(By.XPath(loginButtonXpath));

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

        public void OpenVirtualMode()
        {
            throw new NotImplementedException();
        }
    }
}
