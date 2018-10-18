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
        private const string MARKETS_LOGIN_URL = "https://live-trader.markets.com/trading-platform/?lang=de#login";

        public List<OpenPositionInfo> OpenPositions { get; private set; }

        private Share _selectedShare = Share.None;
        public Share SelectedShare
        {
            get
            {
                if (_selectedShare != Share.None)
                {
                    string url = this.Driver.Url;
                    string selectedShare = url.Split('/').Last();

                    string expectedShare = GetShareName(_selectedShare, false);
                    if (selectedShare != expectedShare)
                    {
                        return Share.None;
                    }
                }

                return _selectedShare;
            }
            set
            {
                if (_selectedShare != value)
                {
                    _selectedShare = value;
                }
            }
        }
        public IWebDriver Driver { get; private set; }

        private bool DemoMode = true;

        public event EventHandler<OpenNewPositionEventArgs> OnOpenNewPosition;

        public Share TradingShare { get; private set; }

        public MarketsDotComCrawler(bool demoMode, Share share)
        {
            this.OpenPositions = new List<OpenPositionInfo>();
            this.DemoMode = demoMode;
            this.TradingShare = share;

            FirefoxOptions options = new FirefoxOptions()
            {
                BrowserExecutableLocation = @"C:\Program Files\Firefox Developer Edition\firefox.exe"
            };
            this.Driver = new FirefoxDriver(options);
        }

        #region Login 

        public void Login(string username, string pwd)
        {
            this.Driver.Url = MARKETS_LOGIN_URL;
            this.Delay();

            try
            {

                Try(() => Login_InputMail(username));
                Try(() => Login_InputPWD(pwd));

                string Xpath = "//*[@id=\"auth-button-login\"]";
                IWebElement loginButton = this.Driver.FindElement(By.XPath(Xpath));
                loginButton.Click();

            }
            catch (NoSuchElementException)
            {
                //Wait for load
                this.Delay();
                Login(username, pwd);
            }
            finally
            {
                this.Delay(6000);
            }
        }

        public bool TryRemoveStartDialog()
        {
            this.Delay(4000);
            try
            {
                // string xpath = @"/html/body/div[8]/div/div/div/div/div/div/div[3]/button";
                string xpath = @"/html/body/div[4]/div/div/div/div/div/div/div[3]/button";
                IWebElement goToDemoElement = this.Driver.FindElement(By.XPath(xpath));
                goToDemoElement.Click();
                this.Delay();
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        public bool IsConnected()
        {
            if (TryRemoveConnectionLostDialog())
            {
                this.Delay();
                this.SelectShareWithUrl(this.TradingShare);
                return false;
            }
            else
                return true;
        }
        private bool TryRemoveConnectionLostDialog()
        {
            this.Delay(200);
            try
            {

                string xpath = @"/html/body/div[8]/div/div/div/div/div/div/div[3]/button";
                IWebElement connectionLostElement = this.Driver.FindElement(By.XPath(xpath));
                connectionLostElement.Click();
                this.Delay();
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        private void Login_InputMail(string mail)
        {
            string Xpath = "/html/body/div[1]/div[4]/div[3]/div/div/div/div/div/div/form/div/div[3]/div[1]/input";
            IWebElement inputNameElement = this.Driver.FindElement(By.XPath(Xpath));
            inputNameElement.SendKeys(mail);
        }

        private void Login_InputPWD(string pwd)
        {
            string Xpath = "/html/body/div[1]/div[4]/div[3]/div/div/div/div/div/div/form/div/div[3]/div[2]/input";
            IWebElement inputPwdElement = this.Driver.FindElement(By.XPath(Xpath));
            inputPwdElement.SendKeys(pwd);
        }
        #endregion

        #region Orders 

        public void OpenBuyOrder(Share share, int units, int takeProfitInPercent, float orderRate)
        {
            SelectShareWithUrl(share);

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

            xpath = @"/html/body/div[8]/div/div/div/div/div/div/div[2]/div[1]/div[4]";
            IWebElement parentOrderElement = this.Driver.FindElement(By.XPath(xpath));
            parentOrderElement.Click();

            xpath = @"/html/body/div[8]/div/div/div/div/div/div/div[2]/div[1]/div[4]/div[2]/div[2]/div[1]";
            IWebElement orderCheckboxElement = this.Driver.FindElement(By.XPath(xpath));
            orderCheckboxElement.Click();

            xpath = @"/html/body/div[8]/div/div/div/div/div/div/div[2]/div[1]/div[4]/div[2]/div[2]/div[2]/div[1]/input";
            IWebElement orderRateElement = this.Driver.FindElement(By.XPath(xpath));
            orderRateElement.Clear();
            orderRateElement.SendKeys(orderRate.ToString());

            xpath = @"/html/body/div[8]/div/div/div/div/div/div/div[2]/div[1]/div[6]/div/span[1]/button";
            IWebElement openOrderElement = this.Driver.FindElement(By.XPath(xpath));

            // openOrderElement.Click();
        }

        public void OpenSellOrder(Share share, int units, int takeProfitInPercent, float orderRate)
        {
            SelectShareWithUrl(share);

            string xpath = @"/html/body/div[1]/div[4]/div[1]/div[2]/div/div[1]/div[2]/div/div/div/div[2]/div[2]/div[1]/div[1]/table/tbody/tr/td/div/button";
            IWebElement SellElement = this.Driver.FindElement(By.XPath(xpath));

            xpath = @"/div[2]/div[1]/span[1]/";
            IWebElement rateElement = SellElement.FindElement(By.XPath(xpath));

            string currentPrice = rateElement.Text + rateElement.FindElement(By.XPath("/i[1]")).Text;

            float currentSellPrice = float.Parse(currentPrice);
            float takeProfit = GetPositiveValue(currentSellPrice, takeProfitInPercent);
            DateTime timeStamp = DateTime.Now;

            SellElement.Click();

            xpath = @"/html/body/div[8]/div/div/div/div/div/div/div[2]/div[1]/div[1]/div[2]/div[1]/input";
            IWebElement amountInputElement = this.Driver.FindElement(By.XPath(xpath));
            amountInputElement.Clear();
            amountInputElement.SendKeys(units.ToString());
        }
        #endregion 
        private object searchLock = new object();
        private void SelectShare(Share share)
        {
            if (SelectedShare == share)
            {
                return;
            }

            IWebElement nameInputElement = null;
            IWebElement searchElement = null;
            string xpath = @"/html/body/div[1]/div[4]/div[1]/div[1]/div[1]/div[1]/div/div[2]/div[3]/div/table/tbody/tr";
            string shareName = GetShareName(share, true);

            TryAndBreak(() =>
            {
                nameInputElement = this.Driver.FindElement(By.Id("search-block-input"));

                lock (searchLock)
                {
                    nameInputElement.Clear();
                    for (int i = 0; i < shareName.Length; i++)
                    {
                        this.Delay(100);
                        char c = shareName[i];
                        nameInputElement.SendKeys(c.ToString());
                    }
                }
                Delay(300);

                searchElement = this.Driver.FindElement(By.XPath(xpath));
                searchElement.Click();
            });

            this.SelectedShare = share;
            this.Delay(500);
        }

        private void SelectShareWithUrl(Share share)
        {
            //SelectShare(share);
            //return;
            if (SelectedShare == share)
            {
                return;
            }

            string url = UrlStorage.Instance.GetShareUrl(share, DemoMode);
            string xpath = UrlStorage.Instance.GetXPath(share);

            try
            {
                this.Driver.Navigate().Refresh();
                this.Driver.Navigate().GoToUrl(url);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return;
            }

            this.Delay(3000);
            //IWebElement rowElement = this.Driver.FindElement(By.XPath(xpath));
            xpath = xpath + "/td[8]/div/div[2]";

            TryAndBreak(() =>
            {
                IWebElement buttonElement = this.Driver.FindElement(By.XPath(xpath));
                buttonElement.Click();
            });

            this.SelectedShare = share;
            this.Delay(1000);
        }

        public float? GetCurrentBuyPrice(Share share)
        {
            SelectShareWithUrl(share);

            string xpath = string.Empty;
            string currentPrice = string.Empty;
            IWebElement rateElement = null;
            float? price = null;

            TryAndBreak(() =>
            {
                //   xpath = @"/html/body/div[1]/div[4]/div[1]/div[2]/div/div[1]/div[2]/div/div/div/div[2]/div[2]/div[1]/div[2]/table/tbody/tr/td/div/button/div[2]/div/span";
                xpath = @"/html/body/div[1]/div[4]/div[1]/div[2]/div/div[1]/div[3]/div/div[2]/div[2]/div[1]/div[2]/table/tbody/tr/td/div/button/div[2]/div/span";
                rateElement = this.Driver.FindElement(By.XPath(xpath));
                currentPrice = rateElement.Text.Replace('.', ',');
                price = float.Parse(currentPrice);
            });

            return price;
        }

        public float? GetCurrentSellPrice(Share share)
        {
            SelectShareWithUrl(share);

            string xpath = string.Empty;
            string currentPrice = string.Empty;
            IWebElement rateElement = null;
            float? price = null;

            TryAndBreak(() =>
            {
                //xpath = @"/html/body/div[1]/div[4]/div[1]/div[2]/div/div[1]/div[2]/div/div/div/div[2]/div[2]/div[1]/div[1]/table/tbody/tr/td/div/button/div[2]/div/span";
                xpath = @"/html/body/div[1]/div[4]/div[1]/div[2]/div/div[1]/div[3]/div/div[2]/div[2]/div[1]/div[1]/table/tbody/tr/td/div/button/div[2]/div/span";
                rateElement = this.Driver.FindElement(By.XPath(xpath));
                currentPrice = rateElement.Text.Replace('.', ',');
                price = float.Parse(currentPrice);
            });

            return price;
        }

        #region Open Buy Position

        public OpenPositionInfo OpenBuyPosition(Share share, int units)
        {
            SelectShareWithUrl(share);
            this.Delay();

            string xpath = @"/html/body/div[1]/div[4]/div[1]/div[2]/div/div[1]/div[2]/div/div/div/div[2]/div[2]/div[1]/div[2]/table/tbody/tr/td/div/button";
            IWebElement BuyElement = this.Driver.FindElement(By.XPath(xpath));

            xpath = @"/html/body/div[1]/div[4]/div[1]/div[2]/div/div[1]/div[2]/div/div/div/div[2]/div[2]/div[1]/div[2]/table/tbody/tr/td/div/button/div[2]";
            IWebElement rateElement = this.Driver.FindElement(By.XPath(xpath));

            string currentPrice = rateElement.Text.Replace('.', ',');

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
            OnOpenNewPosition?.Invoke(this, new OpenNewPositionEventArgs(info));
            return info;
        }

        public OpenPositionInfo OpenBuyPosition(Share share, int units, int takeProfitInPercent)
        {
            SelectShareWithUrl(share);
            this.Delay();

            string xpath = @"/html/body/div[1]/div[4]/div[1]/div[2]/div/div[1]/div[2]/div/div/div/div[2]/div[2]/div[1]/div[2]/table/tbody/tr/td/div/button";
            IWebElement BuyElement = this.Driver.FindElement(By.XPath(xpath));

            xpath = @"/html/body/div[1]/div[4]/div[1]/div[2]/div/div[1]/div[2]/div/div/div/div[2]/div[2]/div[1]/div[2]/table/tbody/tr/td/div/button/div[2]";
            IWebElement rateElement = this.Driver.FindElement(By.XPath(xpath));

            string currentPrice = rateElement.Text.Replace('.', ',');

            float currentBuyPrice = float.Parse(currentPrice);
            float takeProfit = GetPositiveValue(currentBuyPrice, takeProfitInPercent);
            DateTime timeStamp = DateTime.Now;

            BuyElement.Click();

            xpath = @"/html/body/div[8]/div/div/div/div/div/div/div[2]/div[1]/div[1]/div[2]/div[1]/input";
            IWebElement amountInputElement = this.Driver.FindElement(By.XPath(xpath));
            amountInputElement.Clear();
            amountInputElement.SendKeys(units.ToString());

            //Get The Take Profit Checkbox element
            xpath = @"/html/body/div[8]/div/div/div/div/div/div/div[2]/div[1]/div[2]/div[1]/label";
            IWebElement checkBoxElement = this.Driver.FindElement(By.XPath(xpath));
            checkBoxElement.Click();

            //Get the Take Profit plus element
            xpath = @"/html/body/div[8]/div/div/div/div/div/div/div[2]/div[1]/div[2]/div[2]/div[2]/button[1]";
            IWebElement plusElement = this.Driver.FindElement(By.XPath(xpath));
            xpath = @"/html/body/div[8]/div/div/div/div/div/div/div[2]/div[1]/div[2]/div[2]/div[2]/button[2]";
            IWebElement minusElement = this.Driver.FindElement(By.XPath(xpath));

            xpath = @"/html/body/div[8]/div/div/div/div/div/div/div[2]/div[1]/div[2]/div[2]/div[1]/input";

            IWebElement takeProfitInputElement = this.Driver.FindElement(By.XPath(xpath));

            if (float.Parse(takeProfitInputElement.GetAttribute("value").Replace('.', ',')) < takeProfit)
            {
                while (float.Parse(takeProfitInputElement.GetAttribute("value").Replace('.', ',')) < takeProfit)
                {
                    plusElement.Click();
                    this.Delay(100);
                }
            }
            else if (float.Parse(takeProfitInputElement.GetAttribute("value").Replace('.', ',')) > takeProfit)
            {
                while (float.Parse(takeProfitInputElement.GetAttribute("value").Replace('.', ',')) > takeProfit)
                {
                    minusElement.Click();
                    this.Delay(100);
                }
            }

            xpath = @"/html/body/div[8]/div/div/div/div/div/div/div[2]/div[1]/div[6]/div/span[1]/button";
            IWebElement openPositionElement = this.Driver.FindElement(By.XPath(xpath));


            //openPositionElement.Click();   //Danger Zone
            OpenPositionInfo info = new OpenPositionInfo(share, timeStamp, BuySell.Buy, units, currentBuyPrice, takeProfit);
            OnOpenNewPosition?.Invoke(this, new OpenNewPositionEventArgs(info));
            return info;
        }

        public OpenPositionInfo OpenBuyPosition(Share share, int units, int takeProfitInPercent, int stopLossInPercent)
        {
            SelectShareWithUrl(share);


            string xpath = string.Empty;
            IWebElement buyElement = null;
            IWebElement rateElement = null;
            IWebElement checkBoxElement = null;
            IWebElement plusElement = null;
            IWebElement minusElement = null;
            IWebElement takeProfitInputElement = null;
            IWebElement stopLossInputElement = null;
            IWebElement openPositionElement = null;
            IWebElement amountInputElement = null;
            IWebElement closeElement = null;

            TryAndBreak(() =>
            {
                xpath = @"/html/body/div[1]/div[4]/div[1]/div[2]/div/div[1]/div[2]/div/div/div/div[2]/div[2]/div[1]/div[2]/table/tbody/tr/td/div/button";
                xpath = @"/html/body/div[1]/div[4]/div[1]/div[2]/div/div[1]/div[3]/div/div[2]/div[2]/div[1]/div[2]/table/tbody/tr/td/div/button";
                buyElement = this.Driver.FindElement(By.XPath(xpath));
            });

            TryAndBreak(() =>
            {
                xpath = @"/html/body/div[1]/div[4]/div[1]/div[2]/div/div[1]/div[2]/div/div/div/div[2]/div[2]/div[1]/div[2]/table/tbody/tr/td/div/button/div[2]";
                xpath = @"/html/body/div[1]/div[4]/div[1]/div[2]/div/div[1]/div[3]/div/div[2]/div[2]/div[1]/div[2]/table/tbody/tr/td/div/button/div[2]/div/span";
                rateElement = this.Driver.FindElement(By.XPath(xpath));
            });

            string currentPrice = rateElement.Text.Replace('.', ',');
            float currentBuyPrice = float.Parse(currentPrice);
            float takeProfit = GetPositiveValue(currentBuyPrice, takeProfitInPercent);
            float stopLoss = GetNegativeValue(currentBuyPrice, stopLossInPercent);
            DateTime timeStamp = DateTime.Now;

            buyElement.Click();
            this.Delay(100);

            Try(() =>
            {
                xpath = @"/html/body/div[8]/div/div/div/div/div/div/div[2]/div[1]/div[1]/div[2]/div[1]/input";
                amountInputElement = this.Driver.FindElement(By.XPath(xpath));
                amountInputElement.Clear();
                amountInputElement.SendKeys(units.ToString());
            });

            #region Input Take Profit

            Try(() =>
            {
                //Get The Take Profit Checkbox element
                xpath = @"/html/body/div[8]/div/div/div/div/div/div/div[2]/div[1]/div[2]/div[1]/label";
                checkBoxElement = this.Driver.FindElement(By.XPath(xpath));
                checkBoxElement.Click();
            });

            Try(() =>
            {
                //Get the Take Profit plus element
                xpath = @"/html/body/div[8]/div/div/div/div/div/div/div[2]/div[1]/div[2]/div[2]/div[2]/button[1]";
                plusElement = this.Driver.FindElement(By.XPath(xpath));
            });

            Try(() =>
            {
                xpath = @"/html/body/div[8]/div/div/div/div/div/div/div[2]/div[1]/div[2]/div[2]/div[2]/button[2]";
                minusElement = this.Driver.FindElement(By.XPath(xpath));
            });


            Try(() =>
            {
                xpath = @"/html/body/div[8]/div/div/div/div/div/div/div[2]/div[1]/div[2]/div[2]/div[1]/input";
                takeProfitInputElement = this.Driver.FindElement(By.XPath(xpath));
            });

            Try(() =>
            {
                if (float.Parse(takeProfitInputElement.GetAttribute("value").Replace('.', ',')) < takeProfit)
                {
                    while (float.Parse(takeProfitInputElement.GetAttribute("value").Replace('.', ',')) < takeProfit)
                    {
                        plusElement.Click();
                        this.Delay(100);
                    }
                }
                else if (float.Parse(takeProfitInputElement.GetAttribute("value").Replace('.', ',')) > takeProfit)
                {
                    while (float.Parse(takeProfitInputElement.GetAttribute("value").Replace('.', ',')) > takeProfit)
                    {
                        minusElement.Click();
                        this.Delay(100);
                    }
                }
            });
            #endregion

            #region Input Loss 
            Try(() =>
            {
                xpath = @"/html/body/div[8]/div/div/div/div/div/div/div[2]/div[1]/div[3]/div[1]/label";
                checkBoxElement = this.Driver.FindElement(By.XPath(xpath));
                checkBoxElement.Click();
            });

            Try(() =>
            {
                //Get the Take Profit plus element
                xpath = @"/html/body/div[8]/div/div/div/div/div/div/div[2]/div[1]/div[3]/div[2]/div[2]/button[1]";
                plusElement = this.Driver.FindElement(By.XPath(xpath));
            });

            Try(() =>
            {
                xpath = @"/html/body/div[8]/div/div/div/div/div/div/div[2]/div[1]/div[3]/div[2]/div[2]/button[2]";
                minusElement = this.Driver.FindElement(By.XPath(xpath));
            });


            Try(() =>
            {
                xpath = @"/html/body/div[8]/div/div/div/div/div/div/div[2]/div[1]/div[3]/div[2]/div[1]/input";
                stopLossInputElement = this.Driver.FindElement(By.XPath(xpath));
            });

            Try(() =>
            {
                if (float.Parse(stopLossInputElement.GetAttribute("value").Replace('.', ',')) < stopLoss)
                {
                    while (float.Parse(stopLossInputElement.GetAttribute("value").Replace('.', ',')) < stopLoss)
                    {
                        plusElement.Click();
                        this.Delay(100);
                    }
                }
                else if (float.Parse(stopLossInputElement.GetAttribute("value").Replace('.', ',')) > stopLoss)
                {
                    while (float.Parse(stopLossInputElement.GetAttribute("value").Replace('.', ',')) > stopLoss)
                    {
                        minusElement.Click();
                        this.Delay(100);
                    }
                }
            });
            #endregion

            Try(() =>
            {
                xpath = @"/html/body/div[8]/div/div/div/div/div/div/div[2]/div[1]/div[6]/div/span[1]/button";
                openPositionElement = this.Driver.FindElement(By.XPath(xpath));
                //openPositionElement.Click();   //Danger Zone
            });

            //Just for training
            Try(() =>
            {
                xpath = @"/html/body/div[8]/div/div/div/div/div/div/div[1]/div[3]/div[2]";
                closeElement = this.Driver.FindElement(By.XPath(xpath));
                closeElement.Click();
            });

            OpenPositionInfo info = new OpenPositionInfo(share, timeStamp, BuySell.Buy, units, currentBuyPrice, takeProfit, stopLoss);
            OnOpenNewPosition?.Invoke(this, new OpenNewPositionEventArgs(info));
            return info;
        }
        #endregion

        #region 
        public OpenFakePositionInfo OpenFakePosition(Share share, int units, int takeProfitInPercent, int stopLossInPercent)
        {
            SelectShareWithUrl(share);

            string xpath = string.Empty;
            IWebElement buyElement = null;
            IWebElement rateElement = null;

            TryAndBreak(() =>
            {
                xpath = @"/html/body/div[1]/div[4]/div[1]/div[2]/div/div[1]/div[2]/div/div/div/div[2]/div[2]/div[1]/div[2]/table/tbody/tr/td/div/button";
                xpath = @"/html/body/div[1]/div[4]/div[1]/div[2]/div/div[1]/div[3]/div/div[2]/div[2]/div[1]/div[2]/table/tbody/tr/td/div/button";
                buyElement = this.Driver.FindElement(By.XPath(xpath));
            });

            TryAndBreak(() =>
            {
                xpath = @"/html/body/div[1]/div[4]/div[1]/div[2]/div/div[1]/div[2]/div/div/div/div[2]/div[2]/div[1]/div[2]/table/tbody/tr/td/div/button/div[2]";
                xpath = @"/html/body/div[1]/div[4]/div[1]/div[2]/div/div[1]/div[3]/div/div[2]/div[2]/div[1]/div[2]/table/tbody/tr/td/div/button/div[2]/div/span";
                rateElement = this.Driver.FindElement(By.XPath(xpath));
            });

            string currentPrice = rateElement.Text.Replace('.', ',');
            float currentBuyPrice = float.Parse(currentPrice);
            float takeProfit = GetPositiveValue(currentBuyPrice, takeProfitInPercent);
            float stopLoss = GetNegativeValue(currentBuyPrice, stopLossInPercent);
            DateTime timeStamp = DateTime.Now;

            OpenFakePositionInfo info = new OpenFakePositionInfo(share, currentBuyPrice, takeProfit, stopLoss);
            OnOpenNewPosition?.Invoke(this, new OpenNewPositionEventArgs(info));
            return info;
        }
        #endregion

        #region Sell
        public OpenPositionInfo OpenSellPosition(Share share, int units)
        {
            SelectShareWithUrl(share);

            return null;
        }

        public OpenPositionInfo OpenSellPosition(Share share, int units, int takeProfitInPercent)
        {
            SelectShareWithUrl(share);

            string xpath = @"/html/body/div[1]/div[4]/div[1]/div[2]/div/div[1]/div[2]/div/div/div/div[2]/div[2]/div[1]/div[1]/table/tbody/tr/td/div/button";
            IWebElement SellElement = this.Driver.FindElement(By.XPath(xpath));

            xpath = @"/html/body/div[1]/div[4]/div[1]/div[2]/div/div[1]/div[2]/div/div/div/div[2]/div[2]/div[1]/div[2]/table/tbody/tr/td/div/button/div[2]";
            IWebElement rateElement = this.Driver.FindElement(By.XPath(xpath));

            string currentPrice = rateElement.Text.Replace('.', ',');

            float currentSellPrice = float.Parse(currentPrice);
            float takeProfit = GetPositiveValue(currentSellPrice, takeProfitInPercent);

            DateTime timeStamp = DateTime.Now;

            SellElement.Click();

            Try(() =>
            {
                xpath = @"/html/body/div[8]/div/div/div/div/div/div/div[2]/div[1]/div[1]/div[2]/div[1]/input";
                IWebElement amountInputElement = this.Driver.FindElement(By.XPath(xpath));
                amountInputElement.Clear();
                amountInputElement.SendKeys(units.ToString());
            });

            xpath = @"/html/body/div[8]/div/div/div/div/div/div/div[2]/div[1]/div[2]/div[1]/label";
            IWebElement checkBoxElement = this.Driver.FindElement(By.XPath(xpath));
            checkBoxElement.Click();

            xpath = @"/html/body/div[8]/div/div/div/div/div/div/div[2]/div[1]/div[2]/div[2]/div[1]/input";
            IWebElement takeProfitInputElement = this.Driver.FindElement(By.XPath(xpath));
            takeProfitInputElement.Clear();

            //Get the Take Profit plus element
            xpath = @"/html/body/div[8]/div/div/div/div/div/div/div[2]/div[1]/div[2]/div[2]/div[2]/button[1]";
            IWebElement plusElement = this.Driver.FindElement(By.XPath(xpath));
            xpath = @"/html/body/div[8]/div/div/div/div/div/div/div[2]/div[1]/div[2]/div[2]/div[2]/button[2]";
            IWebElement minusElement = this.Driver.FindElement(By.XPath(xpath));

            if (float.Parse(takeProfitInputElement.GetAttribute("value").Replace('.', ',')) < takeProfit)
            {
                while (float.Parse(takeProfitInputElement.GetAttribute("value").Replace('.', ',')) < takeProfit)
                {
                    plusElement.Click();
                    this.Delay(100);
                }
            }
            else if (float.Parse(takeProfitInputElement.GetAttribute("value").Replace('.', ',')) > takeProfit)
            {
                while (float.Parse(takeProfitInputElement.GetAttribute("value").Replace('.', ',')) > takeProfit)
                {
                    minusElement.Click();
                    this.Delay(100);
                }
            }

            xpath = @"/html/body/div[8]/div/div/div/div/div/div/div[2]/div[1]/div[6]/div/span[1]/button";
            IWebElement openPositonElement = this.Driver.FindElement(By.XPath(xpath));

            //openPositonElement.Click();   //DangerZone

            OpenPositionInfo info = new OpenPositionInfo(share, timeStamp, BuySell.Sell, units, currentSellPrice);
            OnOpenNewPosition?.Invoke(this, new OpenNewPositionEventArgs(info));
            return info;
        }

        public OpenPositionInfo OpenSellPosition(Share share, int units, int takeProfitInPercent, int stopLossInPercent)
        {
            SelectShareWithUrl(share);

            string xpath = string.Empty;
            IWebElement sellElement = null;
            IWebElement rateElement = null;
            IWebElement checkBoxElement = null;
            IWebElement plusElement = null;
            IWebElement minusElement = null;
            IWebElement takeProfitInputElement = null;
            IWebElement stopLossInputElement = null;
            IWebElement openPositionElement = null;
            IWebElement amountInputElement = null;
            IWebElement closeElement = null;

            TryAndBreak(() =>
            {
                xpath = @"/html/body/div[1]/div[4]/div[1]/div[2]/div/div[1]/div[2]/div/div/div/div[2]/div[2]/div[1]/div[1]/table/tbody/tr/td/div/button";
                xpath = @"/html/body/div[1]/div[4]/div[1]/div[2]/div/div[1]/div[3]/div/div[2]/div[2]/div[1]/div[1]/table/tbody/tr/td/div/button";
                sellElement = this.Driver.FindElement(By.XPath(xpath));
            });

            TryAndBreak(() =>
            {
                xpath = @"/html/body/div[1]/div[4]/div[1]/div[2]/div/div[1]/div[2]/div/div/div/div[2]/div[2]/div[1]/div[2]/table/tbody/tr/td/div/button/div[2]";
                xpath = @"/html/body/div[1]/div[4]/div[1]/div[2]/div/div[1]/div[3]/div/div[2]/div[2]/div[1]/div[1]/table/tbody/tr/td/div/button/div[2]/div/span";
                rateElement = this.Driver.FindElement(By.XPath(xpath));
            });

            string currentPrice = rateElement.Text.Replace('.', ',');
            float currentSellPrice = float.Parse(currentPrice);
            float takeProfit = GetNegativeValue(currentSellPrice, takeProfitInPercent);
            float stopLoss = GetPositiveValue(currentSellPrice, stopLossInPercent);
            DateTime timeStamp = DateTime.Now;

            sellElement.Click();
            this.Delay(100);

            Try(() =>
            {
                xpath = @"/html/body/div[8]/div/div/div/div/div/div/div[2]/div[1]/div[1]/div[2]/div[1]/input";
                amountInputElement = this.Driver.FindElement(By.XPath(xpath));
                amountInputElement.Clear();
                amountInputElement.SendKeys(units.ToString());
            });


            #region Input Profit
            Try(() =>
            {
                xpath = @"/html/body/div[8]/div/div/div/div/div/div/div[2]/div[1]/div[2]/div[1]/label";
                checkBoxElement = this.Driver.FindElement(By.XPath(xpath));
                checkBoxElement.Click();
            });

            Try(() =>
            {
                //Get the Take Profit plus element
                xpath = @"/html/body/div[8]/div/div/div/div/div/div/div[2]/div[1]/div[2]/div[2]/div[2]/button[1]";
                plusElement = this.Driver.FindElement(By.XPath(xpath));
            });

            Try(() =>
            {
                xpath = @"/html/body/div[8]/div/div/div/div/div/div/div[2]/div[1]/div[2]/div[2]/div[2]/button[2]";
                minusElement = this.Driver.FindElement(By.XPath(xpath));
            });

            Try(() =>
            {
                xpath = @"/html/body/div[8]/div/div/div/div/div/div/div[2]/div[1]/div[2]/div[2]/div[1]/input";
                takeProfitInputElement = this.Driver.FindElement(By.XPath(xpath));
            });

            Try(() =>
            {
                if (float.Parse(takeProfitInputElement.GetAttribute("value").Replace('.', ',')) < takeProfit)
                {
                    while (float.Parse(takeProfitInputElement.GetAttribute("value").Replace('.', ',')) < takeProfit)
                    {
                        plusElement.Click();
                        this.Delay(100);
                    }
                }
                else if (float.Parse(takeProfitInputElement.GetAttribute("value").Replace('.', ',')) > takeProfit)
                {
                    while (float.Parse(takeProfitInputElement.GetAttribute("value").Replace('.', ',')) > takeProfit)
                    {
                        minusElement.Click();
                        this.Delay(100);
                    }
                }
            });
            #endregion

            #region Input Loss 
            Try(() =>
            {
                xpath = @"/html/body/div[8]/div/div/div/div/div/div/div[2]/div[1]/div[3]/div[1]/label";
                checkBoxElement = this.Driver.FindElement(By.XPath(xpath));
                checkBoxElement.Click();
            });

            Try(() =>
            {
                //Get the Take Profit plus element
                xpath = @"/html/body/div[8]/div/div/div/div/div/div/div[2]/div[1]/div[3]/div[2]/div[2]/button[1]";
                plusElement = this.Driver.FindElement(By.XPath(xpath));
            });
            Try(() =>
            {
                xpath = @"/html/body/div[8]/div/div/div/div/div/div/div[2]/div[1]/div[3]/div[2]/div[2]/button[2]";
                minusElement = this.Driver.FindElement(By.XPath(xpath));
            });
            Try(() =>
            {
                xpath = @"/html/body/div[8]/div/div/div/div/div/div/div[2]/div[1]/div[3]/div[2]/div[1]/input";
                stopLossInputElement = this.Driver.FindElement(By.XPath(xpath));
            });
            Try(() =>
            {
                if (float.Parse(stopLossInputElement.GetAttribute("value").Replace('.', ',')) < stopLoss)
                {
                    while (float.Parse(stopLossInputElement.GetAttribute("value").Replace('.', ',')) < stopLoss)
                    {
                        plusElement.Click();
                        this.Delay(100);
                    }
                }
                else if (float.Parse(stopLossInputElement.GetAttribute("value").Replace('.', ',')) > stopLoss)
                {
                    while (float.Parse(stopLossInputElement.GetAttribute("value").Replace('.', ',')) > stopLoss)
                    {
                        minusElement.Click();
                        this.Delay(100);
                    }
                }
            });
            #endregion

            Try(() =>
            {
                xpath = @"/html/body/div[8]/div/div/div/div/div/div/div[2]/div[1]/div[6]/div/span[1]/button";
                openPositionElement = this.Driver.FindElement(By.XPath(xpath));
                //openPositonElement.Click();   //DangerZone
            });

            //Just for training
            Try(() =>
            {
                xpath = @"/html/body/div[8]/div/div/div/div/div/div/div[1]/div[3]/div[2]";
                closeElement = this.Driver.FindElement(By.XPath(xpath));
                closeElement.Click();
            });

            OpenPositionInfo info = new OpenPositionInfo(share, timeStamp, BuySell.Sell, units, currentSellPrice, takeProfit, stopLoss);
            OnOpenNewPosition?.Invoke(this, new OpenNewPositionEventArgs(info));
            return info;
        }

        #endregion

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

                xpath = @"/div[1]/div[2]/span[1]/span[1]";
                string text = positionElement.FindElement(By.XPath(xpath)).Text;

                int amount = int.Parse(text.Split('@')[0]);
                int rate = int.Parse(text.Split('@')[1]);

                xpath = @"/div[1]/div[2]/span[1]/span[3]";       //Just find Element if it had a takeProfit value
                IWebElement timestampElement = positionElement.FindElement(By.XPath(xpath));

                if (timestampElement == null)
                {
                    xpath = @"/div[1]/div[2]/span[1]/span[2]";
                    timestampElement = positionElement.FindElement(By.XPath(xpath));
                }

                text = timestampElement.Text;

                DateTime timeStamp = DateTime.Parse(text);


                if (openPosition.Amount == amount && openPosition.Rate == rate && openPosition.TimeStamp == timeStamp)
                {
                    xpath = @"/div[1]/div[1]/span[1]";

                    IWebElement currentProfitLossValueElement = positionElement.FindElement(By.XPath(xpath));

                    float currentProfitLossValue = float.Parse(currentProfitLossValueElement.Text);    //Critical Point
                    return currentProfitLossValue;
                }

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
            this.Delay();
        }

        public void GoToReal()
        {
            string realUrl = "https://live-trader.markets.com/trading-platform/#trading/Energy/";

            this.Driver.Url = realUrl;
            this.Delay();
        }

        #endregion

        #region Helpers
        private void Try(Action action)
        {
            try
            {
                action.Invoke();
            }
            catch (StaleElementReferenceException)
            {
                Try(() => action.Invoke());
            }
            catch (NoSuchElementException)
            {
                this.Delay(200);
                Try(() => action.Invoke());
            }
        }

        int tryCounter = 0;
        private void TryAndBreak(Action action)
        {
            try
            {
                try
                {
                    action.Invoke();
                }
                catch (StaleElementReferenceException)
                {
                    tryCounter++;
                    if (tryCounter <= 3)
                    {
                        TryAndBreak(() => action.Invoke());
                    }
                    else
                    {
                        throw new NotOnExpectedPageException();
                    }
                }
                catch (NoSuchElementException)
                {
                    tryCounter++;
                    if (tryCounter <= 3)
                    {
                        this.Delay(200);
                        TryAndBreak(() => action.Invoke());
                    }
                    else
                    {
                        throw new NotOnExpectedPageException();
                    }
                }
                finally
                {
                    tryCounter = 0;
                }
            }
            catch (NotOnExpectedPageException)
            {
                Share expectedShare = this.SelectedShare;
                this.SelectedShare = Share.None;
                //SelectShareWithUrl(expectedShare);
                SelectShareWithUrl(expectedShare);
            }
        }

        private void Delay(int delay = 2500)
        {
            System.Threading.Thread.Sleep(delay);
        }

        public static float GetPositiveValue(float currentPrice, int perc)
        {
            float percent = (float)perc / 100f;

            float takeProfit = (currentPrice * percent) / 10;               //10 = amount

            takeProfit = (float)Math.Round((Decimal)takeProfit, 2);

            return currentPrice + takeProfit;

        }

        public static float GetNegativeValue(float currentPrice, int perc)
        {
            float percent = (float)perc / 100f;

            float takeProfit = (currentPrice * percent) / 10;               //10 = amount

            takeProfit = (float)Math.Round((Decimal)takeProfit, 2);

            return currentPrice - takeProfit;

        }

        private string GetShareName(Share share, bool withSpace)
        {
            return !withSpace ? share.ToString().Remove(share.ToString().IndexOf('_'), 1) : share.ToString().Replace('_', ' ');
        }

        public void Refresh()
        {
            Share share = this.SelectedShare;
            this.SelectedShare = Share.None;
            this.SelectShareWithUrl(share);
            //this.SelectShare(share);
            this.Delay(2000);
        }


        #endregion

    }
}
