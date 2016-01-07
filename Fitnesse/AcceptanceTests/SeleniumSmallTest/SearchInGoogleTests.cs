using fitlibrary;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AcceptanceTests.SeleniumSmallTest
{
	public class SearchInGoogleTests : DoFixture
	{
		public void SearchAndWaitBySeleniumRemoteControl()
		{
			//not working on latest IE and FireFox versions
			ISelenium sel = new DefaultSelenium("localhost",4444, "*iehta", "http://www.google.com");
			sel.Start();
			sel.Open("http://www.google.com/");
			sel.Type("q", "FitNesse");
			sel.Click("btnG");
			sel.WaitForPageToLoad("3000");
		}

		public void SearchAndWait()
		{
			IWebDriver driver = new FirefoxDriver();

			//Notice navigation is slightly different than the Java version
			//This is because 'get' is a keyword in C#
			driver.Navigate().GoToUrl("http://www.google.com/");
			IWebElement query = driver.FindElement(By.Name("q"));
			query.SendKeys("FitNesse");
			IWebElement button = driver.FindElement(By.Name("btnG"));
			button.Click();
			Thread.Sleep(3000);
            driver.Quit();
		}
	}
}
