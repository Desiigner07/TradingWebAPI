using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EtoroWebAPI
{
	public class ShareElement
	{
		public string DriverUrl { get; private set; }

		public IWebDriver Driver { get; private set; }

		public string Name { get; private set; }

		public ShareElement(string name, IWebDriver driver)
		{
			this.Name = name;
			this.Driver = driver;
			this.DriverUrl = driver.Url;
		}

		public float CurrentPrice
		{
			get
			{
				IWebElement priceElement = this.Driver.FindElement(By.ClassName("head-info-stats-value ng-binding negative")) == null ? this.Driver.FindElement(By.ClassName("head-info-stats-value ng-binding positive")) : this.Driver.FindElement(By.ClassName("head-info-stats-value ng-binding negative"));
				float price = float.Parse(priceElement.Text);
				return price;
			}
		}
		#region Open Buy Position 
		public void OpenBuyPosition(float amount)
		{
			IWebElement tradeButton = this.Driver.FindElement(By.Id("trade-button"));
			tradeButton.Click();


			IWebElement buyElement = this.Driver.FindElement(By.Id("execution-buy-button"));
			buyElement.Click();

			IWebElement inputElement = this.Driver.FindElement(By.Id("input"));
			inputElement.SendKeys(amount.ToString());
		}

		public void OpenBuyPosition(float amount, float takeProfit)
		{
			IWebElement tradeButton = this.Driver.FindElement(By.Id("trade-button"));
			tradeButton.Click();


			IWebElement buyElement = this.Driver.FindElement(By.Id("execution-buy-button"));
			buyElement.Click();

			IWebElement inputElement = this.Driver.FindElement(By.Id("input"));
			inputElement.SendKeys(amount.ToString());

			IWebElement takeProfitElement = this.Driver.FindElement(By.Id("execution-take-profit-tab-title-value"));
			takeProfitElement.Click();

			IWebElement plusStepperElement = this.Driver.FindElement(By.Id("plus-button"));

			IWebElement rateDataElement = this.Driver.FindElement(By.Id("execution-take-profit-rate-editing-info"));

			string percent = rateDataElement.Text.Remove((rateDataElement.Text).IndexOf('%'));
      
			while (float.Parse(percent) > takeProfit)
			{

			}
		}

		public void OpenBuyPosition(float amount, float takeProfit, float stopLoss)
		{
			IWebElement tradeButton = this.Driver.FindElement(By.Id("trade-button"));
			tradeButton.Click();


			IWebElement buyElement = this.Driver.FindElement(By.Id("execution-buy-button"));
			buyElement.Click();

			IWebElement inputElement = this.Driver.FindElement(By.Id("input"));
			inputElement.SendKeys(amount.ToString());
		}

		#endregion

		#region Open Sell Position

		public void OpenSellPosition(float amount)
		{
			IWebElement tradeButton = this.Driver.FindElement(By.Id("trade-button"));
			tradeButton.Click();

			IWebElement sellElement = this.Driver.FindElement(By.Id("execution-sell-button"));
			sellElement.Click();
			
			IWebElement inputElement = this.Driver.FindElement(By.Id("input"));
			inputElement.SendKeys(amount.ToString());
		}


		#endregion 
	}
}
