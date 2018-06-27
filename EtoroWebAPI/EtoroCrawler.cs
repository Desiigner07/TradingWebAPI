using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Interactions;

namespace EtoroWebAPI
{
	public class EtoroCrawler
	{
		private const string ETORO_URL = "https://www.etoro.com/";

		#region Properties 

		public InternetExplorerDriver Driver { get; private set; }

		#endregion


		private void StartBrowser()
		{
			this.Driver = new InternetExplorerDriver();
			this.Driver.Url = ETORO_URL;
		}
		
		public void Login(string username, string pwd)
		{
			IWebElement loginElement = this.Driver.FindElementByClassName("login");
			loginElement.Click();

			IWebElement usernameBox = this.Driver.FindElementById("username");
			IWebElement pwdBox = this.Driver.FindElementById("password");

			usernameBox.SendKeys(username);
			pwdBox.SendKeys(pwd);

			IWebElement loginButtonElement = this.Driver.FindElementById("login-sts-btn-sign-in");
			loginButtonElement.Click();
		}

	}
}
