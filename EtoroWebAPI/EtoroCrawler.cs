using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Interactions;

namespace EtoroWebAPI
{
	public class EtoroCrawler
	{
		private const string ETORO_URL = "https://www.etoro.com/";

		#region Properties 

		public IWebDriver Driver { get; private set; }

		#region Menu WebElements

		private IWebElement _watchlistElement;
		public IWebElement WatchlistElement
		{
			get
			{
				if (_watchlistElement == null)
				{
					_watchlistElement = this.Driver.FindElements(By.ClassName("i-menu-link")).First(e => e.GetAttribute("ng-href") == "/watchlists");
				}
				return _watchlistElement;
			}
		}

		private IWebElement _portfolioElement;
		public IWebElement PortfolioElement
		{
			get
			{
				if (_portfolioElement == null)
				{
					_portfolioElement = this.Driver.FindElements(By.ClassName("i-menu-link")).First(e => e.GetAttribute("ng-href") == "/app/portfolio-page");
				}
				return _portfolioElement;
			}
		}


		#endregion

		#endregion


		private void StartBrowser()
		{
			this.Driver = new FirefoxDriver();
			this.Driver.Url = ETORO_URL;
		}

		public void Login(string username, string pwd)
		{
			IWebElement loginElement = this.Driver.FindElement(By.ClassName("login"));
			loginElement.Click();

			IWebElement usernameBox = this.Driver.FindElement(By.Id("username"));
			IWebElement pwdBox = this.Driver.FindElement(By.Id("password"));

			usernameBox.SendKeys(username);
			pwdBox.SendKeys(pwd);

			IWebElement loginButtonElement = this.Driver.FindElement(By.Id("login-sts-btn-sign-in"));
			loginButtonElement.Click();
		}

		public void OpenVirtualMode()
		{
			IWebElement dropdownElement = this.Driver.FindElement(By.ClassName("i-menu-link-mode-demo dropdown-menu ng-binding ng-scope"));
			dropdownElement.Click();

			IWebElement virtItemElement = this.Driver.FindElement(By.ClassName("drop-select-box-option"));
			virtItemElement.Click();

			IWebElement goToVirtButton = this.Driver.FindElement(By.ClassName("w-sm-footer-button e-btn-big dark ng-binding"));
			goToVirtButton.Click();
		}

		public void GoToWatchlist()
		{
			if (this.WatchlistElement != null)
			{
				this.WatchlistElement.Click();
			}
			else
			{
				this.Driver.Url = "https://www.Etoro.com/watchlists";
			}
		}

		public void GoToPortfolio()
		{
			if (this.PortfolioElement != null)
			{
				this.PortfolioElement.Click();
			}
			else
			{
				this.Driver.Url = "https://www.etoro.com/portfolio";
			}
		}

		public void SelectShareElement(string name)
		{
			IWebElement[] shareElements = this.Driver.FindElements(By.Id("watchlist-list-instruments-list")).ToArray();
			IWebElement shareElement = shareElements.Where(e => e.FindElement(By.Id("watchlist-item-list-instrument-name")).Text.ToLower() == name.ToLower()).FirstOrDefault();
			if (shareElement == null)
			{
				shareElement.Click();
			}
			else
			{
				this.Driver.Url = "https://www.etoro.com/markets/" + name;
			}
		}
	}
}
