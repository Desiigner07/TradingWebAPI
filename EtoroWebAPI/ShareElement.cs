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
	}
}
